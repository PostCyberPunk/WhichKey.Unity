using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PCP.WhichKey.UI;

namespace PCP.WhichKey.Extra
{

    public class SceneNavWindow : EditorWindow
    {
        private WkExtraManager mManager => WkExtraManager.instance;
        [MenuItem("WhichKey/Extra/SceneNav Setting")]
        public static void ShowWindow()
        {
            var window = GetWindow<SceneNavWindow>();
            window.titleContent = new GUIContent("Whichkey Scene Setting");
            window.RefeshData();
            window.Show();
        }
        private void CreateGUI()
        {
            if (mManager.CurrentSceneData == null)
            {
                rootVisualElement.Add(new Label("No Scene Data,Please Save Scene"));
                return;
            }
            var list = UILoader.instance.List.CloneTree().Q<ListView>();
            list.bindingPath = "Targets";
            var itemvt = mManager.SceneNav;
            list.makeItem = itemvt.CloneTree;


            var binder = new BindableElement();
            binder.bindingPath = "CurrentSceneData";
            binder.Add(list);
            rootVisualElement.Add(binder);

            var okbtn = new Button(() =>
            {
                mManager.SaveSceneData();
            });
            okbtn.text = "Save";
            rootVisualElement.Add(okbtn);
            rootVisualElement.Bind(mManager.GetSerializedObject());
        }
        public void RefeshData()
        {
            rootVisualElement.Bind(mManager.GetSerializedObject());
            rootVisualElement.MarkDirtyRepaint();
        }
    }
}
