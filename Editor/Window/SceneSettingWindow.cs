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
            window.titleContent = new GUIContent("Scene Setting");
            window.Show();
        }
        private void CreateGUI()
        {
            var data = WhichkeyProjectSettings.instance.CurrentSceneData;
            if (data == null)
            {
                rootVisualElement.Add(new Label("No Scene Data,Please Save Scene"));
                return;
            }
            var list = WhichKeyManager.mUILoader.List.CloneTree().Q<ListView>();
            list.bindingPath = "Targets";
            list.makeItem = WhichKeyManager.mUILoader.KeyGO.CloneTree;
            // list.makeItem = () =>
            // {
            //     var vt = WhichKeyManager.mUILoader.KeyGO.CloneTree();
            //     vt.Q<ObjectField>("GO").RegisterValueChangedCallback(evt =>
            //     {
            //         if (evt.newValue == null)
            //             return;
            //         GameObject go = (GameObject)evt.newValue;
            //         data.Targets[evt.].Target = go.GetPath();
            //     });
            //     return vt;
            // };
            // list.bindItem += (e, i) =>
            // {
            //     var vt = e.Q<ObjectField>();
            //     vt.value = GameObject.Find(data.Targets[i].Target);
            // };

            var binder = new BindableElement();
            binder.bindingPath = "CurrentSceneData";
            binder.Add(list);

            var toggle = new Toggle("Show Hint Instant");
            toggle.bindingPath = "showHintInstant";
            rootVisualElement.Add(toggle);
            rootVisualElement.Add(binder);

        }
        private void OnEnable()
        {
            rootVisualElement.Bind(WhichkeyProjectSettings.instance.GetSerializedObject());
        }
        private void OnValidate()
        {
            WhichkeyProjectSettings.Save();
        }
    }
}
// private void AddBookmark(SceneBookmarks sceneBookmarks)
// {
//     SceneBookmarks.SceneGameObjectReference newReference = new SceneBookmarks.SceneGameObjectReference();
//     ArrayUtility.Add(ref sceneBookmarks.bookmarks, newReference);
//     EditorUtility.SetDirty(sceneBookmarks);
// }
