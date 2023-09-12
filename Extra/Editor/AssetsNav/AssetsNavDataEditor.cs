using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using PCP.WhichKey.UI;

namespace PCP.WhichKey.Extra
{
    [CustomEditor(typeof(AssetsNavData))]

    public class AssetsNavDataEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            var vts = UILoader.instance;
            VisualTreeAsset listvt = vts.List;
            VisualTreeAsset itemvt = WkExtraManager.instance.NavSet;

            var list = listvt.CloneTree().Q<ListView>();
            root.Add(list);
            list.bindingPath = "NavSetList";
            list.makeItem = itemvt.CloneTree;

            return root;
        }
    }
}
