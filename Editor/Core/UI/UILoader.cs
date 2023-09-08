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
        public VisualTreeAsset WkBinder { private set; get; }
        public VisualTreeAsset KeySet { private set; get; }
        public VisualTreeAsset NavSet { private set; get; }
        public VisualTreeAsset SceneNav { private set; get; }
        public VisualTreeAsset BindWindow { private set; get; }
        //For window
        public VisualTreeAsset HintLabel { private set; get; }
        public VisualTreeAsset BlankVE { private set; get; }
        public VisualTreeAsset KeyLabel { private set; get; }
        public StyleSheet HintLabelSS { private set; get; }
        public void Refresh()
        {
            Preferences = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Settings/Preferences");
            ProjectSettings = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Settings/ProjectSettings");
            List = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/List");
            KeySet = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/KeySet");
            WkBinder = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/WkBinder");
            NavSet = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/NavSet");
            SceneNav = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/SceneNav");
            KeyLabel = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/KeyLabel");
            BindWindow = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/BindWindow");

            HintLabel = Resources.Load<VisualTreeAsset>("WhichKey/UXML/UI/HintLabel");
            HintLabelSS = Resources.Load<StyleSheet>("WhichKey/UXML/UI/HintLabelSS");
            BlankVE = Resources.Load<VisualTreeAsset>("WhichKey/UXML/UI/Blank");
        }
    }
}
