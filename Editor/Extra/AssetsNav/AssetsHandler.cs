using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;
using System.Linq;
namespace PCP.Tools.WhichKey
{

    internal class AssetsHandler : IWKHandler
    {
        private WkExtraManager mDataManger => WkExtraManager.instance;
        private AssetsNavData assetsData;
        private System.Action<int> mProcessKey;
        public bool ProecessArg(int index)
        {
            if(index>=mDataManger.NavAssetsDatas.Count())
            {
                WhichKeyManager.LogError("AssetsNavData Index Out Of Range");
                return false;
            }
            assetsData = mDataManger.NavAssetsDatas[index];
            if (assetsData != null)
            {
                WhichKeyManager.instance.OverrideWindowTimeout(mDataManger.WinTimeout);
                return true;
            }

            WhichKeyManager.LogError("AssetsNav: No Assets Data Found For Index: " + index);
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
                WhichKeyManager.LogInfo($"AssetsNav: No Assets Path Found For Key: {key.ToLabel()},check your path: {path}");
                return;
            }
            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
            if (obj == null)
            {
                WhichKeyManager.LogWarning($"AssetsNav: No Assets Found For Key: {key.ToLabel()},check your path: {path}");
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
                WhichKeyManager.LogInfo($"AssetsNav: No Selectted Object");
            else if (!AssetDatabase.Contains(go))
                WhichKeyManager.LogInfo($"AssetsNav: Cant Save GameObject,select an asset");
            else
            {
                string path = AssetDatabase.GetAssetPath(go);
                assetsData.SetAssetsPathByKey(key, path);
            }
        }
        public string[] GetLayerHints()
        {
            return assetsData == null ? null : assetsData.LayerHints;
        }
    }
}
