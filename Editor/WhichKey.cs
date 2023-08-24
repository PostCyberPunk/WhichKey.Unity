using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public static class WhichKey
	{
		public static WhichKeySettings mSettings { private set; get; }
		[InitializeOnLoadMethod]
		private static void Init()
		{
			// if(_keySetting==null)
		}
		public static void ProcessKey(KeyCode keyCode, Event e)
		{
			Debug.Log(keyCode.ToString());
			e.Use();
		}
		[MenuItem("Tools/WhichKey/ReloadSettings")]
		public static void LoadSetting()
		{
		}
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
