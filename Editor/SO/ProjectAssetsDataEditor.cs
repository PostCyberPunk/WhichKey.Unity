using UnityEditor;
using UnityEngine.UIElements;
namespace PCP.Tools.WhichKey
{
    [CustomEditor(typeof(ProjectAssetsData))]

    public class ProjectAssetsDataEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            var vts = WhichKey.instance.mUILoader;
            VisualTreeAsset listvt = vts.List;
            VisualTreeAsset itemvt = vts.AssetData;
            root.Add(listvt.CloneTree());
            var list = root.Q<ListView>("List");
            list.bindingPath = "AssetsData";
            list.makeItem = itemvt.CloneTree;
            return root;
        }
    }
}
