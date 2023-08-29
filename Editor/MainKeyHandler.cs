using System.Text;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	internal class MainKeyHandler : IWhichKeyHandler
	{
		private StringBuilder mKeySeq;
		private KeyNode mRoot;
		private KeyNode mCurrentNode;
		private StringBuilder sb;
		private IWhichKeyHandler mCurrentHandler;
		public bool isInitialized { get => mRoot != null; }

		public void Init()
		{
			mKeySeq = new();
			sb = new();

			mRoot = new KeyNode("", "");
			foreach (var keySet in WhichKeySettings.instance.keySets)
			{
				AddKeySetToTree(keySet);
			}
			KeyNode.maxLine = WhichKeySettings.instance.MaxHintLines;
			mRoot.SetLayerHints();
			Complete();
		}

		private void AddKeySetToTree(KeySet keyset)
		{
			mCurrentNode = mRoot;
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
				if (key.StartsWith("Escape"))
					return true;
				if (key.StartsWith("Alpha"))
				{
					key = key.Replace("Alpha", "");
				}
				else if (key.StartsWith("Keypad"))
				{
					key = key.Replace("Keypad", "");
				}
				else if (key.StartsWith("Space"))
				{
					key = " ";
				}
				else
				{
					WhichKey.LogInfo($"Key {key} not supported");
					return true;
				}
			}
			if (key.Length > 1)
			{
				WhichKey.LogError($"Unsupported key found :{key}");
				return true;
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
			ResetRoot();
		}

		private void ResetRoot()
		{
			mCurrentNode = mRoot;
		}

		private void DebugShowHints()
		{
			// Debug.Log($"{item.KeySeq}:{item.Value}");
		}
	}
}