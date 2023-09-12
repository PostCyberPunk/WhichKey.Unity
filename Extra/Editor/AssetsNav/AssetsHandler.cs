using System.Linq;
using UnityEditor;
using UnityEngine;
using PCP.WhichKey.Types;
using PCP.WhichKey.Log;
using PCP.WhichKey.Utils;

namespace PCP.WhichKey.Extra
{

    internal class AssetsHandler : IWkHandler
    {
        private WkExtraManager mDataManger => WkExtraManager.instance;
        private AssetsNavData assetsData;
        private System.Action<int> mProcessKey;
        public bool ProecessArg(int index)
        {
            if (index >= mDataManger.NavAssetsDatas.Count())
            {
                WkLogger.LogError("AssetsNavData Index Out Of Range");
                return false;
            }
            assetsData = mDataManger.NavAssetsDatas[index];
            if (assetsData != null)
            {
                //TEMP:Need use better wan to encapsulate Window callback
                // WhichKeyManager.instance.OverrideWindowTimeout(mDataManger.WinTimeout);
                return true;
            }

            WkLogger.LogError("AssetsNav: No Assets Data Found For Index: " + index);
            return false;
        }
        public void ChangeAction(bool save)
        {
            if (save)
                mProcessKey = SaveAssetPath;
            else
                mProcessKey = LoadAssetToSeletion;
        }
        public void ProcessKey(int key)
        {
            mProcessKey?.Invoke(key);
        }
        private void LoadAssetToSeletion(int key)
        {
            string path = assetsData.GetAssetsPathByKey(key);
            if (string.IsNullOrEmpty(path))
            {
                WkLogger.LogInfo($"AssetsNav: No Assets Path Found For Key: {key.ToLabel()},check your path: {path}");
                return;
            }
            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            if (obj == null)
            {
                WkLogger.LogWarning($"AssetsNav: No Assets Found For Key: {key.ToLabel()},check your path: {path}");
                return;
            }
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
            EditorUtility.FocusProjectWindow();
        }
        private void SaveAssetPath(int key)
        {
            var go = Selection.activeObject;
            if (go == null)
                WkLogger.LogInfo($"AssetsNav: No Selectted Object");
            else if (!AssetDatabase.Contains(go))
                WkLogger.LogInfo($"AssetsNav: Cant Save GameObject,select an asset");
            else
            {
                string path = AssetDatabase.GetAssetPath(go);
                assetsData.SetAssetsPathByKey(key, path);
            }
        }
        public string[] GetLayerHints()
        {
            if (assetsData == null) return null;
            return assetsData.LayerHints.Length == 0 ? new string[1] : assetsData.LayerHints;
        }
        public string GetLayerName()
        {
            if (assetsData == null) return null;
            return assetsData.LayerHints.Length == 0 ? "Add some assets first" : assetsData.name;
        }
    }
}
