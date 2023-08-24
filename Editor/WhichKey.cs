using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public static class WhichKey
	{
		public static Settings mSettings { private set; get; }
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
			TextAsset jsonFile = Resources.Load("WhichKey_setting") as TextAsset;
			if (jsonFile == null)
			{
				Debug.LogError("WhichKeySetting not found");
				return;
			}
			mSettings = JsonUtility.FromJson<Settings>(jsonFile.text);
		}
	}
}
