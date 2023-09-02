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
                rootVisualElement.Add(new Label("No Scene Data"));
                return;
            }
            var list = WhichKeyManager.mUILoader.List.CloneTree().Q<ListView>();
            list.bindingPath = "Targets";
            list.makeItem = () => new PropertyField();
            rootVisualElement.Add(list);
        }
    }
}
