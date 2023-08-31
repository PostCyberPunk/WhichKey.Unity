using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

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
		public bool isInitialized { get => mTreeRoot != null; }

		private static readonly Dictionary<KeyCode, string> mKeyMap = new Dictionary<KeyCode, string>
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
			foreach (var keySet in WhichKeyPreferences.instance.keySets)
			{
				AddKeySetToTree(keySet);
			}
			KeyNode.maxLine = WhichKeyPreferences.instance.MaxHintLines;
			mTreeRoot.SetLayerHints();

			ResetRoot();
			Complete();
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
						childNode = mCurrentNode.AddChild(new KeyNode(keyset));
					else
					{
						if (childNode.Type == KeyCmdType.Layer && keyset.type == KeyCmdType.Layer)
							childNode.UpdateKeySet(keyset);
						else
							WhichKey.LogError($"KeySeq {keyset.KeySeq} already registered,skip Hint: {keyset.HintText},args: {keyset.CmdArg}");
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
			if (ProcessRawKey(keyCode, shift))
			{
				Complete();
				return true;
			}
			else
				return false;
		}

		public bool ProcessRawKey(KeyCode keyCode, bool shift)
		{
			//Oh bad bad bad code

			string key = keyCode.ToString();
			if (key.Length > 1)
			{
				if (!mKeyMap.TryGetValue(keyCode, out key))
				{
					WhichKey.LogInfo($"Key {key} not supported");
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
				WhichKey.LogWarning($"KeySeq {mKeySeq} not found");
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
			if (!EditorApplication.ExecuteMenuItem(menuName)) WhichKey.LogWarning($"Menu {menuName} not available");
			Complete();
		}

		public string[] GetLayerHints()
		{
			return mCurrentNode.LayerHints;
		}
		public void Complete()
		{
			mKeySeq.Clear();
			mCurrentHandler = this;
			mCurrentNode = mRoot;
		}

		public void ResetRoot()
		{
			mRoot = mTreeRoot;
			Complete();
		}

		public void ChagneRoot(string key)
		{
			if (key.Length == 0)
			{
				ResetRoot();
				return;
			}
			KeyNode kn = mTreeRoot;

			for (int i = 0; i < key.Length; i++)
			{
				kn = mCurrentNode.GetChildByKey(key[i]);
				if (kn == null)
				{
					WhichKey.LogWarning($"KeySeq {mKeySeq} not found @key {key[i]}");
					return;
				}
			}
			if (kn.Type != KeyCmdType.Layer)
			{
				WhichKey.LogWarning($"KeySeq {mKeySeq} not a layer");
				return;
			}
			mRoot = kn;
			Complete();
			WhichKey.LogInfo($"Change root to {key}");
		}
		private void DebugShowHints()
		{
			// Debug.Log($"{item.KeySeq}:{item.Value}");
		}
	}
}