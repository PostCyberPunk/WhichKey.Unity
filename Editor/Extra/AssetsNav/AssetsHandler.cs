using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;
namespace PCP.Tools.WhichKey
{

    internal class AssetsHandler : IWKHandler
    {
        private AssetsDataManager mDataManger => AssetsDataManager.instance;
        private ProjectAssetsData assetsData;
        private System.Action<int> mProcessKey;
        public bool ProecessArg(int index)
        {
            assetsData = mDataManger.projectAssetsDatas[index];
            if (assetsData != null)
            {
                //OPT bad reference
                WhichKeyManager.instance.OverrideWindowTimeout(mDataManger.WinTimeout);
                return true;
            }

            WhichKeyManager.LogError("AssetsNav: No Assets Data Found For Index: " + index);
            return false;
        }
        public void ProcessKey(int key)
        {
            Debug.Log("ProcessKey: " + key);
        }
        private void LoadAsset(int key)
        {
            string path = assetsData.GetAssetsPathByKey(key);
            if (string.IsNullOrEmpty(path))
            {
                WhichKeyManager.LogInfo($"AssetsNav: No Assets Path Found For Key: {key.ToLabel()},check your path: {path}");
                return;
            }
            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
            EditorUtility.FocusProjectWindow();
        }
        private void SaveAsset(int key)
        {
            var go = Selection.activeObject;
            //FIXME
            //TODO: add support for multiple selection
        }
        public string[] GetLayerHints()
        {
            return assetsData == null ? null : assetsData.LayerHints;
        }
    }
}
