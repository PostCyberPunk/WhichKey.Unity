using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	internal class WhichKeyManager : ScriptableSingleton<WhichKeyManager>
	{
		internal readonly static UILoader mUILoader = new();
		private readonly TreeHandler mainKeyHandler = new TreeHandler();
		private WhichKeyPreferences Preferences => WhichKeyPreferences.instance;
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
			if (Preferences == null)
			{
				LogError("WhichKey Preferences instance is null");
				return;
			}
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
		public void ChangeHanlder(IWKHandler handler, int depth) => mainKeyHandler.ChangeHandler(handler, depth);
	}
}