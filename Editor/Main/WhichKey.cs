using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public class WhichKey : ScriptableSingleton<WhichKey>
	{
		private readonly MainKeyHandler mainKeyHandler = new MainKeyHandler();
		private static int loggingLevel;

		public Action ShowHintWindow;
		[InitializeOnLoadMethod]
		public static void Init()
		{
			if (instance.mainKeyHandler.isInitialized)
				return;
			SaveSettings();
			Refresh();
		}

		[MenuItem("Tools/WhichKey/Active")]
		public static void ShowWindow()
		{
			instance.ShowHintWindow();
		}

		[MenuItem("WhichKey/Refresh")]
		public static void Refresh()
		{
			loggingLevel = (int)WhichKeySettings.instance.LogLevel;
			instance.mainKeyHandler.Init();
			WhichKeyWindow.Init();
		}

		[MenuItem("WhichKey/ChangeRoot")]
		public static void ChangeRoot()
		{
			FloatingTextField.ShowInputField(ChangeRoot, "Change Root To:");
		}
		public static void ChangeRoot(string key)
		{
			instance.mainKeyHandler.ChagneRoot(key);
		}

		[MenuItem("WhichKey/ResetRoot")]
		public static void ResetRoot()
		{
			instance.mainKeyHandler.ResetRoot();
		}
		public static void ApplySettings()
		{
			SaveSettings();
			Refresh();
			// SettingLogInfo("WhichKey settings applied");
		}
		public static void SaveSettings()
		{

			if (WhichKeySettings.instance == null)
				CreateInstance<WhichKeySettings>();
			WhichKeySettings.instance.Save();
		}
		public static void LoadSettingFromJSON()
		{
			TextAsset jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/WhichKey.json");
			if (jsonFile == null)
			{
				LogError("WhichKey.json not found");
				return;
			}
			WhichKeySettings.instance.keySets = JsonUtility.FromJson<KeySetsWrapper>(jsonFile.text).keySets;
		}
		public static void SaveSettingToJSON()
		{
			KeySetsWrapper keySetsWrapper = new KeySetsWrapper(WhichKeySettings.instance.keySets);
			string json = JsonUtility.ToJson(keySetsWrapper, true);
			Debug.Log(json);
			System.IO.File.WriteAllText("Assets/WhichKey.json", json);
		}
		internal static void LogInfo(string msg)
		{
			if (loggingLevel == 0)
				Debug.Log("Whichkey:" + msg);
		}
		internal static void LogWarning(string msg)
		{
			if (loggingLevel <= 1)
				Debug.LogWarning("Whichkey:" + msg);
		}
		internal static void LogError(string msg)
		{
			if (loggingLevel <= 2)
				Debug.LogError("Whichkey:" + msg);
		}
		public bool Input(KeyCode keyCode, bool shift) => mainKeyHandler.KeyHandler(keyCode, shift);
		public void Complete() => mainKeyHandler.Complete();
		public string[] GetHints() => mainKeyHandler.GetLayerHints();
	}
}