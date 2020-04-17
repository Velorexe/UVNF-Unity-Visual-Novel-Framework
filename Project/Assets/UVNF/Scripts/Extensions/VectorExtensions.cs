using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 SetY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 SetX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector2 SetY(this Vector2 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector2 OffsetY(this Vector2 v, float offset)
    {
        v.y -= offset;
        return v;
    }
}

