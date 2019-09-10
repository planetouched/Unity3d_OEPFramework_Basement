using UnityEngine;

namespace Basement.OEPFramework.UnityEngine.Util
{
    public static class VectorUtil
    {
        public static Vector3 ToV3(Vector3 origin, float newZ)
        {
            return new Vector3(origin.x, origin.y, newZ);
        }

        public static Vector3 ToV3(Vector2 origin, float newZ)
        {
            return new Vector3(origin.x, origin.y, newZ);
        }

        public static Vector2 ToV2(Vector3 origin)
        {
            return new Vector2(origin.x, origin.y);
        }
    }
}
