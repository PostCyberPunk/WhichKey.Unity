using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public class WhichKeyWindow : EditorWindow
	{
		//when whichkey is active disable all other key event,process key event and show hint
		[MenuItem("Tools/WhichKey/Active")]
		public static void Active()
		{
			GetWindow<WhichKeyWindow>();
		}
		private void OnEnable()
		{
			keyReleased = true;
		}
		private bool keyReleased;
		private KeyCode prevKey;
		public void OnGUI()
		{
			DummyWindow();
			Event e = Event.current;
			if (e == null) return;
			if (e.isKey)
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
					case KeyCode.Escape:
						Close(e);
						break;
					default:
						if (e.keyCode != prevKey || keyReleased)
						{
							prevKey = e.keyCode;
							keyReleased = false;
							if (WhichKey.instance.ProcessRawKey(e.keyCode, e.shift))
								Close(e);
						}
						break;
				}
			}
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
			EditorGUILayout.LabelField("WhichKey");
			EditorGUILayout.LabelField("Press ESC to close");
		}
		private void Close(Event e)
		{
			Deactive();
			e.Use();
		}
		private void Deactive() => Close();
	}

}

