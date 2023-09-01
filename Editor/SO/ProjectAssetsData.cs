using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using Unity.Properties;
namespace PCP.Tools.WhichKey
{

    [CreateAssetMenu(fileName = "ProjectAssetsData", menuName = "Whichkey/ProjectAssetsData", order = 1)]
    public class ProjectAssetsData : ScriptableObject
    {
        [SerializeField] public string[] LayerHints;
        [SerializeField] private AssetData[] AssetsData = new AssetData[0];
        public string GetAssetsPathByKey(char key)
        {
            foreach (var item in AssetsData)
            {
                if (item.Key == key)
                    return item.AssetPath;
            }
            return "";
        }
        private void OnValidate()
        {
            OnAssetsChange();
        }
        private void OnAssetsChange()
        {
            LayerHints = new string[AssetsData.Length * 2];
            for (int i = 0; i < AssetsData.Length; i++)
            {
                LayerHints[i * 2] = AssetsData[i].Key.ToString();
                LayerHints[i * 2 + 1] = AssetsData[i].Hint;
            }
        }
    }
}
