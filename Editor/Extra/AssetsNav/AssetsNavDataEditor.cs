using UnityEditor;
using UnityEngine.UIElements;
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
            VisualTreeAsset itemvt = vts.AssetData;

            var list = listvt.CloneTree().Q<ListView>();
            root.Add(list);
            list.bindingPath = "NavSetList";
            list.makeItem = itemvt.CloneTree;

            return root;
        }
    }
}
