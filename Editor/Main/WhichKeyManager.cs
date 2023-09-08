using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	internal class WhichKeyManager : ScriptableSingleton<WhichKeyManager>
	{
		private readonly TreeHandler mainKeyHandler = new TreeHandler();
		internal readonly static UILoader mUILoader = new();
		internal static WhichKeyPreferences Preferences { private set; get; }
		private static int loggingLevel;

		public Action ShowHintsWindow;
		public Action CloseHintsWindow;
		public Action<float> OverrideWindowTimeout;
		public Action UpdateHints;
		#region Setup
		public void Init()
		{
			if (mainKeyHandler.isInitialized)
			{
				LogError("WhichKeyManager is already initialized");
				return;
			}
			// WhichkeyProjectSettings.instance?.Init();
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
		public void Refresh()
		{
			RefreshUI();
			RefreshDatabase();
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
		#endregion

		#region Tree

		public void ChangeRoot(int[] key)
		{
			mainKeyHandler.ChagneRoot(key);
		}
		public void ResetRoot()
		{
			mainKeyHandler.ResetRoot();
		}
		#endregion

		#region Logger

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
		#endregion

		#region Settings
		private void SavePreferences()
		{

			if (WhichKeyPreferences.instance != null)
				WhichKeyPreferences.instance.Save();
			else
				LogError("WhichKey Preferences instance is null");
		}
		public void ApplyPreferences()
		{
			SavePreferences();
			Refresh();
		}
		[System.Serializable]
		private class OldKeySet
		{
			public string KeySeq;
			public KeyCmdType type;
			public string HintText;
			public string CmdArg;
		}
		private OldKeySet[] oldKeySets = new OldKeySet[0];
		[MenuItem("WhichKeyDev/UpdateJson")]
		public static void UpdateJson()
		{
			TextAsset jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/WhichKeyPreference-bak.json");
			instance.oldKeySets = JsonUtility.FromJson<JSONArrayWrapper<OldKeySet>>(jsonFile.text).array;
			Preferences.KeyMap = new KeySet[instance.oldKeySets.Length];
			for (int i = 0; i < instance.oldKeySets.Length; i++)
			{
				OldKeySet oldKeySet = instance.oldKeySets[i];
				int[] list = new int[oldKeySet.KeySeq.Length];
				for (int i1 = 0; i1 < oldKeySet.KeySeq.Length; i1++)
				{
					char cha = oldKeySet.KeySeq[i1];
					try
					{
						list[i1] = (int)cha;
					}
					catch (System.Exception)
					{
						Debug.Log("error");
						continue;
					}
				}
				Preferences.KeyMap[i]= new KeySet();
				Preferences.KeyMap[i].KeySeq = list;
				Preferences.KeyMap[i].CmdType = (int)oldKeySet.type;
				Preferences.KeyMap[i].Hint = oldKeySet.HintText;
				Preferences.KeyMap[i].CmdArg = oldKeySet.CmdArg;
				instance.Save(true);
			}
			Debug.Log("done");
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
		#endregion

		#region Methods

		public void Active(int[] key)
		{
			mainKeyHandler.Reset(key);
			ShowHintsWindow();
		}
		public void ShowWindow()
		{
			ShowHintsWindow();
			mainKeyHandler.Reset();
		}
		#endregion

		public void Input(KeyCode keyCode, bool shift) => mainKeyHandler.ProcesRawKey(keyCode, shift);
		public string[] GetHints() => mainKeyHandler.GetLayerHints();
		public void ChangeHanlder(IWKHandler handler,int depth) => mainKeyHandler.ChangeHandler(handler,depth);
	}
}