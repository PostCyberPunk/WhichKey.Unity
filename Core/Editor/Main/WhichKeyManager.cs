using UnityEditor;
using UnityEngine;
using PCP.WhichKey.Types;
using PCP.WhichKey.Log;
using PCP.WhichKey.Utils;
using PCP.WhichKey.UI;

namespace PCP.WhichKey.Core
{
	internal class WhichKeyManager
	{
		private TreeBuilder mTreeBuilder;
		private TreeHandler mainKeyHandler;
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
			mTreeBuilder = new();
			mTreeBuilder.Build();
			mainKeyHandler = new(mTreeBuilder.TreeRoot);
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

		//TEMP
		public void Active(int[] key)
		{
			// mainKeyHandler.Reset(key);
			// ShowHintsWindow();
		}

		//TEMP
		public void ShowWindow()
		{
			ChangeBaseKeyHandler(mainKeyHandler);
		}

		public void ChangeBaseKeyHandler(BaseKeyHandler handler)
		{
			mCurrentHandler = handler;
			handler.ShowWindow();
			handler.OnActive();
		}

		#endregion

		public void ChangeHanlder(IWKHandler handler, int depth) => mainKeyHandler.ChangeHandler(handler, depth);
		public void OverrideWindowTimeout(float time) => mainKeyHandler.OverrideTimeout(time);
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