using PCP.Tools.WhichKey;
using UnityEngine;
using UnityEditor;
namespace PCP.Tools.WhichKey
{

    public class SceneHandler : IWKHandler
    {
        private SceneNavData sceneData;

        public bool Set = false;
        public void Init()
        {
            if (WkExtraManager.instance == null)
            {
                WhichKeyManager.LogError("Cant find project settings");
                return;
            }
            sceneData = WkExtraManager.instance.CurrentSceneData;
        }
        public void ProcessKey(int key)
        {
            if (sceneData == null)
            {
                WhichKeyManager.LogError("No Scene Data,Please Save Scene");
                return;
            }
            if (Set)
                SetTarget(key);
            else
                LoadTarget(key);
        }
        public void LoadTarget(int key)
        {
            //ping target gameobject by key
            for (int i = 0; i < sceneData.Targets.Length; i++)
            {
                if (sceneData.Targets[i].Key == key)
                {
                    var target = sceneData.Targets[i].Target;
                    if (target == "")
                    {
                        WhichKeyManager.LogInfo($"No Reference for {key.ToLabel()}");
                        return;
                    }
                    var go = GameObject.Find(target);
                    if (go == null)
                        WhichKeyManager.LogError($"Cant find {target}");
                    Selection.activeGameObject = go;
                    EditorGUIUtility.PingObject(go);
                }
            }
        }
        public void SetTarget(int key)
        {

        }
        public string[] GetLayerHints()
        {
            if (sceneData == null)
                return new string[0];
            return sceneData.KeyHints;
        }
    }
}
