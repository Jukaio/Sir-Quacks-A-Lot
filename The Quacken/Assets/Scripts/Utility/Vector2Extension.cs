using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extension
{

    public static Vector2 Rotate(this Vector2 v, float degrees)
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
}

public static class Vector3Extension
{

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
}