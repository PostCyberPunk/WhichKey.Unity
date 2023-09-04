using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace PCP.Tools.WhichKey
{
	public class MainHintsWindow : EditorWindow
	{
		//when whichkey is active disable all other KeySeq event,process KeySeq event and show hint
		// public static WhichKeyWindow inistance;
		public static MainHintsWindow instance;
		private bool keyReleased;
		private KeyCode prevKey;
		private float hideTill;
		private bool showHint;
		private float mHeight;
		private float mWidth;
		#region Elements
		private VisualElement mainFrame;
		private VisualElement labelFrame;
		private Label titleLabel;
		#endregion
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
		public static void Active()
		{
			// var win = GetWindow<WhichKeyWindow>();
			if (instance == null)
			{
				instance = ScriptableObject.CreateInstance<MainHintsWindow>();
			}
			// if (lineHeight == 0)
				// WKTestWindow.Test(mFontSize);
				// lineHeight = 24;
			instance.showHint = false;
			instance.titleContent = new GUIContent("WhichKey");
			instance.UpdateDelayTimer();
			//?
			instance.ShowPopup();
			instance.minSize = new(0, 0);
			instance.position = new Rect(0, 0, 0, 0);
		}

		internal static void Init()
		{
			// Setup Settings
			if (WhichKeyPreferences.instance == null)
			{
				WhichKeyManager.LogError("WhichKey Setting instance is null");
				return;
			}
			WhichKeyManager.instance.ShowHintWindow = Active;

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
			//Calculate line height
			// WKTestWindow.Test(mFontSize);

		}
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
		private void OnEnable()
		{
			keyReleased = true;
		}
		public void OnGUI()
		{
			Event e = Event.current;
			if (e == null) return;
			CheckUI();
			if (!e.isKey)
				return;
			KeyHandler(e);
		}
		private void Update()
		{
			CheckDelayTimer();
		}
		private void KeyHandler(Event e)
		{
			if (e.type == EventType.KeyUp)
			{
				keyReleased = true;
				return;
			}
			if (e.type == EventType.KeyDown)
			{
				switch (e.keyCode)
				{
					case KeyCode.None:
						break;
					case KeyCode.Escape:
						Close(e);
						break;
					case KeyCode.LeftShift:
					case KeyCode.RightShift:
						break;
					default:
						if (e.keyCode != prevKey || keyReleased)
						{
							prevKey = e.keyCode;
							keyReleased = false;
							if (WhichKeyManager.instance.Input(e.keyCode, e.shift))
							{
								Close(e);
							}
							else
							{
								_changeUI = true;
								UpdateDelayTimer();
							}
						}
						break;
				}
			}
		}
		private void UpdateDelayTimer()
		{
			if (!showHint)
				hideTill = Time.realtimeSinceStartup + timeoutLen;
		}
		private void CheckDelayTimer()
		{
			if (showHint) return;
			showHint = Time.realtimeSinceStartup > hideTill;
			if (showHint)
			{
				//OPT cant get mouse position here,need to find a way to get mouse position
				_changeUI = true;
				Repaint();
			}
		}
		//This will lost focus of unity editor,need fix
		private void OnLostFocus() => Deactive();

		private void DummyWindow()
		{
			// labelFrame.Clear();
			position = new Rect(0, 0, 0, 0);
		}
		private string DebugGetHints()
		{
			string s = string.Empty;
			// string s = "<size=20>";
			for (int i = 10; i > 0; i--)
			{
				s += (i.ToString() + "\n");
			}
			// s += "</size>";
			return s;
		}
		private void HintsWindow()
		{
			string[] hints = WhichKeyManager.instance.GetHints();
			if (hints == null)
			{
				Deactive();
				return;
			}
			labelFrame.Clear();
			mHeight = lineHeight * (maxHintLines + 1) + 2*mainFrame.resolvedStyle.paddingTop;
			var cols = Mathf.CeilToInt(hints.Length / 2f / maxHintLines);
			mWidth = cols * maxColWidth + mainFrame.resolvedStyle.paddingLeft*2;
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
		private void CachedHintsWindow()
		{
			string[] hints = WhichKeyManager.instance.GetHints();
			if (hints == null)
			{
				Deactive();
				return;
			}
			mainFrame.Clear();
			mHeight = lineHeight * maxHintLines;
			mainFrame.style.flexDirection = FlexDirection.Row;
			mWidth = hints.Length * maxColWidth;
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

			for (int i = 0; i < hints.Length; i++)
			{
				Label label = new Label(hints[i]);
				// label.style.fontSize = mFontSize;
				label.style.width = maxColWidth;
				mainFrame.Add(label);
			}
		}
		private bool _changeUI;
		private void CheckUI()
		{
			if (_changeUI)
			{
				_changeUI = false;
				if (showHint)
					HintsWindow();
				else
					DummyWindow();
			}
		}

		private void Close(Event e)
		{
			Deactive();
			e.Use();
		}
		private void Deactive()
		{
			Close();
		}
		public void ForceActive()
		{
			if (instance == null)
			{
				Debug.LogWarning("Found no WhichKeyWindow instance");
				return;
			}
			instance.showHint = true;
		}
	}

}

