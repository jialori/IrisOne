using UnityEngine;
using System; // Infinity

namespace Util
{

static class Vector2Ext
{

    static public bool IsCloseToZero(Vector2 a)
    {
        return a.sqrMagnitude < ApproximatelyZeroSqrMagnitude;
    }

    static public float ApproximatelyZeroSqrMagnitude
    {
        get {return 1.5E-10f;} // magic number
    }

    static public void Project(Vector2 a, Vector2 b, out Vector2 projAdjacent, out Vector2 projOpposite)
    {
        projAdjacent = Vector2Ext.VectorProjectionAdjacent(a, b);
        projOpposite = a - projAdjacent;
    }

    static public float ScalarProjectionAdjacent(Vector2 a, Vector2 b)
    {
        return Vector2.Dot(a, b.normalized);
    }

    static public Vector2 VectorProjectionAdjacent(Vector2 a, Vector2 b)
    {
        return Vector2Ext.ScalarProjectionAdjacent(a, b) * b.normalized;
    }


    // static public Vector2 VectorProjectionOpposite(Vector2 a, Vector2 b)
    // {
    //     return (a - Vector2Ext.VectorProjectionAdjacent(a, b));
    // }


    // static public float ScalarProjectionOpposite(Vector2 a, Vector2 b)
    // {
    //     return Vector2Ext.VectorProjectionOpposite(a, b).magnitude;
    // }


    static public Vector2 NegativeInfinity
    {
        get {return new Vector2(Single.NegativeInfinity, Single.NegativeInfinity);}
    }
    static Vector2 LineLineIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        Vector2 outIntersectPoint;
        // mathematics reference https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
        // answer by Jason Cohen and Lance Roberts

        // Vector2 a, b, c, d;
        // a = l1.p1;
        // b = l1.p2;
        // c = l2.p1;
        // d = l2.p2;
        
        Vector2 E = b - a;
        Vector2 F = d - c;
        Vector2 P = new Vector2(-E.y, E.x);
        if (Vector2.Dot(F, P) != 0)
        {
            float h = Vector2.Dot(a - c, P) / Vector2.Dot(F, P);

            Vector2 Q = new Vector2(-F.y, F.x);
            float g =  Vector2.Dot(c - a, Q) / Vector2.Dot(E, Q);

            if ((h >= 0 && h <= 1) && (g >= 0 && g <= 1))
            {
                outIntersectPoint = new Vector2(c.x + F.x * h, c.y + F.y * h);
            }
            else
            {
                outIntersectPoint = NegativeInfinity;
            }

        }
        else
        {   // parallel
            outIntersectPoint = NegativeInfinity;
        }
        
        return outIntersectPoint;
    }

}

}
