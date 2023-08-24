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
            GetWindow<WhichKey>();
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
						ProcessKey(e.keyCode, e);
						break;
				}
			}
		}
		private void ProcessKey(KeyCode keyCode, Event e)
		{
			Debug.Log(keyCode.ToString());
			e.Use();
		}
		private void Close(Event e)
		{
            Debug.Log("Quit");
			Deactive();
			e.Use();
		}
		private void Deactive() =>GetWindow<WhichKey>().Close();
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

	public enum KeyCmdType
	{
		Layer,
		Menu,
		File,
		ChangeRoot,
	}
}

