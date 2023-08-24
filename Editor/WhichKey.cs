using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public static class WhichKey
	{
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
		[MenuItem("Tools/WhichKey/LoadSettingFromJSON")]
		public static void LoadSettingFromJSON()
		{
			TextAsset jsonFile =  AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/WhichKeySettings.json");
			if(jsonFile==null)
			{
				Debug.LogError("WhichKeySettings.json not found");
				return;
			}
			WhichKeySettings.instance.keySets = JsonUtility.FromJson<WhichKeySettings>(jsonFile.text).keySets;
		}
		[MenuItem("Tools/WhichKey/SaveSettingToJSON")]
		public static void SaveSettingToJSON()
		{
			string json = JsonUtility.ToJson(WhichKeySettings.instance);
			System.IO.File.WriteAllText("Assets/WhichKeySettings.json", json);
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
