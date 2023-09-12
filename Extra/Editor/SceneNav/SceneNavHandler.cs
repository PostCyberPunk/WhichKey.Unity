using UnityEditor;
using UnityEngine;
using PCP.WhichKey.Types;
using PCP.WhichKey.Log;
using PCP.WhichKey.Utils;
namespace PCP.WhichKey.Extra
{

    public class SceneNavHandler : IWkHandler
    {
        public float Timeout => 0;
        public float ColWidth => 400;
        private SceneNavData sceneData => WkExtraManager.instance?.CurrentSceneData;
        public bool Set = false;
        public void ProcessKey(int key)
        {
            if (sceneData == null)
            {
                WkLogger.LogError("No Scene Data,Please Save Scene");
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
                        WkLogger.LogInfo($"No Reference for {key.ToLabel()}");
                        return;
                    }
                    var go = GameObject.Find(target)?.transform;
                    if (go == null)
                    {
                        WkLogger.LogError($"Cant find {target}");
                        return;
                    }

                    Selection.activeTransform = go; EditorGUIUtility.PingObject(go);
                }
            }
        }
        public void SetTarget(int key)
        {
            var target = Selection.activeTransform;
            if (target == null)
            {
                WkLogger.LogWarning($"No Selectted Object");
                return;
            }
            for (int i = 0; i < sceneData.Targets.Count; i++)
            {
                if (sceneData.Targets[i].Key.lastKey == key)
                {
                    sceneData.Targets[i] = new SceneNavTarget(key, target.name, target.GetPath());
                    WkExtraManager.instance.SaveSceneData();
                    WkLogger.LogInfo($"Set {key.ToLabel()} to {target.name}");
                    return;
                }
            }

            sceneData.Targets.Add(new SceneNavTarget(key, target.name, target.GetPath()));
            WkExtraManager.instance.SaveSceneData();
            WkLogger.LogInfo($"Set {key.ToLabel()} to {target.name}");
        }
        public string[] GetLayerHints()
        {
            if (sceneData == null)
                return null;
            if (sceneData.Targets.Count == 0)
                return new string[1];
            return sceneData.KeyHints;
        }
        public string GetLayerName()
        {
            if (sceneData == null)
                return null;
            return sceneData.Targets.Count == 1 ? "Set some gameobjects first" : "Scene Navgation";
        }
    }
}
