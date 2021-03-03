using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Select(), Infinity
using System.Linq; // Select()

public class RoadSegment : MonoBehaviour
{
    public GameObject prototypeRoadSeg;

    public int Id;
    
    List<Vector2> m_points;
    private LineRenderer m_renderer;

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
    }

    void Start()
    {
    }


    
    public  RoadSegment CreateSimple(int id, Vector2 startPoint, Vector2 endPoint)
    {
        GameObject roadSeg = Instantiate(prototypeRoadSeg);
        RoadSegment roadSegScript = roadSeg.GetComponent<RoadSegment>();
        roadSegScript.Initialize(id, startPoint, endPoint);
        return roadSegScript;
    }

    public  RoadSegment CreateSimple(int id, Vector2 startPoint, Vector2 endPoint, float width, Color color)
    {
        GameObject roadSeg = Instantiate(prototypeRoadSeg);
        RoadSegment roadSegScript = roadSeg.GetComponent<RoadSegment>();
        roadSegScript.Initialize(id, startPoint, endPoint);
        return roadSegScript;
    }
    
    // public  RoadSegment CreateSimple(int id, int orderInLayer, Vector2 startPoint, Vector2 endPoint)
    // {
    //     GameObject roadSeg = Instantiate(prototypeRoadSeg);
    //     RoadSegment roadSegScript = roadSeg.GetComponent<RoadSegment>();
    //     roadSegScript.Initialize(id, orderInLayer, startPoint, endPoint);
    //     return roadSegScript;
    // }
    // public  RoadSegment CreateSimple(int id, int orderInLayer, Vector2 startPoint, Vector2 endPoint, float width, Color color)
    // {
    //     GameObject roadSeg = Instantiate(prototypeRoadSeg);
    //     RoadSegment roadSegScript = roadSeg.GetComponent<RoadSegment>();
    //     roadSegScript.Initialize(id, orderInLayer, startPoint, endPoint);
    //     return roadSegScript;
    // }

    public  RoadSegment CreatePoly(int id, List<Vector2> spline, float width, Color startColor, Color endColor)
    {
        GameObject roadSeg = Instantiate(prototypeRoadSeg);

        LineRenderer lineRenderer = roadSeg.GetComponent<LineRenderer>();
        lineRenderer.positionCount = spline.Count;
        Vector3[] verts3D = spline.Select(x => (Vector3)x).ToArray();
        lineRenderer.SetPositions(verts3D);
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        RoadSegment roadSegScript = roadSeg.GetComponent<RoadSegment>();
        roadSegScript.Initialize(id, spline);
        return roadSegScript;
    }

    public void Initialize(int id, List<Vector2> spline)
    {
        this.SetId(id);
        this.m_points = spline;
    }

    public void Initialize(int id, Vector2 startPoint, Vector2 endPoint)
    {
        this.SetId(id);

        this.StartPoint = startPoint;
        this.EndPoint = endPoint;
        // UpdateTransform();
    }

    public void Initialize(int id, int orderInLayer, Vector2 startPoint, Vector2 endPoint)
    {
        this.SetId(id);
        this.m_renderer.sortingOrder = orderInLayer;

        this.StartPoint = startPoint;
        this.EndPoint = endPoint;
        // UpdateTransform();
    }

    public void Initialize(int id, Vector2 startPoint, Vector2 endPoint, float width, Color color)
    {
        Initialize(id, startPoint, endPoint);
        Color = color;
        Width = width;
    }

    public void Initialize(int id, int orderInLayer, Vector2 startPoint, Vector2 endPoint, float width, Color color)
    {
        Initialize(id, orderInLayer, startPoint, endPoint);
        Color = color;
        Width = width;
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

    public void SetId(int id)
    {
        Id = id;
    }

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


    private void Tests()
    {
        this.Initialize(0, new Vector2(1, 1), new Vector2(5, 5));
    }
}