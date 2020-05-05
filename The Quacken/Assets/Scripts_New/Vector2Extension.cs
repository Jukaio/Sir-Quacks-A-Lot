using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Vector2Extension
{

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        degrees *= Mathf.Deg2Rad;
        double sin = Math.Sin(degrees);
        double cos = Math.Cos(degrees);

        double tx = v.x;
        double ty = v.y;
        v.x = (float)((cos * tx) - (sin * ty));
        v.y = (float)((sin * tx) + (cos * ty));
        return v;
    }

    public static float Distancef(this Vector3 v, Vector3 rhs)
    {
        double d = Math.Sqrt(Math.Pow(v.x - rhs.x, 2) + Math.Pow(v.y - rhs.y, 2));
        return (float) d;
    }

    public static float Distancef(this Vector2 v, Vector3 rhs)
    {
        double d = Math.Sqrt(Math.Pow(v.x - rhs.x, 2) + Math.Pow(v.y - rhs.y, 2));
        return (float)d;
    }

    public static Vector3 Rotate(this Vector3 v, float degrees)
    {
        degrees *= Mathf.Deg2Rad;
        float sin = Mathf.Sin(degrees);
        float cos = Mathf.Cos(degrees);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static Vector2[] To_Vector2_Array(this Vector3[] v3)
    {
        return System.Array.ConvertAll<Vector3, Vector2>(v3, getV3fromV2);
    }

    public static Vector2 getV3fromV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }
}

