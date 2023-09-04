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
			{
				LogError("WhichKeyManager is already initialized");
				return;
			}
			WhichkeyProjectSettings.instance?.Init();
			SavePreferences();
			Preferences = WhichKeyPreferences.instance;
			if (SessionState.GetBool("WhichKeyOnce", false))
				RefreshUI();
			else
			{
				SessionState.SetBool("WhichKeyOnce", true);
				EditorApplication.update += RunOnce;
			}
			RefreshDatabase();
		}
		private void RunOnce()
		{
			RefreshUI();
			// Debug.Log("WhichKey is running for the first time");
			EditorApplication.update -= RunOnce;
		}
		private void SavePreferences()
		{

			if (WhichKeyPreferences.instance != null)
				WhichKeyPreferences.instance.Save();
			else
				LogError("WhichKey Preferences instance is null");
		}
		private void RefreshUI()
		{
			mUILoader.Refresh();
			MainHintsWindow.Init();
		}
		private void RefreshDatabase()
		{
			loggingLevel = (int)Preferences.LogLevel;
			mainKeyHandler.Init();
		}
		public void Active(int[] key)
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
			RefreshUI();
			RefreshDatabase();
		}
		public void ChangeRoot(int[] key)
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
			Preferences.KeyMap = JsonUtility.FromJson<JSONArrayWrapper<KeySet>>(jsonFile.text).array;
		}
		public void SavePreferenceToJSON()
		{
			JSONArrayWrapper<KeySet> keySetsWrapper = new JSONArrayWrapper<KeySet>(Preferences.KeyMap);
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