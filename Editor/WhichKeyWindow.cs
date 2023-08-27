using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PCP.Tools.WhichKey
{
	public class WhichKeyWindow : EditorWindow
	{
		//when whichkey is active disable all other KeySeq event,process KeySeq event and show hint
		private bool keyReleased;
		private KeyCode prevKey;
		private float hideTill;
		private bool showHint;
		private string hintText;
		private float mHeight;
		private float mWidth;
		private float lineHeight;
		#region Elements
		private VisualElement mainFrame;
		private Label TestLabel;
		#endregion
		[MenuItem("Tools/WhichKey/Active")]
		public static void Active()
		{
			// var win = GetWindow<WhichKeyWindow>();
			WhichKeyWindow win = ScriptableObject.CreateInstance<WhichKeyWindow>();
			win.showHint = false;
			win.UpdateDelayTimer();
			win.ShowPopup();
			win._changeUI = true;
		}
		private void CreateGUI()
		{
			mainFrame = new VisualElement();
			TestLabel = new Label("<size=20>WhichKey</size>");
			mainFrame.Add(TestLabel);
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
				hideTill = Time.realtimeSinceStartup + WhichKey.instance.HintDelayTime;
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
		private void CalculateLineHeight()
		{
			position = new Rect(0, 0, 1000,1000);
			minSize = new Vector2(1000, 1000);
			maxSize = new Vector2(1000, 1000);
			mainFrame.Clear();
			mainFrame.Add(TestLabel);
			lineHeight = TestLabel.resolvedStyle.height;
			TestLabel.resolvedStyle.
			Debug.Log(lineHeight);
		}
		private void DummyWindow()
		{
			mainFrame.Clear();
			position = new Rect(0, 0, 1, 1);
		}
		private string DebugGetHints()
		{
			string s = "<size=20>";
			for (int i = 10; i > 0; i--)
			{
				s += (i.ToString() + "\n");
			}
			s += "</size>";
			return s;
		}
		private void HintsWindow()
		{
			CalculateLineHeight();
			mainFrame.Clear();
			mHeight = lineHeight * 10+6;
			mWidth = 200;
			maxSize=new Vector2(mWidth,mHeight);
			Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			position = new Rect(mousePos.x, mousePos.y, mWidth, mHeight);
			mainFrame.Add(new Label(DebugGetHints()));
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

