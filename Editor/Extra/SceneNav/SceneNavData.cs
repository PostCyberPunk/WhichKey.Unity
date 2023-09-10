using System;
using System.Collections.Generic;
using UnityEditor;
using PCP.WhichKey.Types;

namespace PCP.WhichKey.Extra
{
    [Serializable]
    public class SceneNavData
    {
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
                KeyHints[i * 2 + 1] = Targets[i].Hint;
            }
        }
    }
    [Serializable]
    public struct SceneNavTarget
    {
        public WkKeySeq Key;
        private string _hint;
        private string _target;
        public string Target { get => _target; set => _target = value; }
        public string Hint { get => _hint; set => _hint = value; }
        public SceneNavTarget(int key, string hint, string target)
        {
            Key = new WkKeySeq(key);
            _hint = hint;
            _target = target;
        }

        //PERF: This is a hot path, so we need to avoid allocations here.
        public void ChangeTarget(string hint, string path)
        {
            this.Hint = hint;
            this.Target = path;
        }
    }
}
