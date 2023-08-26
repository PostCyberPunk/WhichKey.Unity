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
		[MenuItem("Tools/WhichKey/Active")]
		public static void Active()
		{
			// var win = GetWindow<WhichKeyWindow>();
			WhichKeyWindow win = ScriptableObject.CreateInstance<WhichKeyWindow>();
			Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			win.position = new(mousePos.x+10, mousePos.y+10, 200, 100);
			win.showHint = false;
			win.UpdateDelayTimer();
			win.ShowPopup();
		}
		private void OnEnable()
		{
			keyReleased = true;
		}
		private Label label;
		private void CreateGUI()
		{
			label = new Label(hintText);
			rootVisualElement.Add(label);
		}
		public void OnGUI()
		{
			if (showHint)
				HintsWindow();
			else
				DummyWindow();
			Event e = Event.current;
			if (e == null) return;
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
								Close(e);
							else
								UpdateDelayTimer();
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
				Repaint();
			}
		}
		//This will lost focus of unity editor,need fix
		private void OnLostFocus()
		{
			Deactive();
		}
		private void DummyWindow()
		{
			minSize = Vector2.zero;
			maxSize = minSize;
		}
		private void HintsWindow()
		{
			label.text = WhichKey.instance.mLayerHint;
			minSize = new Vector2(200, 200);
			maxSize = minSize;
		}
		private void Close(Event e)
		{
			Deactive();
			e.Use();
		}
		private void Deactive() => Close();
	}

}

