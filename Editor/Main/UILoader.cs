using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace PCP.Tools.WhichKey
{
    internal class UILoader
    {
        public VisualTreeAsset Preferences { private set; get; }
        public VisualTreeAsset ProjectSettings { private set; get; }
        public VisualTreeAsset List { private set; get; }
        public VisualTreeAsset KeySet { private set; get; }
        public VisualTreeAsset AssetData { private set; get; }
        public UILoader()
        {
            Preferences = Resources.Load<VisualTreeAsset>("UXML/Settings/Preferences");
            ProjectSettings = Resources.Load<VisualTreeAsset>("UXML/Settings/ProjectSettings");
            List = Resources.Load<VisualTreeAsset>("UXML/Templates/List");
            KeySet = Resources.Load<VisualTreeAsset>("UXML/Templates/KeySet");
            AssetData = Resources.Load<VisualTreeAsset>("UXML/Templates/AssetData");
        }
    }
}
