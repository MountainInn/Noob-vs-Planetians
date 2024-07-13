using UnityEngine;

static public class Vector2Ext
{
    static public Vector3 xy_(this Vector2 v2, float z = 0) => new Vector3(v2.x, v2.y, z);
}
