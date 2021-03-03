using UnityEngine;
using System.Collections.Generic;

// Copyright:
// Code is re-rewritten from the snippets in
// https://jeremykun.com/2013/05/11/bezier-curves-and-picasso/
public class BezierSpline
{


    public static List<Vector2> CubicSplineC0(List<Vector2> controlPoints)
    {
        List<Vector2> spline = new List<Vector2>();
        spline.Add(controlPoints[0]);
        for (int i = 0; i < controlPoints.Count - 3; i = i + 3)
        {
            Vector2 a = controlPoints[i];
            Vector2 b = controlPoints[i+1];
            Vector2 c = controlPoints[i+2];
            Vector2 d = controlPoints[i+3];
            
            Vector2[] lst = new Vector2[4] {a, b, c, d};
            Vector2[] curve = BezierSpline.CubicCurve(lst);

            for (int j = 0; j < curve.Length; j++)
            {
                spline.Add(curve[j]);
            }
        }
        return spline;
    } 

    public static List<Vector2> CubicSplineC1(List<Vector2> controlPoints)
    {
        List<Vector2> spline = new List<Vector2>();

        // int debugk = 1;
        for (int i = 0; i < controlPoints.Count-3; i = i + 3)
        {
            if (i + 4 < controlPoints.Count)
            {
                controlPoints[i+3] = 0.5f * (controlPoints[i+3] + controlPoints[i+4]);
            }

            Vector2 a = controlPoints[i];
            Vector2 b = controlPoints[i+1];
            Vector2 c = controlPoints[i+2];
            Vector2 d = controlPoints[i+3];
            
            Vector2[] lst = new Vector2[4] {a, b, c, d};
            Vector2[] curve = BezierSpline.CubicCurve(lst);
            // debugk += 3;
            for (int j = 0; j < curve.Length; j++)
            {
                spline.Add(curve[j]);
            }
        }
        // Debug.Log(debugk);

        return spline;

    } 

    // https://www.rose-hulman.edu/~finn/CCLI/Notes/day18.pdf
    public static List<Vector2> CubicSplineC2(List<Vector2> controlPoints)
    {
        List<Vector2> spline = new List<Vector2>();

        int L = controlPoints.Count - 2;
        int Len_C2 = L * 3;
        Vector2[] controlPointsC2 = new Vector2[Len_C2+1];
        List<Vector2> D = controlPoints;

        controlPointsC2[0] = D[0];
        controlPointsC2[1] = D[1];
        controlPointsC2[Len_C2-2] = D[D.Count-2];
        controlPointsC2[Len_C2-1] = D[D.Count-1];
        controlPointsC2[2] = D[1]/2f + D[2]/2f;
        controlPointsC2[Len_C2-3] = D[D.Count-3]/2f + D[D.Count-2]/2f;
        // note D: positive index - needs to add 2
        controlPointsC2[Len_C2] = D[D.Count-1]; // make up
        
        // Debug.Log(controlPointsC2[Len_C2-2]);
        // Debug.Log(controlPointsC2[Len_C2-1]);

        for (int i = 1; i <= L - 2; i++)
        {
            controlPointsC2[3*i+1] = 2f/3f * D[i - 1 + 2] + 1f/3f * D[i + 2];
            controlPointsC2[3*i+2] = 1f/3f * D[i - 1 + 2] + 2f/3f * D[i + 2];
        }

        for (int i = 1; i <= L - 1; i++)
        {
            controlPointsC2[3*i] = 1f/2f * controlPointsC2[3*i-1] + 1f/2f * controlPointsC2[3*i+1];
        }

        for (int i = 0; i <= Len_C2-3; i = i + 3)
        {
            Vector2 a = controlPointsC2[i];
            Vector2 b = controlPointsC2[i+1];
            Vector2 c = controlPointsC2[i+2];
            Vector2 d = controlPointsC2[i+3];
            Color segColor = Color.Lerp(Color.red, Color.white, (i + 1.0f) / (Len_C2 - 1));
            
            Vector2[] lst = new Vector2[4] {a, b, c, d};
            Vector2[] curve = BezierSpline.CubicCurve(lst);
        // Debug.Log("===");
        //     Debug.Log(a);
        //     Debug.Log(b);
        //     Debug.Log(c);
        //     Debug.Log(d);
        //     Debug.Log(i);
        // Debug.Log("===");
            for (int j = 0; j < curve.Length; j++)
            {
                spline.Add(curve[j]);
            }
        }
        // Debug.Log(Len_C2+1);

        return spline;
    } 

    /**
     * Helper Functions
     */

     public static Vector2[] CubicCurve(Vector2[] curve) {
        var ret = curve;
        while (!isFlatCubic(ret)) {
            var pieces = SubdivideCubic(curve);
            var x = CubicCurve(pieces[0]);
            var y = CubicCurve(pieces[1]);
            ret = new Vector2[x.Length + y.Length];
            x.CopyTo(ret, 0);
            y.CopyTo(ret, x.Length);
        }
        return ret;
    }

    public static Vector2[] Midpoints(Vector2[] pointList) {
        var midpointList = new Vector2[pointList.Length - 1];
        for (int i = 0; i < midpointList.Length; i++) {
            midpointList[i] = Midpoint(pointList[i], pointList[i+1]);
        }
        return midpointList;

        Vector2 Midpoint (Vector2 p, Vector2 q) {
            return new Vector2((p[0] + q[0]) / 2.0f, (p[1] + q[1]) / 2.0f);
        };
    }

    public static Vector2[][] SubdivideCubic(Vector2[] curve) {
        var firstMidpoints = Midpoints(curve);
        var secondMidpoints = Midpoints(firstMidpoints);
        var thirdMidpoints = Midpoints(secondMidpoints);
        
        return new Vector2[2][] {
                new Vector2[] {curve[0], firstMidpoints[0], secondMidpoints[0], thirdMidpoints[0]},
                new Vector2[] {thirdMidpoints[0], secondMidpoints[1], firstMidpoints[2], curve[3]}
                };
    }
    public static bool isFlatCubic(Vector2[] curve) {
        float tol = 10; // anything below 50 is roughly good-looking
        
        float ax = 3.0f*curve[1][0] - 2.0f*curve[0][0] - curve[3][0]; ax *= ax;
        float ay = 3.0f*curve[1][1] - 2.0f*curve[0][1] - curve[3][1]; ay *= ay;
        float bx = 3.0f*curve[2][0] - curve[0][0] - 2.0f*curve[3][0]; bx *= bx;
        float by = 3.0f*curve[2][1] - curve[0][1] - 2.0f*curve[3][1]; by *= by;
        
        return (Mathf.Max(ax, bx) + Mathf.Max(ay, by) <= tol);
    }

    public static void TestGenerate(GameObject prefabLine)
    {
        // Test Bezier
        Vector2 a = new Vector2(1, 1);
        Vector2 b = new Vector2(2, 1);
        Vector2 c = new Vector2(10,10);
        Vector2 d = new Vector2(20, 4);
        Vector2[] lst = new Vector2[4] {a, b, c, d};
        Vector2[] curve = BezierSpline.CubicCurve(lst);
        for (int i = 0; i < curve.Length - 1; i++)
        {
            prefabLine.GetComponent<RoadSegment>().CreateSimple(0, curve[i], curve[i+1], 0.5f, Color.white);
        }
    }

}