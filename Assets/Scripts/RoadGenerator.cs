using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Func(aka. Delegate)

using RegionId = UnityEngine.Vector2Int;

public class RoadGenerator : MonoBehaviour
{
    /**
     * Regions: used to plan the road route in the general-generation phase
     */

    [Header("References")]
    public GameObject prefabSeg;

    [Header("Road Generation Params")]
    public bool isInitialized;
    public int numBlocksHorizontal = 10;
    public int numBlocksVectical = 10;

    // Test params
    [Header("Testing Params")]
    public int numFold;
    public float lengthSegment;
    public float widthSegment;
    public float angleStartDeg;
    public float maxAngleChangeDeg;
    public Vector2 startPoint;


    // Data
    float subregionWidth;
    float subregionLength;
    Region[,] subRegions;
    RegionId regionPlayerStart;
    // // road data
    // List<RegionId> roadRegions = new List<RegionId>();
    // List<Vector2> roadControlPointsFirstPass = new List<Vector2>();
    // List<RoadSegment> roadSegments;


    void Awake()
    {
        isInitialized = false;
    }

    void Start()
    {
        // TestGenerate();
    }

    public void Initialize(World world)
    {
        subregionWidth = world.worldWidth / (float)numBlocksHorizontal;
        subregionLength = world.worldLength / (float)numBlocksVectical;

        subRegions = GenerateRegions(numBlocksHorizontal, numBlocksVectical, subregionWidth, subregionLength);
        isInitialized = true;
    }

    public void GenerateRoad(World world, Vector2 positionPlayerStart)
    {
        if (!isInitialized)
        {
            Debug.Log("ERROR: trying to use uninitialized RoadGenerator.");
            return;
        }

        // use a middle point as the player start point
        regionPlayerStart = Vector2Int.FloorToInt(
                                                new Vector2(positionPlayerStart.x / subregionWidth,
                                                            positionPlayerStart.y / subregionLength)
                                                );
        Debug.Log(regionPlayerStart);

        // road data
        List<RegionId> roadRegions = new List<RegionId>();
        List<Vector2> roadControlPointsFirstPass = new List<Vector2>();
        RoadSegment roadSegments;

        PickRoadTiles(ref roadRegions, ref roadControlPointsFirstPass, regionPlayerStart);
        PerturbControlPoints(ref roadRegions, ref roadControlPointsFirstPass, world);
        roadSegments = GenerateRoadGameObjects(ref roadRegions, ref roadControlPointsFirstPass, GenerationMode.CUBINC_BEZIER_SPLINE_C2);
    }

    private Region[,] GenerateRegions(int numBlocksHorizontal, int numBlocksVectical, float width, float length)
    {
        Region[,] ret = new Region[numBlocksHorizontal, numBlocksVectical];
        for (int i = 0; i < numBlocksHorizontal; i++)
        {
            for (int j = 0; j < numBlocksHorizontal; j++)
            {
                RegionId index = new RegionId(i, j);
                Vector2 minCorner = new Vector2(i * width, j * length);
                Vector2 maxCorner = new Vector2((i + 1) * width, (j + 1) * length);
                ret[i, j] = new Region(index, minCorner, maxCorner);
            }
        }

        return ret;
    }

    private void PickRoadTiles(ref List<RegionId> roadRegions, ref List<Vector2> roadControlPointsFirstPass, RegionId startRegion)
    {
        // method: uniformly random
        // Choose the next tile from 4 directions.
        // List<RegionId> ret = new List<RegionId>();
        GetRegion(startRegion).hasRoad = true;
        roadRegions.Add(startRegion);
        roadControlPointsFirstPass.Add(GetRegion(startRegion).centerPoint); 
        List<RegionId> nextStepOptions = GetAdjacentRegionsWithFilter(startRegion, FILTER_IS_EMPTY);
        while (nextStepOptions.Count > 0)
        {
            RegionId regionSelected = nextStepOptions[UnityEngine.Random.Range(0, nextStepOptions.Count)];
            GetRegion(regionSelected).hasRoad = true;
            roadRegions.Add(regionSelected);
            roadControlPointsFirstPass.Add(GetRegion(regionSelected).centerPoint); 
            nextStepOptions = GetAdjacentRegionsWithFilter(regionSelected, FILTER_IS_EMPTY);
        }
        // note: roadRegions.Count can be used as a control for difficulty
        // do
        // {
        // } while (roadRegions.Count < 20);// can crush the program somtimes, especially when number is large
    }

    private void PerturbControlPoints(ref List<RegionId> roadRegions, ref List<Vector2> roadControlPointsFirstPass, World world)
    {
        // 2f is very great;
        float degreeRandom = 2f;
        float width = world.worldWidth / (float)numBlocksHorizontal / degreeRandom;
        float length = world.worldLength / (float)numBlocksVectical / degreeRandom;

        for (int i = 1; i < roadControlPointsFirstPass.Count - 1; i++)
        {
            Vector2 dir;
            if (UnityEngine.Random.Range(0, 1f) > 0.75f)
                dir = Vector2.up;
            else if (UnityEngine.Random.Range(0, 1f) > 0.5f)
                dir = Vector2.down;
            else if (UnityEngine.Random.Range(0, 1f) > 0.25f)
                dir = Vector2.left;
            else
                dir = Vector2.right;
            
            roadControlPointsFirstPass[i] += UnityEngine.Random.Range(-width, width) * dir;
        }
    }

   public enum GenerationMode
    {
        CENTER_POINT,
        CUBINC_BEZIER_SPLINE_C0,
        CUBINC_BEZIER_SPLINE_C1,
        CUBINC_BEZIER_SPLINE_C2
    }
    private RoadSegment GenerateRoadGameObjects(ref List<RegionId> roadRegions, ref List<Vector2> roadControlPointsFirstPass, GenerationMode generationMode, bool visualize=false)
    {
        // float widthSegment = 0.5f;

        // 补齐成3
        if (generationMode == GenerationMode.CUBINC_BEZIER_SPLINE_C0
            || generationMode == GenerationMode.CUBINC_BEZIER_SPLINE_C1
            || generationMode == GenerationMode.CUBINC_BEZIER_SPLINE_C2
            )
        {
            int reminder = (roadControlPointsFirstPass.Count - 1) % 3;
            Vector2 dumbPoint = roadControlPointsFirstPass[roadControlPointsFirstPass.Count - 1];
            for (int i = 0; i < 3 - reminder; i++)
            {
                roadControlPointsFirstPass.Add(dumbPoint);
            }
        }

        // Generate
        List<Vector2> spline = new List<Vector2>();
        if (visualize || generationMode == GenerationMode.CENTER_POINT)
        {
            spline = roadControlPointsFirstPass;
        }
        else if (generationMode == GenerationMode.CUBINC_BEZIER_SPLINE_C0)
        {
            spline = BezierSpline.CubicSplineC0(roadControlPointsFirstPass);
        }
        else if (generationMode == GenerationMode.CUBINC_BEZIER_SPLINE_C1)
        {
            spline = BezierSpline.CubicSplineC1(roadControlPointsFirstPass);
        }   
        else if (generationMode == GenerationMode.CUBINC_BEZIER_SPLINE_C2)
        {
            spline = BezierSpline.CubicSplineC2(roadControlPointsFirstPass);
        }   

        // Draw
        RoadSegment road;
        // for (int i = 0; i < spline.Count - 1; i++)
        // {
        //     Color segColor = Color.Lerp(Color.red, Color.white, (i + 1.0f) / (spline.Count - 1));
        //     RoadSegment roadSeg = prefabSeg.GetComponent<RoadSegment>().Create(0, spline[i], spline[i+1], widthSegment, segColor);
        //     road.Add(roadSeg);
        // }
        road = prefabSeg.GetComponent<RoadSegment>().CreatePoly(0, spline, widthSegment, Color.red, Color.white);
        return road;

    }

    public ref Region GetRegion(RegionId regionId)
    {
        return ref subRegions[regionId.x, regionId.y];
    }

    public List<RegionId> GetAdjacentRegions(RegionId regionId)
    {
        List<RegionId> adjacentRegions = new List<RegionId>();
        int regionIdX = regionId[0];
        int regionIdY = regionId[1];
        RegionId left = new RegionId(regionIdX - 1, regionIdY);
        RegionId right = new RegionId(regionIdX + 1, regionIdY);
        RegionId below = new RegionId(regionIdX, regionIdY - 1);
        RegionId above = new RegionId(regionIdX, regionIdY + 1);
        if (regionIdX > 0)
        {
            adjacentRegions.Add(left);
        }
        if (regionIdX < numBlocksHorizontal - 1)
        {
            adjacentRegions.Add(right);
        }
        if (regionIdY > 0)
        {
            adjacentRegions.Add(below);
        }
        if (regionIdY < numBlocksVectical - 1)
        {
            adjacentRegions.Add(above);
        }

        return adjacentRegions;
    }

    private List<RegionId> GetAdjacentRegionsWithFilter(RegionId regionId, Func<RegionId, bool> filterFunc)
    {
        List<RegionId> adjacentRegionsFiltered = new List<RegionId>();
        List<RegionId> adjacentRegions = GetAdjacentRegions(regionId);
        foreach (RegionId region in adjacentRegions)
        {
            if (filterFunc(region))
            {
                adjacentRegionsFiltered.Add(region);
            }
        }
        return adjacentRegionsFiltered;
    }


    private bool FILTER_IS_EMPTY(RegionId regionIndex)
    {
        return !GetRegion(regionIndex).hasRoad;
    }





    // ====
    // Test
    // ====

    void TestGenerate()
    {
        int i = 0;
        Vector2 segStart = startPoint;
        Vector2 segEnd;
        float angleDegPrev = angleStartDeg;
        while (i < numFold)
        {
            float angleStepDeg = UnityEngine.Random.Range(-maxAngleChangeDeg, maxAngleChangeDeg);
            float angleDeg = angleDegPrev + angleStepDeg;
            Vector2 segStep = lengthSegment * (Quaternion.Euler(0, 0, angleDeg) * Vector2.right);
            Color segColor = Color.Lerp(Color.red, Color.white, (i + 1.0f) / numFold);

            // GameObject roadSeg = Instantiate(prefabSeg, Vector2.zero, Quaternion.identity);
            GameObject roadSeg = Instantiate(prefabSeg, segStart, Quaternion.Euler(0, 0, angleDeg)); // warning: this one adjusts the object axis, hence affect rendering
            RoadSegment roadSegScript = roadSeg.GetComponent<RoadSegment>();
            roadSegScript.Initialize(i, segStart, segStep, widthSegment, segColor);
            
            segEnd = segStart + segStep;
            segStart = segEnd;
            angleDegPrev = angleDeg;
            i++;
        }
    }
}

public class Region
{
    public Region(RegionId id, Vector2 minCorner, Vector2 maxCorner)
    {
        this.regionId = id;
        this.minCorner = minCorner;
        this.maxCorner = maxCorner;
    }

    public RegionId regionId;
    public Vector2 minCorner;
    public Vector2 maxCorner;
    public bool hasRoad =  false;

    public Vector2 centerPoint
    {
        get {return (minCorner + maxCorner) / 2.0f;}
    }

}

