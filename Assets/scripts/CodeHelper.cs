using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CodeHelper
{

    public static Vector2 vec2(this MonoBehaviour self,float v1, float v2)
    {
        return new Vector2(v1, v2);
    }

    public static Vector3 vec3(this MonoBehaviour self, float v1, float v2, float v3)
    {
        return new Vector3(v1, v2, v3);
    }
}
