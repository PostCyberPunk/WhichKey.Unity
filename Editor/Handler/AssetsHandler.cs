using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;
namespace PCP.Tools.WhichKey
{

    public class AssetsHandler : IWhichKeyHandler
    {
        private ProjectAssetsData assetsData;
        public bool showHint;
        public bool ProecessArg(int index)
        {
            assetsData = WhichkeyProjectSettings.instance.projectAssetsDatas[index];
            if (assetsData != null)
            {
                //OPT bad reference
                if (showHint)
                    MainHintsWindow.instance.ForceActive();
                return false;
            }

            WhichKey.LogError("AssetsHandler: No Assets Data Found For Index: " + index);
            return true;
        }
        public bool ProcessKey(char key)
        {
            string path = assetsData.GetAssetsPathByKey(key);
            if (string.IsNullOrEmpty(path))
            {
                WhichKey.LogError("AssetsHandler: No Assets Path Found For Key: " + key);
                return true;
            }
            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
            return true;
        }
        public string[] GetLayerHints()
        {
            return assetsData == null ? null : assetsData.LayerHints;
        }
    }
}
