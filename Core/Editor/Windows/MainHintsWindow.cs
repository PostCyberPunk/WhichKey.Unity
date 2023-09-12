using UnityEngine;
using UnityEngine.UIElements;
using PCP.WhichKey.Types;
using PCP.WhichKey.Log;
using PCP.WhichKey.UI;

namespace PCP.WhichKey.Core
{
	internal class MainHintsWindow : WkBaseWindow
	{
		protected static WhichKeyManager wkm => WhichKeyManager.instance;

		#region Data

		//OPT
		//Maybe a structure
		//OPT
		internal static float lineHeight;
		private static bool followMouse;
		private static Vector2 fixedPosition;
		private static int maxHintLines;
		private static float maxColWidth;
		private static VisualTreeAsset hintLabel;
		private static VisualTreeAsset blankVE;
		private static StyleSheet hintLabelSS;

		#endregion

		public float overrideTimeout;
		public float overrideColWidth;
		private float mColWidth;
		public override void OnActive()
		{
			if (overrideTimeout >= 0)
				timeout = overrideTimeout;
			else
				timeout = DefaultTimeoutLen;
			if (overrideColWidth >= 0)
				mColWidth = overrideColWidth;
			else
				mColWidth = maxColWidth;
		}
		public static void Init()
		{
			// Setup Settings
			if (WhichKeyPreferences.instance == null)
			{
				WkLogger.LogError("WhichKey Preferences instance is null");
				return;
			}

			var pref = WhichKeyPreferences.instance;
			var uil = UILoader.instance;
			followMouse = pref.WindowFollowMouse;
			fixedPosition = pref.FixedPosition;
			maxHintLines = pref.MaxHintLines;
			maxColWidth = pref.ColWidth;
			hintLabel = uil.HintLabel;
			hintLabelSS = uil.HintLabelSS;
			blankVE = uil.BlankVE;

			//FIXME
			// lineHeight = 60;
		}

		private VisualElement mainFrame;
		private VisualElement labelFrame;
		private Label titleLabel;
		public string[] Hints;
		public string Title;
		private void CreateGUI()
		{
			mainFrame = blankVE.CloneTree().Q<VisualElement>();
			if (mainFrame == null) return;
			mainFrame.styleSheets.Add(hintLabelSS);
			mainFrame.AddToClassList("main");

			labelFrame = blankVE.CloneTree().Q<VisualElement>();
			labelFrame.style.flexDirection = FlexDirection.Row;

			titleLabel = new Label("WhichKey");
			titleLabel.AddToClassList("title");
			var e = hintLabel.CloneTree().ElementAt(0);
			e.RegisterCallback<GeometryChangedEvent>(evt => { lineHeight = e.layout.height; });
			labelFrame.Add(e);

			mainFrame.Clear();
			mainFrame.Add(titleLabel);
			mainFrame.Add(labelFrame);

			rootVisualElement.Add(mainFrame);
		}

		protected override void ShowHints()
		{
			if (Hints == null)
			{
				Close();
				return;
			}

			titleLabel.text = Title == null ? "WhichKey:No Hints" : Title;
			labelFrame.Clear();
			if (Hints.Length == 1)
			{
				mDepth = 1;
				mHeight = lineHeight + 2 * mainFrame.resolvedStyle.paddingTop;
				mWidth = mColWidth + mainFrame.resolvedStyle.paddingLeft * 2;
				maxSize = new Vector2(mWidth, mHeight);
			}
			else if (Hints.Length < 1)
			{
				Close();
				return;
			}
			else
			{
				mHeight = lineHeight * (maxHintLines + 1) + 2 * mainFrame.resolvedStyle.paddingTop;
				var cols = Mathf.CeilToInt(Hints.Length / 2f / maxHintLines);
				mWidth = cols * mColWidth + mainFrame.resolvedStyle.paddingLeft * 2;
				maxSize = new Vector2(mWidth, mHeight);

				for (int j = 0; j < cols; j++)
				{
					var col = new VisualElement();
					col.style.flexDirection = FlexDirection.Column;
					col.style.width = mColWidth;
					col.style.height = mHeight;
					for (int i = 0; i < maxHintLines; i++)
					{
						int ind = i + j * maxHintLines;
						if (ind * 2 >= Hints.Length) break;
						var row = hintLabel.CloneTree().Q<VisualElement>();
						var k = row.Q<Label>("Key");
						var h = row.Q<Label>("Hint");
						k.text = Hints[ind * 2];
						h.text = Hints[ind * 2 + 1];
						row.style.width = mColWidth;
						row.style.height = lineHeight;
						col.Add(row);
					}

					labelFrame.Add(col);
				}
			}

			if (followMouse)
			{
				Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
				position = new Rect(mousePos.x - mWidth / 2, mousePos.y - mHeight / 2, mWidth, mHeight);
			}
			else
			{
				position = new Rect(fixedPosition.x, fixedPosition.y, mWidth, mHeight);
			}
		}
	}
}