using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CodeHelper
{
    public static class vec
    {

        public static Vector2 vec2(this MonoBehaviour self, float v1, float v2)
        {
            return new Vector2(v1, v2);
        }

        public static Vector3 vec3(this MonoBehaviour self, float v1, float v2, float v3)
        {
            return new Vector3(v1, v2, v3);
        }

        public static Vector3 lerp(Vector3 from,Vector3 to , float t)
        {
            return new Vector3(Mathf.Lerp(from.x, to.x, t), Mathf.Lerp(from.y, to.y, t), Mathf.Lerp(from.z, to.z, t));
        }
    }
}
