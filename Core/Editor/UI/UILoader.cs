using UnityEngine;
using UnityEngine.UIElements;
using PCP.WhichKey.Log;
using PCP.WhichKey.Core;

namespace PCP.WhichKey.UI
{
	public class UILoader
	{
		public static UILoader instance;

		//Dummy
		public VisualTreeAsset List { private set; get; }

		public VisualTreeAsset BlankVE { private set; get; }

		//Settings
		public VisualTreeAsset Preferences { private set; get; }
		public VisualTreeAsset ProjectSettings { private set; get; }
		public VisualTreeAsset WkBinder { private set; get; }
		public VisualTreeAsset BindWindow { private set; get; }
		public VisualTreeAsset KeySet { private set; get; }
		public VisualTreeAsset LayerSet { private set; get; }

		public VisualTreeAsset MenuSet { private set; get; }

		//For window
		public VisualTreeAsset HintLabel { private set; get; }
		public VisualTreeAsset KeyLabel { private set; get; }

		public StyleSheet HintLabelSS { private set; get; }


		public UILoader()
		{
			if (instance != null)
			{
				WkLogger.LogError("Multiple UILoader instance found");
			}

			instance = this;
		}

		public void Refresh()
		{
			List = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/List");
			BlankVE = Resources.Load<VisualTreeAsset>("WhichKey/UXML/UI/Blank");

			Preferences = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Settings/Preferences");
			ProjectSettings = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Settings/ProjectSettings");
			WkBinder = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/WkBinder");
			BindWindow = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/BindWindow");
			KeySet = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/KeySet");
			LayerSet = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/LayerSet");
			MenuSet = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/MenuSet");

			KeyLabel = Resources.Load<VisualTreeAsset>("WhichKey/UXML/Templates/KeyLabel");
			HintLabel = Resources.Load<VisualTreeAsset>("WhichKey/UXML/UI/HintLabel");
			if (WhichkeyProjectSettings.instance?.HintLabelSS != null)
				HintLabelSS = WhichkeyProjectSettings.instance.HintLabelSS;
			else
				HintLabelSS = Resources.Load<StyleSheet>("WhichKey/UXML/UI/HintLabelSS");

		}
	}
}