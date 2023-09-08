using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
namespace PCP.Tools.WhichKey
{
    [CustomEditor(typeof(AssetsNavData))]

    public class AssetsNavDataEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            var vts = WhichKeyManager.mUILoader;
            VisualTreeAsset listvt = vts.List;
            VisualTreeAsset itemvt = vts.NavSet;

            var list = listvt.CloneTree().Q<ListView>();
            root.Add(list);
            list.bindingPath = "NavSetList";
            list.makeItem = () =>
            {
                var item = itemvt.CloneTree();
                var wk = item.Q<PropertyField>("WkBinder");
                wk.userData = new WkBinderSetting(1);
                return item;
            };


            return root;
        }
    }
}
