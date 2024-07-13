using UnityEngine;

static public class Vector3Extensions
{
    static public Vector3 AddX(this Vector3 v, float x)
    {
        return new Vector3(v.x + x, v.y, v.z);
    }
    static public Vector3 AddY(this Vector3 v, float y)
    {
        return new Vector3(v.x, v.y + y, v.z);
    }
    static public Vector3 AddZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, v.z + z);
    }

    static public Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }
    static public Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }
    static public Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    static public Vector2 xy(this Vector3 v3) => new Vector2(v3.x, v3.y);
}
