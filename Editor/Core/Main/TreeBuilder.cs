using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Net.NetworkInformation;
namespace PCP.Tools.WhichKey
{
	internal class TreeBuilder
	{
		public bool isInitialized { get => mTreeRoot != null; }
		public KeyNode TreeRoot { get => mTreeRoot; }
		private KeyNode mTreeRoot;
		private KeyNode mCurrentNode;
		private readonly CmdFactoryManager mCmdFactoryManager = new CmdFactoryManager();
		public void Build()
		{
			mTreeRoot = new KeyNode(0, "WhichKey");
			
			AddKeySetFromMap(WhichKeyPreferences.instance.KeyMap);
			KeyNode.maxLine = WhichKeyManager.Preferences.MaxHintLines;
			
			mTreeRoot.SetLayerHints();
		}
		private void AddKeySetFromMap(KeySet[] keymap)
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
				int key = keyset.KeySeq[i];
				KeyNode kn = mCurrentNode.GetChildByKey(key);
				if (i == keyset.KeySeq.Length - 1)
				{
					if (kn == null)
					{
						WKCommand cmd = mCmdFactoryManager.CreateCommand(keyset.CmdType, keyset.CmdArg);
						if (cmd == null)
						{
							WhichKeyManager.LogError($"<color=yellow>Hint: {keyset.Hint}</color>||<color=green>Key:{keyset.KeySeq.ToLabel()}</color>||<color=red>Command:{keyset.CmdType}</color> has no valid command type,skipped ");
							return;
						}
						kn = mCurrentNode.AddChild(new KeyNode(keyset, i, mCurrentNode, cmd));
					}
					else
					{
						if (kn.isLayer && string.IsNullOrEmpty(kn.Hint) && keyset.IsLayer)
						{
							kn.UpdateKeySet(keyset);
						}
						else
							WhichKeyManager.LogError($"<color=yellow>Hint: {keyset.Hint}</color>||<color=green>Key:{keyset.KeySeq.ToLabel()}</color> already registered To \"<color=yellow>{kn.Hint}</color>\" ,skipped ");
					}
					return;
				}
				if (kn == null)
				{
					kn = mCurrentNode.AddChild(new KeyNode(key, ""));
				}
				mCurrentNode = kn;
			}
		}
		


	}

}
