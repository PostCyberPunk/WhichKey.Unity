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
        private void Awake()
        {
            Debug.Log("Awake");
        }
        private void OnEnable()
        {
            Debug.Log("OnEnable");
        }
		internal static void Test(float fontSize)
		{
			WKTestWindow win = ScriptableObject.CreateInstance<WKTestWindow>();
			win.fontSize = fontSize;
			win.position = new Rect(0, 0, 5, 50);
			win.ShowPopup();
		}
        private void OnGUI()
        {
            Debug.Log("OnGUI");
            Debug.Log(TestLabel==null);
        }
		private void CreateGUI()
		{
			TestLabel = new Label("W");
			TestLabel.style.fontSize = fontSize;
			rootVisualElement.Add(TestLabel);
            Debug.Log("CreateGUI");
		}

		private void OnBecameVisible()
		{
			// WhichKeyWindow.lineHeight = TestLabel.resolvedStyle.height;
			Debug.Log("OnBecameVisible");
			// Close();
		}
		private void OnLostFocus() => Close();
	}
}
