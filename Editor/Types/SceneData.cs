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
        public KeyObject[] Targets = new KeyObject[0];
        public string[] KeyHints = new string[0];
        public SceneData(SceneAsset scene)
        {
            Scene = scene;
            Targets = new KeyObject[0];
            KeyHints = new string[0];
        }
        public void SetupKeyHints()
        {
            if (Targets == null || Targets.Length == 0)
                return;
            KeyHints = new string[Targets.Length * 2];
            for (int i = 0; i < Targets.Length; i++)
            {
                KeyHints[i * 2] = Targets[i].Key.ToString();
                KeyHints[i * 2 + 1] = Targets[i].Target;
            }
        }
    }
    [Serializable]
    public struct KeyObject
    {
        public char Key;
        public string Target;
    }
}
