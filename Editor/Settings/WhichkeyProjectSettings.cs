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
        [SerializeField] public SceneData[] savedSceneDatas = new SceneData[0];
        public Dictionary<int, SceneData?> sceneDatas = new Dictionary<int, SceneData?>();
        // internal List<SceneData> sceneDataList = new List<SceneData>();
        internal SceneData? CurrentSceneData;
        internal void Init()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
            var c_scene = SceneManager.GetActiveScene();
            SetCurrentSceneData(c_scene);
            Debug.Log($"WhichKey: Init: {c_scene.name},path:{c_scene.path}");
        }
        private void SetupDict()
        {
            foreach (var data in savedSceneDatas)
            {
                sceneDatas.Add(data.Path.GetHashCode(), data);
            }
        }
        public static void Save()
        {
            Undo.RegisterCompleteObjectUndo(instance, "Save WhichKey Project Settings");
            instance.Save(true);
        }
        private void SaveDict()
        {

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
            else if (sceneDatas.TryGetValue(scene.path.GetHashCode(), out var data))
            {
                CurrentSceneData = data;
                return;
            }
            else
            {
                CurrentSceneData = new SceneData(scene.path, new GameObject[0]);
                sceneDatas.Add(scene.path.GetHashCode(), CurrentSceneData);
                Save();
                return;
            }
            CurrentSceneData = null;
        }
    }

}
