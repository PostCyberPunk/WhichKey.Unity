using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public class WhichKey : EditorWindow
	{
		//when whichkey is active disable all other key event,process key event and show hint
		[MenuItem("Tools/WhichKey/Active")]
		public static void Active()
		{
			var win = GetWindow<WhichKey>();
		}

		[MenuItem("Tools/WhichKey/ReloadSettings")]
		public static void LoadSetting()
		{
			TextAsset jsonFile = Resources.Load("WhichKey_setting") as TextAsset;
			if (jsonFile == null)
			{
				Debug.LogError("WhichKeySetting not found");
				return;
			}
			KeySet[] keySets = JsonUtility.FromJson<KeySet[]>(jsonFile.text);
		}
		public void OnGUI()
		{
			DummyWindow();
			Event e = Event.current;
			if (e == null) return;
			if (e.type == EventType.KeyDown)
			{
				switch (e.keyCode)
				{
					case KeyCode.Escape:
						Close(e);
						break;
					default:
						this.ShowNotification(new GUIContent("WhichKey Active"));
						ProcessKey(e.keyCode, e);
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
		private void ProcessKey(KeyCode keyCode, Event e)
		{
			Debug.Log(keyCode.ToString());
			e.Use();
		}
		private void Close(Event e)
		{
			Deactive();
			e.Use();
		}
		private void Deactive() => GetWindow<WhichKey>().Close();
	}

	[Serializable]
	public class KeySet
	{
		public string key;
		public KeyCmdType type;
		public string HintText;
		public string CmdArg0;
		public string CmdArg1;
	}
	[Serializable]
	public class Settings
	{
		public KeySet[] KeySets;
		public bool ShowHint;
		public float HintDelayText;
	}
	public enum KeyCmdType
	{
		Layer,
		Menu,
		File,
		ChangeRoot,
	}
}

