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

		#region Window

		public void Active(int[] key)
		{
			//TEMP
			// mainKeyHandler.Reset(key);
			// ShowHintsWindow();
		}
		//TEMP
		public Action<float> OverrideWindowTimeout;
		public void ShowWindow()
		{
			mainKeyHandler.Reset();
			mCurrentHandler = mainKeyHandler;
			mCurrentHandler.ShowWindow();
		}
		#endregion

		public void ChangeHanlder(IWKHandler handler, int depth) => mainKeyHandler.ChangeHandler(handler, depth);

		private BaseKeyHandler mCurrentHandler;
		public void ProcesRawKey(KeyCode keyCode, bool shift)
		{
			int key = keyCode.ToAscii(shift);
			//TODO: Add support for backspace?
			if (key == 0)
				return;
			mCurrentHandler.HandleKey(key);
		}

	}
}