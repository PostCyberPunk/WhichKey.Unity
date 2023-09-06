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
        [SerializeField] private List<AssetData> AssetsDataList = new();
        public string GetAssetsPathByKey(int key)
        {
            foreach (var item in AssetsDataList)
            {
                if (item.Key == key)
                    return item.AssetPath;
            }
            return "";
        }
        public void SetAssetsPathByKey(int key, string path)
        {
            for (int i = 0; i < AssetsDataList.Count; i++)
            {
                AssetData item = AssetsDataList[i];
                if (item.Key == key)
                {
                    item.AssetPath = path;
                    UpdateLayerHints(i);
                    return;
                }
            }
            var asset = new AssetData(key, path);
            AssetsDataList.Add(asset);
            UpdateLayerHints(AssetsDataList.Count - 1);
        }
        private void OnValidate()
        {
            OnAssetsChange();
        }
        private void UpdateLayerHints(int i)
        {
            LayerHints[i * 2] = AssetsDataList[i].Key.ToLabel();
            LayerHints[i * 2 + 1] = AssetsDataList[i].Hint;
        }
        private void OnAssetsChange()
        {
            LayerHints = new string[AssetsDataList.Count * 2];
            for (int i = 0; i < AssetsDataList.Count; i++)
            {
                LayerHints[i * 2] = AssetsDataList[i].Key.ToLabel();
                LayerHints[i * 2 + 1] = AssetsDataList[i].Hint;
            }
        }
    }
}
