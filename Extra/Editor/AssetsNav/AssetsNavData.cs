using System.Collections.Generic;
using System.Linq;
using PCP.WhichKey.Types;
using UnityEditor;
using UnityEngine;
namespace PCP.WhichKey.Extra
{

    [CreateAssetMenu(fileName = "AssetsNavData", menuName = "Whichkey/AssetsNavData", order = 1)]
    public class AssetsNavData : ScriptableObject
    {
        public LayerHint[] LayerHints = new LayerHint[0];
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
            bool result = false;
            for (int i = 0; i < NavSetList.Count; i++)
            {
                AssetNavSet item = NavSetList[i];
                if (item.Key == key)
                {
                    item.AssetPath = path;
                    item.Hint = path.Split("/").Last();
                    NavSetList[i] = item;
                    result = true;
                    break;
                }
            }
            if (!result)
            {
                var asset = new AssetNavSet(key, path);
                NavSetList.Add(asset);
            }
            OnValidate();
        }
        private void OnValidate()
        {
            OnAssetsChange();
            Undo.RecordObject(this, "Add AssetNavSet");
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
