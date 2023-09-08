using PCP.Tools.WhichKey;
using UnityEngine;
using UnityEditor;
using System.Xml.Schema;
namespace PCP.Tools.WhichKey
{

    public class SceneNavHandler : IWKHandler
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
            for (int i = 0; i < sceneData.Targets.Count; i++)
            {
                if (sceneData.Targets[i].Key.lastKey == key)
                {
                    var target = sceneData.Targets[i].Target;
                    if (target == "")
                    {
                        WhichKeyManager.LogInfo($"No Reference for {key.ToLabel()}");
                        return;
                    }
                    var go = GameObject.Find(target).transform;
                    if (go == null)
                        WhichKeyManager.LogError($"Cant find {target}");
                    Selection.activeTransform = go;
                    EditorGUIUtility.PingObject(go);
                }
            }
        }
        public void SetTarget(int key)
        {
            for (int i = 0; i < sceneData.Targets.Count; i++)
            {
                SceneNavTarget item = sceneData.Targets[i];
                if (item.Key.lastKey == key)
                {
                    item.Target = Selection.activeTransform.GetPath();
                    WkExtraManager.Save();
                    WhichKeyManager.LogInfo($"Set {key.ToLabel()} to {item.Target}");
                    return;
                }
            }
            var target = new SceneNavTarget(key, Selection.activeTransform.GetPath());
            sceneData.Targets.Add(target);
        }
        public string[] GetLayerHints()
        {
            if (sceneData == null)
                return new string[0];
            return sceneData.KeyHints;
        }
    }
}
