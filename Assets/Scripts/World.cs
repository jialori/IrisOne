using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World : MonoBehaviour
{

    [Header("References")]
    public RoadGenerator roadGenerator;
    public ClanGenerator clanGenerator;
    public GameObject prefabLine; // world boundary
    public Player player;

    // Configurations
    [Header("World Generation Params")]
    public float worldWidth = 100;
    public float worldLength = 100;
    public Vector2 positionPlayerStart;
    public int numClans;

    [Header("Visualization")]
    public bool drawWorldBoundaries = false;
    public bool drawWorldRegionLines = false;



    void Awake()
    {

    }

    void Start()
    {
        // generate world boundary (prefabs)
        if (drawWorldBoundaries) DrawWorldBoundaries();
        if (drawWorldRegionLines) DrawWorldRegionLines();

        roadGenerator.Initialize(this);
        // clanGenerator.Initialize(this);
        Road road = roadGenerator.GenerateRoad(this, positionPlayerStart);
        if (road != null)
        {
            // List<Clan> clans = clanGenerator.GenerateClans(this, road, numClans);


            // generate player in the center
            player.Initialize(positionPlayerStart, road);

            // focus camera on player (implement at the camera)

            // generate monster (call monster data generator)

        }



        // // Tests
        // BezierCurve.TestGenerate(prefabLine);
    }

    void Update()
    {
        
    }


    void DrawWorldBoundaries()
    {
        Vector2 lowerLeftCorner = new Vector2(0, 0);
        Vector2 lowerRightCorner = new Vector2(worldWidth, 0);
        Vector2 upperLeftCorner = new Vector2(0, worldLength);
        Vector2 upperRightCorner = new Vector2(worldWidth, worldLength);

        float lineWidth = 1;
        Color lineColor = Color.white;

        GameObject roadSeg = Instantiate(prefabLine);
        Road roadSegScript = roadSeg.GetComponent<Road>();
        roadSegScript.CreateSimple(0, lowerLeftCorner, lowerRightCorner, lineWidth, lineColor);

        roadSeg = Instantiate(prefabLine);
        roadSegScript = roadSeg.GetComponent<Road>();
        roadSegScript.CreateSimple(0, upperLeftCorner, upperRightCorner, lineWidth, lineColor);

        roadSeg = Instantiate(prefabLine);
        roadSegScript = roadSeg.GetComponent<Road>();
        roadSegScript.CreateSimple(0, lowerLeftCorner, upperLeftCorner, lineWidth, lineColor);

        roadSeg = Instantiate(prefabLine);
        roadSegScript = roadSeg.GetComponent<Road>();
        roadSegScript.CreateSimple(0, lowerRightCorner, upperRightCorner, lineWidth, lineColor);

    }

    void DrawWorldRegionLines()
    {
        Vector2 lowerLeftCorner = new Vector2(0, 0);
        Vector2 lowerRightCorner = new Vector2(worldWidth, 0);
        Vector2 upperLeftCorner = new Vector2(0, worldLength);
        Vector2 upperRightCorner = new Vector2(worldWidth, worldLength);

        float lineWidth = 0.5f;
        Color lineColor = Color.white;

        float width = worldWidth / roadGenerator.numBlocksHorizontal;
        float length = worldLength / roadGenerator.numBlocksVectical;

        for (int i = 1; i < roadGenerator.numBlocksHorizontal; i++)
        {
            for (int j = 1; j < roadGenerator.numBlocksVectical; j++)
            {
                Vector2 lowerPoint = new Vector2(j * width, 0);
                Vector2 upperPoint = new Vector2(j * width, worldLength);
                GameObject roadSegVertical = Instantiate(prefabLine);
                Road roadSegScriptVertical = roadSegVertical.GetComponent<Road>();
                roadSegScriptVertical.CreateSimple(0, lowerPoint, upperPoint, lineWidth, lineColor);
            }
            Vector2 leftPoint = new Vector2(0, i * length);
            Vector2 rightPoint = new Vector2(worldWidth, i * length);
            GameObject roadSegHorizontal = Instantiate(prefabLine);
            Road roadSegScriptHorizontal = roadSegHorizontal.GetComponent<Road>();
            roadSegScriptHorizontal.CreateSimple(0, leftPoint, rightPoint, lineWidth, lineColor);
        }
    }


}


