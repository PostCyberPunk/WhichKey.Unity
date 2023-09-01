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
		internal static UILoader mUILoader = new();
		internal static WhichKeyPreferences Preferences { private set; get; }
		private static int loggingLevel;

		public Action ShowHintWindow;
		[InitializeOnLoadMethod]
		public static void Init()
		{
			if (instance.mainKeyHandler.isInitialized)
				return;
			if (SessionState.GetBool("WhichKeyOnce", false))
				mUILoader.Refresh();
			else
			{
				SessionState.SetBool("WhichKeyOnce", true);
				EditorApplication.update += instance.RunOnce;
			}
			SavePreferences();
			Preferences = WhichKeyPreferences.instance;
			Refresh();
		}
		private void RunOnce()
		{
			mUILoader.Refresh();
			// Debug.Log("WhichKey is running for the first time");
			EditorApplication.update -= instance.RunOnce;
		}
		public static void Active(string key)
		{
			instance.mainKeyHandler.Reset(key);
			instance.ShowHintWindow();
		}

		[MenuItem("WhichKey/Active")]
		public static void ShowWindow()
		{
			instance.mainKeyHandler.Reset();
			instance.ShowHintWindow();
		}

		[MenuItem("WhichKey/Refresh")]
		public static void Refresh()
		{
			loggingLevel = (int)Preferences.LogLevel;
			instance.mainKeyHandler.Init();
			MainHintsWindow.Init();
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
		public static void ApplyPreferences()
		{
			SavePreferences();
			Refresh();
		}
		public static void SavePreferences()
		{

			if (WhichKeyPreferences.instance == null)
				LogError("WhichKey Preferences instance is null");
			WhichKeyPreferences.instance.Save();
		}
		public static void LoadPreferenceFromJSON()
		{
			TextAsset jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/WhichKeyPreference.json");
			if (jsonFile == null)
			{
				LogError("WhichKey.json not found");
				return;
			}
			Preferences.KeyMap = JsonUtility.FromJson<KeyMapWrapper>(jsonFile.text).KeyMap;
		}
		public static void SavePreferenceToJSON()
		{
			KeyMapWrapper keySetsWrapper = new KeyMapWrapper(Preferences.KeyMap);
			string json = JsonUtility.ToJson(keySetsWrapper, true);
			Debug.Log(json);
			System.IO.File.WriteAllText("Assets/WhichKeyPreference.json", json);
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
		public string[] GetHints() => mainKeyHandler.GetLayerHints();
	}
}