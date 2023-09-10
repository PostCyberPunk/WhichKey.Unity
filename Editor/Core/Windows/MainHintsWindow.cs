using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Net.Sockets;

namespace PCP.Tools.WhichKey
{
	internal class MainHintsWindow : WkBaseWindow
	{
		protected static WhichKeyManager wkm => WhichKeyManager.instance;
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
				WkLogger.LogError("WhichKey Preferences instance is null");
				return;
			}
			//BAD
			wkm.ShowHintsWindow = Active;

			var pref = WhichKeyPreferences.instance;
			var uil = UILoader.instance;
			followMouse = pref.WindowFollowMouse;
			fixedPosition = pref.FixedPosition;
			maxHintLines = pref.MaxHintLines;
			maxColWidth = pref.ColWidth;
			timeoutLen = pref.Timeout;
			hintLabel = uil.HintLabel;
			hintLabelSS = uil.HintLabelSS;
			blankVE = uil.BlankVE;

			//FIXME
			// lineHeight = 60;
		}
		protected override void OnActive()
		{
			// TEMP
			// wkm.OverrideWindowTimeout = instance.OverriderTimeout;
			// wkm.CloseHintsWindow = instance.ShouldClose;
			// wkm.UpdateHints = UpdateHints;
		}

		private VisualElement mainFrame;
		private VisualElement labelFrame;
		private Label titleLabel;
		private string[] Hints;
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
			Hints = wkm.GetHints();
			if (Hints == null)
			{
				Close();
				return;
			}
			if (Hints.Length <= 1)
			{
				DummyWindow();
				return;
			}
			labelFrame.Clear();
			mHeight = lineHeight * (maxHintLines + 1) + 2 * mainFrame.resolvedStyle.paddingTop;
			var cols = Mathf.CeilToInt(Hints.Length / 2f / maxHintLines);
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
					if (ind * 2 >= Hints.Length) break;
					var row = hintLabel.CloneTree().Q<VisualElement>();
					var k = row.Q<Label>("Key");
					var h = row.Q<Label>("Hint");
					k.text = Hints[ind * 2];
					h.text = Hints[ind * 2 + 1];
					row.style.width = maxColWidth;
					row.style.height = lineHeight;
					col.Add(row);
				}
				labelFrame.Add(col);
			}
		}
		// private void UpdateHints(string[] hints)
		// {
		// 	Hints = hints;
		// 	UpdateHintsWindow();
		// }

	}
}
