using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCP.Tools.WhichKey
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
