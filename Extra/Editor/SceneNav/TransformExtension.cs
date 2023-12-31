using UnityEngine;

namespace PCP.WhichKey
{
    internal static class TransformExtension
    {
        public static string GetPath(this Transform t)
        {
            string path = "/" + t.name;
            while (t.parent != null)
            {
                t = t.parent.transform;
                path = "/" + t.name + path;
            }
            return path;
        }
    }
}
