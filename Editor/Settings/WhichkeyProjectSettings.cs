using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace PCP.Tools.WhichKey
{
    [FilePath("Project/WhichkeyProjectSettings", FilePathAttribute.Location.ProjectFolder)]
    public class WhichkeyProjectSettings : ScriptableSingleton<WhichkeyProjectSettings>
    {
        [SerializeField] public bool showHintInstant = true;
        [SerializeField] public KeySet[] KeyMap;
        [SerializeField] public ProjectAssetsData[] projectAssetsDatas = new ProjectAssetsData[0];
        [SerializeField] public List<SceneData> savedSceneDatas = new();
        public SceneData CurrentSceneData;
        internal void Init()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
            var c_scene = SceneManager.GetActiveScene();
            SetCurrentSceneData(c_scene);
            Debug.Log($"WhichKey: Init: {c_scene.name},path:{c_scene.path}");
        }
        public static void Save()
        {
            Undo.RegisterCompleteObjectUndo(instance, "Save WhichKey Project Settings");
            instance.Save(true);
        }
        internal SerializedObject GetSerializedObject()
        {
            return new SerializedObject(this);
        }
        private void OnEnable()
        {
            hideFlags &= ~HideFlags.NotEditable;
        }
        private void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            SetCurrentSceneData(scene);
        }
        private void SetCurrentSceneData(Scene scene)
        {
            if (!scene.IsValid())
                WhichKeyManager.LogError($"WhichKey: SetCurrentSceneData: Invalid Scene");
            else if (scene.path == "")
                WhichKeyManager.LogInfo($"WhichKey:Save and reopen the scene to use WhichKey");
            else if (!FindScenedata(scene))
            {
                CurrentSceneData = new SceneData(AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path));
                savedSceneDatas.Add(CurrentSceneData);
                Save();
            }
        }
        private bool FindScenedata(Scene scene)
        {
            SceneAsset c_scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
            CurrentSceneData = savedSceneDatas.Find(x => x.Scene == c_scene);
            return CurrentSceneData != null;
        }

}
