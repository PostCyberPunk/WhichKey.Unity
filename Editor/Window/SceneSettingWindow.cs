using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Xml.Schema;
using UnityEditor.UIElements;
namespace PCP.Tools.WhichKey
{

    public class SceneSettingWindow : EditorWindow
    {
        [MenuItem("WhichKey/Scene Setting")]
        public static void ShowWindow()
        {
            var window = GetWindow<SceneSettingWindow>();
            window.titleContent = new GUIContent("Whichkey Scene Setting");
            window.Show();
        }
        private void CreateGUI()
        {
            if (WhichkeyProjectSettings.instance.CurrentSceneData == null)
            {
                rootVisualElement.Add(new Label("No Scene Data,Please Save Scene"));
                return;
            }
            var list = WhichKeyManager.mUILoader.List.CloneTree().Q<ListView>();
            list.bindingPath = "Targets";
            list.makeItem = WhichKeyManager.mUILoader.KeyGO.CloneTree;

            var binder = new BindableElement();
            binder.bindingPath = "CurrentSceneData";
            binder.Add(list);

            var infoLabel = new Label();
            infoLabel.text = "Select GameObject to set or remove reference";
            var setButton = new Button(() =>
            {
                var go = Selection.activeGameObject;
                if (go == null)
                    return;
                var data = WhichkeyProjectSettings.instance.CurrentSceneData;
                data.Targets[list.selectedIndex].Target = Selection.activeGameObject.GetPath();
                WhichkeyProjectSettings.instance.SaveSceneData();
            });
            setButton.text = "Set Reference";

            var delButton = new Button(() =>
            {
                var data = WhichkeyProjectSettings.instance.CurrentSceneData;
                data.Targets[list.selectedIndex].Target = "";
                WhichkeyProjectSettings.instance.SaveSceneData();
            });
            delButton.text = "Remove Reference";

            rootVisualElement.Add(binder);
            rootVisualElement.Add(setButton);
            rootVisualElement.Add(delButton);
            rootVisualElement.Add(infoLabel);

        }
        private void OnEnable()
        {
            rootVisualElement.Bind(WhichkeyProjectSettings.instance.GetSerializedObject());
        }
        private void OnValidate()
        {
            WhichkeyProjectSettings.instance?.SaveSceneData();
        }
    }
}
// private void AddBookmark(SceneBookmarks sceneBookmarks)
// {
//     SceneBookmarks.SceneGameObjectReference newReference = new SceneBookmarks.SceneGameObjectReference();
//     ArrayUtility.Add(ref sceneBookmarks.bookmarks, newReference);
//     EditorUtility.SetDirty(sceneBookmarks);
// }
