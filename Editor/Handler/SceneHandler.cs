using PCP.Tools.WhichKey;
using UnityEngine;
using UnityEditor;
namespace PCP.Tools.WhichKey
{

    public class SceneHandler : IWhichKeyHandler
    {
        private SceneData sceneData;

        public void Init()
        {
            if (WhichkeyProjectSettings.instance == null)
            {
                WhichKeyManager.LogError("Cant find project settings");
                return;
            }
            sceneData = WhichkeyProjectSettings.instance.CurrentSceneData;
        }
        public bool ProcessKey(char key)
        {
            if (sceneData == null)
            {
                WhichKeyManager.LogError("No Scene Data,Please Save Scene");
                return true;
            }
            //ping target gameobject by key
            for (int i = 0; i < sceneData.Targets.Length; i++)
            {
                if (sceneData.Targets[i].Key == key)
                {
                    var target = sceneData.Targets[i].Target;
                    if (target == "")
                    {
                        WhichKeyManager.LogInfo($"No Reference for {key}");
                        return true;
                    }
                    var go = GameObject.Find(target);
                    if (go == null)
                        WhichKeyManager.LogError($"Cant find {target}");
                    EditorGUIUtility.PingObject(go);
                }
            }
            return true;
        }
        public string[] GetLayerHints()
        {
            if (sceneData == null)
                return new string[0];
            return sceneData.KeyHints;
        }
    }
}
