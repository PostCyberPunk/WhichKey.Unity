using UnityEngine;
using System;

namespace PCP.Tools.WhichKey
{
    [Serializable]
    public struct SceneData
    {
        [SerializeField]
        public string Path;
        [SerializeReference]
        public GameObject[] Targets;

        public SceneData(string path, GameObject[] targets)
        {
            Path = path;
            Targets = targets;
        }
    }
}
