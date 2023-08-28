using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace PCP.Tools.WhichKey
{
	public class WhichKeyWindow : EditorWindow
	{
		//when whichkey is active disable all other KeySeq event,process KeySeq event and show hint
		public static WhichKeyWindow inistance;
		private bool keyReleased;
		private KeyCode prevKey;
		private float hideTill;
		private bool showHint;
		private float mHeight;
		private float mWidth;
		private float lineHeight;
		#region Elements
		private VisualElement mainFrame;
		private Label TestLabel;
		private float mFontSize = 12;
		#endregion
		#region Data
		//OPT
		//Maybe a structure
		private float hintDelayTime;
		private bool followMouse;
		private Vector2 fixedPosition;
		private int maxHintLines;
		private float maxColWidth;
		private bool showKeyHint;
		#endregion
		[MenuItem("Tools/WhichKey/Active")]
		public static void Active()
		{
			// var win = GetWindow<WhichKeyWindow>();
			WhichKeyWindow win = ScriptableObject.CreateInstance<WhichKeyWindow>();
			win.showHint = false;
			win.titleContent = new GUIContent("WhichKey");
			win.UpdateDelayTimer();
			win.Init();
			//?
			win.ShowPopup();
			win.maxSize = new Vector2(5, 50);
		}
		private void Init()
		{
			if (WhichKey.instance == null)
			{
				WhichKey.LogError("WhichKey instance is null");
				return;
			}
			var settings = WhichKey.instance;
			showKeyHint = settings.ShowHint;
			followMouse = settings.WindowFollowMouse;
			fixedPosition = settings.FixedPosition;
			maxHintLines = settings.MaxHintLines;
			maxColWidth = settings.MaxColWidth;
			hintDelayTime = settings.HintDelayTime;
		}
		private void CreateGUI()
		{
			mainFrame = new VisualElement();
			TestLabel = new Label("a");
			mainFrame.Add(TestLabel);
			TestLabel.style.fontSize = mFontSize;
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
					default:
						if (e.keyCode != prevKey || keyReleased)
						{
							prevKey = e.keyCode;
							keyReleased = false;
							if (WhichKey.instance.KeyHandler(e.keyCode, e.shift))
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
				hideTill = Time.realtimeSinceStartup + hintDelayTime;
		}
		private void CheckDelayTimer()
		{
			if(!showKeyHint) return;
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
		private void CalculateLineHeight()
		{
			mainFrame.Clear();
			// position = new Rect(0, 0, 1000, 1000);
			// mainFrame.style.width = 200;
			// mainFrame.style.height = 200;
			mainFrame.Add(TestLabel);
			// TestLabel.style.fontSize = 20;
			lineHeight = TestLabel.resolvedStyle.height;
		}
		private void DummyWindow()
		{
			mainFrame.Clear();
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
			CalculateLineHeight();
			string[] hints = WhichKey.instance.mLayerHint;
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
				label.style.fontSize = mFontSize;
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
		private void Deactive() => Close();
	}

}

