using System;
using System.Collections.Generic;
using UnityEditor;

namespace PCP.Tools.WhichKey
{
    [Serializable]
    public class SceneNavData
    {
        // [SerializeField]
        // [SerializeReference]
        public SceneAsset Scene;
        public List<SceneNavTarget> Targets = new();
        public string[] KeyHints = new string[0];
        public SceneNavData(SceneAsset scene)
        {
            Scene = scene;
            Targets = new();
            KeyHints = new string[0];
        }
        public void SetupKeyHints()
        {
            if (Targets == null || Targets.Count == 0)
                return;
            KeyHints = new string[Targets.Count * 2];
            for (int i = 0; i < Targets.Count; i++)
            {
                KeyHints[i * 2] = Targets[i].Key.KeyLabel;
                KeyHints[i * 2 + 1] = Targets[i].Target;
            }
        }
    }
    [Serializable]
    public struct SceneNavTarget
    {
        public WkKeySeq Key;
        private string _target;
        public SceneNavTarget(int key, string target)
        {
            Key = new WkKeySeq(key);
            _target = target;
        }

        public string Target { get => _target; set => _target = value; }
    }
}
