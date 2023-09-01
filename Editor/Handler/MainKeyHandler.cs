using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Schema;

namespace PCP.Tools.WhichKey
{
	internal class MainKeyHandler : IWhichKeyHandler
	{
		private StringBuilder mKeySeq;
		private KeyNode mTreeRoot;
		private KeyNode mRoot;
		private KeyNode mCurrentNode;
		private StringBuilder sb;
		private IWhichKeyHandler mCurrentHandler;
		private AssetsHandler mAssetsHandler = new();
		public bool isInitialized { get => mTreeRoot != null; }

		private static readonly Dictionary<KeyCode, string> mKeycodeMap = new Dictionary<KeyCode, string>
		{
		{ KeyCode.Space, " " },
		{ KeyCode.Alpha0, "0" },
		{ KeyCode.Alpha1, "1" },
		{ KeyCode.Alpha2, "2" },
		{ KeyCode.Alpha3, "3" },
		{ KeyCode.Alpha4, "4" },
		{ KeyCode.Alpha5, "5" },
		{ KeyCode.Alpha6, "6" },
		{ KeyCode.Alpha7, "7" },
		{ KeyCode.Alpha8, "8" },
		{ KeyCode.Alpha9, "9" },
		{ KeyCode.Keypad0, "0" },
		{ KeyCode.Keypad1, "1" },
		{ KeyCode.Keypad2, "2" },
		{ KeyCode.Keypad3, "3" },
		{ KeyCode.Keypad4, "4" },
		{ KeyCode.Keypad5, "5" },
		{ KeyCode.Keypad6, "6" },
		{ KeyCode.Keypad7, "7" },
		{ KeyCode.Keypad8, "8" },
		{ KeyCode.Keypad9, "9" },
		};
		public void Init()
		{
			mKeySeq = new();
			sb = new();

			mTreeRoot = new KeyNode("", "");

			ProcessKeyMap(WhichKeyPreferences.instance.KeyMap);

			KeyNode.maxLine = WhichKeyManager.Preferences.MaxHintLines;
			mTreeRoot.SetLayerHints();

			ResetRoot();
			Reset();
		}

		private void ProcessKeyMap(KeySet[] keymap)
		{
			for (int i = 0; i < keymap.Length; i++)
			{
				KeySet keySet = keymap[i];
				AddKeySetToTree(keySet);
			}
		}
		private void AddKeySetToTree(KeySet keyset)
		{
			mCurrentNode = mTreeRoot;
			for (int i = 0; i < keyset.KeySeq.Length; i++)
			{
				char key = keyset.KeySeq[i];
				KeyNode childNode = mCurrentNode.GetChildByKey(key);
				if (i == keyset.KeySeq.Length - 1)
				{
					if (childNode == null)
						childNode = mCurrentNode.AddChild(new KeyNode(keyset, mCurrentNode));
					else
					{
						if (childNode.Type == KeyCmdType.Layer && keyset.type == KeyCmdType.Layer)
							childNode.UpdateKeySet(keyset);
						else
							WhichKeyManager.LogError($"KeySeq {keyset.KeySeq} already registered,skip Hint: {keyset.HintText},args: {keyset.CmdArg}");
					}
					return;
				}
				if (childNode == null)
				{
					childNode = mCurrentNode.AddChild(new KeyNode(key.ToString(), ""));
				}
				mCurrentNode = childNode;
			}
		}

		public bool KeyHandler(KeyCode keyCode, bool shift)
		{
			return ProcessRawKey(keyCode, shift);
		}

		public bool ProcessRawKey(KeyCode keyCode, bool shift)
		{
			//Oh bad bad bad code

			string key = keyCode.ToString();
			if (key.Length > 1)
			{
				if (!mKeycodeMap.TryGetValue(keyCode, out key))
				{
					WhichKeyManager.LogInfo($"Key {key} not supported");
					return true;
				}
			}
			return mCurrentHandler.ProcessKey(shift ? key[0] : key.ToLower()[0]);
		}

		public bool ProcessKey(char key)
		{
			mKeySeq.Append(key);
			KeyNode kn = mCurrentNode.GetChildByKey(key);
			if (kn == null)
			{
				WhichKeyManager.LogWarning($"KeySeq {mKeySeq} not found");
				return true;
			}
			switch (kn.Type)
			{
				case KeyCmdType.Layer:
					ProcessLayer(kn);
					return false;
				case KeyCmdType.Menu:
					ProcessMenu(kn.CmdArg);
					return true;
				case KeyCmdType.ChangeRoot:
					ChagneRoot(kn.CmdArg);
					return true;
				case KeyCmdType.Assets:
					int index = 0;
					try
					{
						index = int.Parse(kn.CmdArg);
					}
					catch (System.Exception)
					{
						WhichKeyManager.LogError($"AssetsHandler: Invalid Index {kn.CmdArg},Check your project settings for AssetsData");
						return true;
					}
					if (!mAssetsHandler.ProecessArg(index)) return true;
					mCurrentHandler = mAssetsHandler;
					return false;
				default:
					return true;
			}
		}

		private void ProcessLayer(KeyNode kn)
		{
			mCurrentNode = kn;
		}

		private void ProcessMenu(string menuName)
		{
			if (!EditorApplication.ExecuteMenuItem(menuName)) WhichKeyManager.LogWarning($"Menu {menuName} not available");
		}

		public string[] GetLayerHints()
		{
			if (mCurrentHandler != this && mCurrentHandler != null)
				return mCurrentHandler.GetLayerHints();
			return mCurrentNode.LayerHints;
		}
		public void Reset()
		{
			mKeySeq.Clear();
			mCurrentHandler = this;
			mCurrentNode = mRoot;
		}
		public void Reset(string key)
		{
			Reset();
			mCurrentNode = GetKeyNodebyKeySeq(key);
		}

		public void ResetRoot()
		{
			mRoot = mTreeRoot;
		}

		public void ChagneRoot(string key)
		{
			var kn = GetKeyNodebyKeySeq(key);
			if (kn == null) return;
			mRoot = kn;
			WhichKeyManager.LogInfo($"Change root to {key}");
		}
		private KeyNode GetKeyNodebyKeySeq(string key)
		{

			if (key.Length == 0)
			{
				ResetRoot();
				return null;
			}
			KeyNode kn = mTreeRoot;

			for (int i = 0; i < key.Length; i++)
			{
				kn = mCurrentNode.GetChildByKey(key[i]);
				if (kn == null)
				{
					WhichKeyManager.LogWarning($"KeySeq {mKeySeq} not found @key {key[i]}");
					return null;
				}
			}
			if (kn.Type != KeyCmdType.Layer)
			{
				WhichKeyManager.LogWarning($"KeySeq {mKeySeq} not a layer");
				return null;
			}
			return kn;
		}
		private void DebugShowHints()
		{
			// Debug.Log($"{item.KeySeq}:{item.Value}");
		}
	}
}