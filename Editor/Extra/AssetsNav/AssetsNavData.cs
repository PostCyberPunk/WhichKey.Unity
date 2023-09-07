using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using Unity.Properties;
namespace PCP.Tools.WhichKey
{

    [CreateAssetMenu(fileName = "AssetsNavData", menuName = "Whichkey/AssetsNavData", order = 1)]
    public class AssetsNavData : ScriptableObject
    {
        [SerializeField] public string[] LayerHints;
        [SerializeField] private List<AssetNavSet> NavSetList = new();
        public string GetAssetsPathByKey(int key)
        {
            foreach (var item in NavSetList)
            {
                if (item.Key == key)
                    return item.AssetPath;
            }
            return "";
        }
        public void SetAssetsPathByKey(int key, string path)
        {
            for (int i = 0; i < NavSetList.Count; i++)
            {
                AssetNavSet item = NavSetList[i];
                if (item.Key == key)
                {
                    item.AssetPath = path;
                    item.Hint = path;
                    NavSetList[i] = item;
                    // UpdateLayerHints(i);
                    OnAssetsChange();
                    return;
                }
            }
            var asset = new AssetNavSet(key, path);
            NavSetList.Add(asset);
            OnAssetsChange();
            // UpdateLayerHints(AssetsDataList.Count - 1);
        }
        private void OnValidate()
        {
            OnAssetsChange();
        }
        private void UpdateLayerHints(int i)
        {
            LayerHints[i * 2] = NavSetList[i].Key.ToLabel();
            LayerHints[i * 2 + 1] = NavSetList[i].Hint;
        }
        private void OnAssetsChange()
        {
            LayerHints = new string[NavSetList.Count * 2];
            for (int i = 0; i < NavSetList.Count; i++)
            {
                LayerHints[i * 2] = NavSetList[i].Key.ToLabel();
                LayerHints[i * 2 + 1] = NavSetList[i].Hint;
            }
        }
    }
}
