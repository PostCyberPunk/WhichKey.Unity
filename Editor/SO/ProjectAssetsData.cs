using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
namespace PCP.Tools.WhichKey
{

    [CreateAssetMenu(fileName = "ProjectAssetsData", menuName = "Whichkey/ProjectAssetsData", order = 1)]
    public class ProjectAssetsData : ScriptableObject
    {
        [SerializeField] public AssetsData[] Assets = new AssetsData[0];
        [SerializeField] public string[] LayerHints;
        public string testr = "hello";
        public string GetAssetsPathByKey(char key)
        {
            foreach (var item in Assets)
            {
                if (item.Key == key)
                    return item.AssetPath;
            }
            return "";
        }
    }
    [System.Serializable]
    public struct AssetsData
    {
        public char Key;
        public string Hint;
        public string AssetPath;
    }
    [CustomEditor(typeof(ProjectAssetsData))]

    public class ProjectAssetsDataEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            VisualTreeAsset listvt = Resources.Load<VisualTreeAsset>("Settings/List");
            VisualTreeAsset itemvt = Resources.Load<VisualTreeAsset>("Settings/AssetData");
            root.Add(listvt.CloneTree());
            var list = root.Q<ListView>("List");
            list.bindingPath = "Assets";
            list.makeItem = itemvt.CloneTree;
            return root;
        }
    }
}
