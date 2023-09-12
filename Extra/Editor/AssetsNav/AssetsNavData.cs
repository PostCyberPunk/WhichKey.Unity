using System.Collections.Generic;
using PCP.WhichKey.Types;
using UnityEngine;
namespace PCP.WhichKey.Extra
{

    [CreateAssetMenu(fileName = "AssetsNavData", menuName = "Whichkey/AssetsNavData", order = 1)]
    public class AssetsNavData : ScriptableObject
    {
        public LayerHint[] LayerHints;
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
        private void OnAssetsChange()
        {
            LayerHints = new LayerHint[NavSetList.Count];
            for (int i = 0; i < NavSetList.Count; i++)
            {
                LayerHints[i] = new LayerHint(NavSetList[i].Key, NavSetList[i].Hint);
            }
        }
    }
}
