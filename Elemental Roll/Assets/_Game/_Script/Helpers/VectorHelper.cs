using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorHelper
{
    //We apply a rotation on the Y axis
    public static Vector3 RotateY(Vector3 _v, float angle)
    {
        Vector3 v = _v;
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = v.x;
        float tz = v.z;
        v.x = (cos * tx) + (sin * tz);
        v.z = (cos * tz) - (sin * tx);
        return v;
    }


}
