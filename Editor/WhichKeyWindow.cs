using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public class WhichKeyWindow : EditorWindow
	{
		//when whichkey is active disable all other KeySeq event,process KeySeq event and show hint
		private bool keyReleased;
		private KeyCode prevKey;
		private float hideTill;
		private bool showHint;

		[MenuItem("Tools/WhichKey/Active")]
		public static void Active()
		{
			// var win = GetWindow<WhichKeyWindow>();
			WhichKeyWindow win = ScriptableObject.CreateInstance<WhichKeyWindow>();
			minSize = new Vector2(1, 1);
			win.showHint = false;
			win.UpdateDelayTimer();
			win.ShowPopup();
		}
		private void OnEnable()
		{
			keyReleased = true;
		}
		public void OnGUI()
		{
			// CheckDelayTiemr();
			// if (showHint)
			// 	HintsWindow();
			// else
			// DummyWindow();
			HintsWindow();
			Repaint();
			Event e = Event.current;
			if (e == null) return;
			if (!e.isKey)
				return;
			KeyHandler(e);
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
		private void CheckDelayTiemr()
		{
			if (showHint) return;
			showHint = Time.realtimeSinceStartup > hideTill;
		}
		//This will lost focus of unity editor,need fix
		private void OnLostFocus()
		{
			Deactive();
		}
		private void DummyWindow()
		{
			minSize = new Vector2(1, 1);
			maxSize = minSize;
			// EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent("WhichKey Active"));
		}
		private void HintsWindow()
		{
			string text = WhichKey.instance.mLayerHint;
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.richText = true;
			style.fontSize =30;

			GUIContent content = new GUIContent(text);
			float height = style.CalcHeight(content, position.width);

			EditorGUILayout.LabelField(text, style, GUILayout.Height(height));
		}
		private void Close(Event e)
		{
			Deactive();
			e.Use();
		}
		private void Deactive() => Close();
	}

}

