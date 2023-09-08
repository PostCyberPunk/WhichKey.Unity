using PCP.Tools.WhichKey;
using UnityEngine;
using UnityEditor;
using System.Xml.Schema;
namespace PCP.Tools.WhichKey
{

    public class SceneNavHandler : IWKHandler
    {
        private SceneNavData sceneData => WkExtraManager.instance?.CurrentSceneData;
        public bool Set = false;
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
                    var go = GameObject.Find(target)?.transform;
                    if (go == null)
                    {
                        WhichKeyManager.LogError($"Cant find {target}");
                        return;
                    }

                    Selection.activeTransform = go; EditorGUIUtility.PingObject(go);
                }
            }
        }
        public void SetTarget(int key)
        {
            var target = Selection.activeTransform;
            if(target==null)
            {
                WhichKeyManager.LogWarning($"No Selectted Object");
                return;
            }
            for (int i = 0; i < sceneData.Targets.Count; i++)
            {
                if (sceneData.Targets[i].Key.lastKey == key)
                {
                    sceneData.Targets[i] = new SceneNavTarget(key, target.name, target.GetPath());
                    WkExtraManager.instance.SaveSceneData();
                    WhichKeyManager.LogInfo($"Set {key.ToLabel()} to {target.name}");
                    return;
                }
            }

            sceneData.Targets.Add(new SceneNavTarget(key, target.name, target.GetPath()));
            WkExtraManager.instance.SaveSceneData();
            WhichKeyManager.LogInfo($"Set {key.ToLabel()} to {target.name}");
        }
        public string[] GetLayerHints()
        {
            if (sceneData == null)
                return null;
            return sceneData.KeyHints;
        }
    }
}
