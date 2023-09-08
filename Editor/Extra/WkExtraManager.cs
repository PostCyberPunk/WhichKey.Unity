using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;


namespace PCP.Tools.WhichKey
{
    [FilePath("Project/Whichkey/ExtraData", FilePathAttribute.Location.ProjectFolder)]
    public class WkExtraManager : ScriptableSingleton<WkExtraManager>
    {
        #region SceneNav

        [SerializeField] private List<SceneNavData> savedSceneDatas = new();
        public SceneNavData CurrentSceneData;
        internal void Init()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
            var c_scene = SceneManager.GetActiveScene();
            SetSceneData(c_scene);
        }
        public static void Save()
        {
            Undo.RegisterCompleteObjectUndo(instance, "Save WhichKey Extra Data");
            instance.Save(true);
        }
        public void SaveSceneData()
        {
            CurrentSceneData?.SetupKeyHints();
            Save(true);
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
            SetSceneData(scene);
        }
        private void SetSceneData(Scene scene)
        {
            if (!scene.IsValid())
                WhichKeyManager.LogError($"WhichKey: SetCurrentSceneData: Invalid Scene");
            else if (scene.path == "")
                WhichKeyManager.LogInfo($"WhichKey:Save and reopen the scene to use WhichKey");
            else if (!FindScenedata(scene))
            {
                CurrentSceneData = new SceneNavData(AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path));
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

        #endregion

        #region AssetNav
        [SerializeField] public float WinTimeout;
        [SerializeField] public AssetsNavData[] NavAssetsDatas = new AssetsNavData[0];

        #endregion
    }
}
