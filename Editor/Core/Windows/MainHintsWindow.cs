using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace PCP.Tools.WhichKey
{
	public class MainHintsWindow : BaseWKWindow<MainHintsWindow>
	{
		#region Data
		//OPT
		//Maybe a structure
		//OPT
		internal static float lineHeight;
		private static float timeoutLen;
		private static bool followMouse;
		private static Vector2 fixedPosition;
		private static int maxHintLines;
		private static float maxColWidth;
		private static VisualTreeAsset hintLabel;
		private static VisualTreeAsset blankVE;
		private static StyleSheet hintLabelSS;
		#endregion
		internal static void Init()
		{
			// Setup Settings
			if (WhichKeyPreferences.instance == null)
			{
				WhichKeyManager.LogError("WhichKey Preferences instance is null");
				return;
			}
			WhichKeyManager.instance.ShowHintsWindow = Active;

			var pref = WhichKeyManager.Preferences;
			var uil = WhichKeyManager.mUILoader;
			followMouse = pref.WindowFollowMouse;
			fixedPosition = pref.FixedPosition;
			maxHintLines = pref.MaxHintLines;
			maxColWidth = pref.ColWidth;
			timeoutLen = pref.Timeout;
			hintLabel = uil.HintLabel;
			hintLabelSS = uil.HintLabelSS;
			blankVE = uil.BlankVE;
		}
		protected override void OnActive() { }

		private VisualElement mainFrame;
		private VisualElement labelFrame;
		private Label titleLabel;
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
			e.RegisterCallback<GeometryChangedEvent>(evt =>
			{
				lineHeight = e.layout.height;
			});
			labelFrame.Add(e);

			mainFrame.Clear();
			mainFrame.Add(titleLabel);
			mainFrame.Add(labelFrame);

			rootVisualElement.Add(mainFrame);
		}
		protected override void ShowHints()
		{
			string[] hints = WhichKeyManager.instance.GetHints();
			if (hints == null)
			{
				Close();
				return;
			}
			labelFrame.Clear();
			mHeight = lineHeight * (maxHintLines + 1) + 2 * mainFrame.resolvedStyle.paddingTop;
			var cols = Mathf.CeilToInt(hints.Length / 2f / maxHintLines);
			mWidth = cols * maxColWidth + mainFrame.resolvedStyle.paddingLeft * 2;
			maxSize = new Vector2(mWidth, mHeight);
			if (followMouse)
			{
				Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
				position = new Rect(mousePos.x - mWidth / 2, mousePos.y - mHeight / 2, mWidth, mHeight);
			}
			else
			{
				position = new Rect(fixedPosition.x, fixedPosition.y, mWidth, mHeight);
			}

			for (int j = 0; j < cols; j++)
			{
				var col = new VisualElement();
				col.style.flexDirection = FlexDirection.Column;
				col.style.width = maxColWidth;
				col.style.height = mHeight;
				for (int i = 0; i < maxHintLines; i++)
				{
					int ind = i + j * maxHintLines;
					if (ind * 2 >= hints.Length) break;
					var row = hintLabel.CloneTree().Q<VisualElement>();
					var k = row.Q<Label>("Key");
					var h = row.Q<Label>("Hint");
					k.text = hints[ind * 2];
					h.text = hints[ind * 2 + 1];
					row.style.width = maxColWidth;
					row.style.height = lineHeight;
					col.Add(row);
				}
				labelFrame.Add(col);
			}
		}
	}
}
