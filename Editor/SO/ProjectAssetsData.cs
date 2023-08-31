using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace PCP.Tools.WhichKey
{

    [CreateAssetMenu(fileName = "ProjectAssetsData", menuName = "Whichkey/ProjectAssetsData", order = 1)]
    public class ProjectAssetsData : ScriptableObject
    {
        [SerializeField] private string LayerHints;
        private void Awake()
        {
            LayerHints = "LayerHints";
            Debug.Log(LayerHints);
        }
    }
}
