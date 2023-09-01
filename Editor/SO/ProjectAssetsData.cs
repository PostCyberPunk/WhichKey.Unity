using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Unity.Properties;
namespace PCP.Tools.WhichKey
{

    [CreateAssetMenu(fileName = "ProjectAssetsData", menuName = "Whichkey/ProjectAssetsData", order = 1)]
    public class ProjectAssetsData : ScriptableObject
    {
        [SerializeField] public string[] LayerHints;
        [SerializeField] private AssetsData[] Assets = new AssetsData[0];
        public string GetAssetsPathByKey(char key)
        {
            foreach (var item in Assets)
            {
                if (item.Key == key)
                    return item.AssetPath;
            }
            return "";
        }
        private void OnValidate()
        {
            OnAssetsChange();
        }
        private void OnAssetsChange()
        {
            LayerHints = new string[Assets.Length * 2];
            for (int i = 0; i < Assets.Length; i++)
            {
                LayerHints[i * 2] = Assets[i].Key.ToString();
                LayerHints[i * 2 + 1] = Assets[i].Hint;
            }
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
            var vts = WhichKey.instance.mUILoader;
            VisualTreeAsset listvt = vts.List;
            VisualTreeAsset itemvt = vts.AssetData;
            root.Add(listvt.CloneTree());
            var list = root.Q<ListView>("List");
            list.bindingPath = "Assets";
            list.makeItem = itemvt.CloneTree;
            return root;
        }
    }
}
