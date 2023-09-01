using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	internal class WhichKeyManager : ScriptableSingleton<WhichKeyManager>
	{
		private readonly MainKeyHandler mainKeyHandler = new MainKeyHandler();
		internal readonly static UILoader mUILoader = new();
		internal static WhichKeyPreferences Preferences { private set; get; }
		private static int loggingLevel;

		public Action ShowHintWindow;
		public void Init()
		{
			if (mainKeyHandler.isInitialized)
				return;
			if (SessionState.GetBool("WhichKeyOnce", false))
				mUILoader.Refresh();
			else
			{
				SessionState.SetBool("WhichKeyOnce", true);
				EditorApplication.update += RunOnce;
			}
			SavePreferences();
			Preferences = WhichKeyPreferences.instance;
			Refresh();
		}
		private void RunOnce()
		{
			mUILoader.Refresh();
			// Debug.Log("WhichKey is running for the first time");
			EditorApplication.update -= RunOnce;
		}
		private void SavePreferences()
		{

			if (WhichKeyPreferences.instance == null)
				LogError("WhichKey Preferences instance is null");
			WhichKeyPreferences.instance.Save();
		}
		public void Active(string key)
		{
			mainKeyHandler.Reset(key);
			ShowHintWindow();
		}
		public void ShowWindow()
		{
			mainKeyHandler.Reset();
			ShowHintWindow();
		}
		public void Refresh()
		{
			loggingLevel = (int)Preferences.LogLevel;
			mainKeyHandler.Init();
			MainHintsWindow.Init();
		}
		public void ChangeRoot(string key)
		{
			mainKeyHandler.ChagneRoot(key);
		}
		public void ResetRoot()
		{
			mainKeyHandler.ResetRoot();
		}
		public void ApplyPreferences()
		{
			SavePreferences();
			Refresh();
		}
		public void LoadPreferenceFromJSON()
		{
			TextAsset jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/WhichKeyPreference.json");
			if (jsonFile == null)
			{
				LogError("WhichKey.json not found");
				return;
			}
			Preferences.KeyMap = JsonUtility.FromJson<KeyMapWrapper>(jsonFile.text).KeyMap;
		}
		public void SavePreferenceToJSON()
		{
			KeyMapWrapper keySetsWrapper = new KeyMapWrapper(Preferences.KeyMap);
			string json = JsonUtility.ToJson(keySetsWrapper, true);
			Debug.Log(json);
			System.IO.File.WriteAllText("Assets/WhichKeyPreference.json", json);
		}
		public static void LogInfo(string msg)
		{
			if (loggingLevel == 0)
				Debug.Log("Whichkey:" + msg);
		}
		public static void LogWarning(string msg)
		{
			if (loggingLevel <= 1)
				Debug.LogWarning("Whichkey:" + msg);
		}
		public static void LogError(string msg)
		{
			if (loggingLevel <= 2)
				Debug.LogError("Whichkey:" + msg);
		}
		public bool Input(KeyCode keyCode, bool shift) => mainKeyHandler.KeyHandler(keyCode, shift);
		public string[] GetHints() => mainKeyHandler.GetLayerHints();
	}
}