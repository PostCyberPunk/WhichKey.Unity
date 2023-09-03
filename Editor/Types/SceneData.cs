using UnityEngine;
using System;
using UnityEditor;

namespace PCP.Tools.WhichKey
{
    [Serializable]
    public class SceneData
    {
        // [SerializeField]
        // [SerializeReference]
        public SceneAsset Scene;
        public KeyObject[] Targets;
        public SceneData(SceneAsset scene)
        {
            Scene = scene;
        }
    }
    [Serializable]
    public struct KeyObject
    {
        public string Target;
        public char Key;
    }
}
