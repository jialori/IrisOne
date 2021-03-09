using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Select(), Infinity
using System.Linq; // Select()

// A pair of extended endpoints corresponding to a pair of road points,
// Navigating with this coordinate will make sure the mover stays on the line.
// Used for navigating through the road.
public struct NavBlock
{
    public Vector2 startPoint;
    public Vector2 endPoint;

    public NavBlock(Vector2 start, Vector2 end)
    {
        startPoint = start;
        endPoint = end;
    }
}

public class Road : MonoBehaviour
{
    public GameObject prototypeRoadSeg;

    public int Id;
    
    List<Vector2> m_points;
    List<NavBlock> m_navBlocks; // used for road traversal, calculated on the fly
    private LineRenderer m_renderer;

    public List<Vector2> Points
    {
        get {return m_points;}
    }

    public int PointsCount
    {
        get {return m_points.Count;}
    }

    public Vector2 StartPoint
    {
        get {return m_points[0];}
        set {
            m_points[0] = value;
            m_renderer.SetPosition(0, value);
        }
    }
    public Vector2 EndPoint
    {
        get {return m_points[m_points.Count-1];}
        set {
            m_points[m_points.Count-1] = value;
            m_renderer.SetPosition(m_points.Count-1, value);
        }
    }

    public float StartWidth
    {
        get {return m_renderer.startWidth;}
        set {m_renderer.startWidth = value;}
    }

    public float EndWidth
    {
        get {return m_renderer.endWidth;}
        set {m_renderer.endWidth = value;}
    }

    public float Width
    {
        get {
            return ((StartWidth != EndWidth) ? Single.NegativeInfinity : StartWidth);
        }
        set {
            StartWidth = value;
            EndWidth = value;
        }
    }

    public Color StartColor
    {
        get {return m_renderer.startColor;}
        set {m_renderer.startColor = value;}
    }

    public Color EndColor
    {
        get {return m_renderer.endColor;}
        set {m_renderer.endColor = value;}
    }

    public Color Color
    {
        // get {
        //     return ((StartColor != EndColor) ? Color.clear : StartColor);
        // }
        set {
            StartColor = value;
            EndColor = value;
        }
    }

    public float SignedAngle
    {
        get {return Vector2.SignedAngle(Vector2.right, Step);}
    }
    public float Length
    {
        get {return Step.magnitude;}
    }
    public Vector2 Step
    {
        get {return EndPoint - StartPoint;}
    }

    void Awake()
    {
        m_renderer = GetComponent<LineRenderer>();
        m_points = new List<Vector2>();
        m_points.Add(Vector2.negativeInfinity);
        m_points.Add(Vector2.negativeInfinity);
        m_navBlocks = new List<NavBlock>();
    }

    void Start()
    {
    }

    private void Tests()
    {
        this.InitializeOnCreate(0, new Vector2(1, 1), new Vector2(5, 5));
    }
    
    public Road CreateSimple(int id, Vector2 startPoint, Vector2 endPoint, int orderInLayer = int.MinValue)
    {
        GameObject roadGameObject = Instantiate(prototypeRoadSeg);
        Road roadScript = roadGameObject.GetComponent<Road>();
        roadScript.InitializeOnCreate(id, startPoint, endPoint, orderInLayer);
        return roadScript;
    }

    public Road CreateSimple(int id, Vector2 startPoint, Vector2 endPoint, float width, Color color, int orderInLayer = int.MinValue)
    {
        GameObject roadGameObject = Instantiate(prototypeRoadSeg);
        Road roadScript = roadGameObject.GetComponent<Road>();
        roadScript.InitializeOnCreate(id, startPoint, endPoint, orderInLayer);
        return roadScript;
    }
    
    public Road CreatePoly(int id, List<Vector2> spline, float width, Color startColor, Color endColor)
    {
        GameObject roadGameObject = Instantiate(prototypeRoadSeg);

        LineRenderer lineRenderer = roadGameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = spline.Count;
        Vector3[] verts3D = spline.Select(x => (Vector3)x).ToArray();
        lineRenderer.SetPositions(verts3D);
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        Road roadScript = roadGameObject.GetComponent<Road>();
        roadScript.InitializeOnCreate(id, spline);
        return roadScript;
    }

    private void InitializeOnCreate(int id, List<Vector2> spline)
    {
        this.SetId(id);
        this.m_points = spline;
    }

    private void InitializeOnCreate(int id, Vector2 startPoint, Vector2 endPoint, int orderInLayer = int.MinValue)
    {
        this.SetId(id);

        this.StartPoint = startPoint;
        this.EndPoint = endPoint;
        // UpdateTransform();

        if (orderInLayer != int.MinValue)
        {
            this.m_renderer.sortingOrder = orderInLayer;
        }
    }

    private void InitializeOnCreate(int id, Vector2 startPoint, Vector2 endPoint, float width, Color color, int orderInLayer = int.MinValue)
    {
        InitializeOnCreate(id, startPoint, endPoint, orderInLayer);
        Color = color;
        Width = width;

        if (orderInLayer != int.MinValue)
        {
            this.m_renderer.sortingOrder = orderInLayer;
        }
    }

    public void SetId(int id)
    {
        Id = id;
    }

    public Vector2 GetPoint(int index)
    {
        if (index < 0 || index > m_points.Count -1) return Vector2.negativeInfinity;
        return m_points[index];
    }

    public NavBlock GetNavBlock(int index)
    {

        if (index < 0 || index > this.m_points.Count - 2)
        {
            // invalid search
            return new NavBlock(Vector2.negativeInfinity, Vector2.negativeInfinity); 
        }
        
        if (m_navBlocks.Count == 0 && m_points.Count == 2)
        {
            m_navBlocks.Add(new NavBlock(this.StartPoint, this.EndPoint));
        }
        else if (m_navBlocks.Count == 0 && m_points.Count > 2)
        {
            NavBlock curBlock = new NavBlock(m_points[0], Vector2.negativeInfinity);
            float lenHalfWidth = this.Width / 2f; // RISK varying width
            for (int i = 1; i <= m_points.Count-2; i++)
            {
                Vector2 OA = m_points[i-1] - m_points[i]; 
                Vector2 OB = m_points[i+1] - m_points[i];
                float angleAOB = Vector2.Angle(OA, OB);

                if (angleAOB < Single.Epsilon * 10E8f)
                {
                    curBlock.endPoint = m_points[i];
                    m_navBlocks.Add(curBlock);
                    curBlock.startPoint = m_points[i];
                    continue;
                }

                float lenExtended;
                if (angleAOB == 90)
                {
                    lenExtended = lenHalfWidth;
                }
                else
                {
                    float angleHalfResidual = (angleAOB > 90) ? 
                                                (angleAOB - 2f * (angleAOB - 90f)) / 2f :
                                                (angleAOB + 2f * (90f - angleAOB)) / 2f;// the nav algorithm is different too..
                    lenExtended = Mathf.Tan(Mathf.Deg2Rad * angleHalfResidual) * lenHalfWidth;
                }
                curBlock.endPoint = m_points[i] + lenExtended * (-OA).normalized;
                m_navBlocks.Add(curBlock);
                curBlock.startPoint = m_points[i] + lenExtended * (-OB).normalized;
            }
            curBlock.endPoint = m_points[m_points.Count - 1];
            m_navBlocks.Add(curBlock);
        }

        return m_navBlocks[index];
    }


}


    // public void UpdateTransform()
    // {
    //     // warning: this one adjusts the object axis, hence affect rendering
    //     this.transform.position = (Vector3)StartPoint;
    //     this.transform.rotation = Quaternion.Euler(0, 0, SignedAngle);
    //     this.transform.localScale = new Vector3(Length, 0, 0);

    //     // m_renderer.SetPosition(0, new Vector3(width.x, width.y, 0));
    //     // m_renderer.SetPosition(0, new Vector3(width.x, width.y, 0));
    // }
    // public void SetStart(Vector2 start)
    // {
    //     StartPoint = start;
    //     // UpdateTransform();
    // }

    // public void SetEnd(Vector2 end)
    // {
    //     EndPoint = end;
    //     // UpdateTransform();
    // }
