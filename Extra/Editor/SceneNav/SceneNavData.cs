using System;
using System.Collections.Generic;
using UnityEditor;
using PCP.WhichKey.Types;
using UnityEngine;

namespace PCP.WhichKey.Extra
{
    [Serializable]
    public class SceneNavData
    {
        public SceneAsset Scene;
        public List<SceneNavTarget> Targets;
        public LayerHint[] KeyHints;
        public SceneNavData(SceneAsset scene)
        {
            Scene = scene;
            Targets = new();
            KeyHints = new LayerHint[0];
        }
        public void SetupKeyHints()
        {
            if (Targets == null || Targets.Count == 0)
                return;
            KeyHints = new LayerHint[Targets.Count];
            for (int i = 0; i < Targets.Count; i++)
            {
                KeyHints[i] = new LayerHint(Targets[i].Key, Targets[i].Hint);
            }
        }
    }
    [Serializable]
    public struct SceneNavTarget
    {
        public WkKeySeq Key;
        [SerializeField]
        private string _hint;
        [SerializeField]
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
