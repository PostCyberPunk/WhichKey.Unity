using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
namespace PCP.Tools.WhichKey
{
	public class WKTestWindow : EditorWindow
	{
		private Label TestLabel;
		private float fontSize;
		public static void Test(float fontSize)
		{
			WKTestWindow win = ScriptableObject.CreateInstance<WKTestWindow>();
			win.fontSize = fontSize;
			win.minSize = new(5, 50);
			win.position = new Rect(0, 0, 5, 50);
			win.ShowPopup();
		}
		private void CreateGUI()
		{
			TestLabel = new Label("W");
			TestLabel.style.fontSize = fontSize;
			rootVisualElement.Add(TestLabel);
		}
		private void OnGUI()
		{
			if (TestLabel == null) return;
			MainHintsWindow.lineHeight = TestLabel.resolvedStyle.height;
			Close();
		}

		// private void OnLostFocus() => Close();
	}
}
