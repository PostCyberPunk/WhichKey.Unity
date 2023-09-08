using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Xml.Schema;
using UnityEditor.UIElements;
using System.Linq;
namespace PCP.Tools.WhichKey
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
            var list = WhichKeyManager.mUILoader.List.CloneTree().Q<ListView>();
            list.bindingPath = "Targets";
            list.makeItem = WhichKeyManager.mUILoader.SceneNav.CloneTree;

            var binder = new BindableElement();
            binder.bindingPath = "CurrentSceneData";
            binder.Add(list);
            rootVisualElement.Add(binder);

            // var infoLabel = new Label();
            // infoLabel.text = "Select GameObject to set or remove reference";
            // var setButton = new Button(() =>
            // {
            //     var go = Selection.activeTransform;
            //     if (go == null)
            //         return;
            //     //FIXME: add a method in scneneNavData to set target by index
            //     mManager.CurrentSceneData.Targets[list.selectedIndex].ChangeTarget(go.name, go.GetPath());
            //     mManager.CurrentSceneData.SetupKeyHints();
            //     mManager.SaveSceneData();
            // });
            // setButton.text = "Set Reference";

            // var delButton = new Button(() =>
            // {
            //     mManager.CurrentSceneData.Targets[list.selectedIndex].ChangeTarget("", "");
            //     mManager.CurrentSceneData.SetupKeyHints();
            //     mManager.SaveSceneData();
            // });
            // delButton.text = "Remove Reference";

            // rootVisualElement.Add(setButton);
            // rootVisualElement.Add(delButton);
            // rootVisualElement.Add(infoLabel);
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
// private void AddBookmark(SceneBookmarks sceneBookmarks)
// {
//     SceneBookmarks.SceneGameObjectReference newReference = new SceneBookmarks.SceneGameObjectReference();
//     ArrayUtility.Add(ref sceneBookmarks.bookmarks, newReference);
//     EditorUtility.SetDirty(sceneBookmarks);
// }
