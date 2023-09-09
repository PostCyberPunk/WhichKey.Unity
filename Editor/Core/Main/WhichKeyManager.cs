using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	internal class WhichKeyManager
	{
		private readonly TreeHandler mainKeyHandler = new TreeHandler();
		private WhichKeyPreferences Preferences => WhichKeyPreferences.instance;

		public static WhichKeyManager instance;
		public Action ShowHintsWindow;
		public Action CloseHintsWindow;
		public Action<float> OverrideWindowTimeout;
		public Action UpdateHints;

		#region Setup
		public WhichKeyManager()
		{
			if (instance != null)
			{
				WkLogger.LogError("Multiple WhichKeyManager instance found");
			}
			instance = this;
			instance.Init();
		}
		private void Init()
		{
			if (mainKeyHandler.isInitialized)
			{
				WkLogger.LogError("WhichKeyManager is already initialized");
				return;
			}
			if (Preferences == null)
			{
				WkLogger.LogError("WhichKey Preferences instance is null");
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
			UILoader.instance.Refresh();
			MainHintsWindow.Init();
		}
		private void RefreshDatabase()
		{
			WkLogger.loggingLevel = (int)Preferences.LogLevel;
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