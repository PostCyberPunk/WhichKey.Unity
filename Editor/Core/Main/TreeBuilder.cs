using PCP.WhichKey.Types;
namespace PCP.WhichKey.Core
{
    internal class TreeBuilder
	{
		public bool isInitialized { get => mTreeRoot != null; }
		public KeyNode TreeRoot { get => mTreeRoot; }
		private KeyNode mTreeRoot;
		private KeyNode mCurrentNode;
		private CmdFactoryManager mCmdFactoryManager;
		private WhichKeyPreferences Preferences { get => WhichKeyPreferences.instance; }
		private WhichkeyProjectSettings ProjectSettings { get => WhichkeyProjectSettings.instance; }
		public void Build()
		{
			mCmdFactoryManager = new();
			
			mTreeRoot = new KeyNode(0, "WhichKey");

			AddKeySetFromMap(Preferences.LayerMap);
			AddKeySetFromMap(Preferences.MenuMap);
			AddKeySetFromMap(Preferences.KeyMap);


			AddKeySetFromMap(ProjectSettings.LayerMap);
			AddKeySetFromMap(ProjectSettings.MenuMap);
			AddKeySetFromMap(ProjectSettings.KeyMap);

			KeyNode.maxLine = Preferences.MaxHintLines;

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
			for (int i = 0; i < keyset.KeySeq.KeySeq.Length; i++)
			{
				int key = keyset.KeySeq.KeySeq[i];
				KeyNode kn = mCurrentNode.GetChildByKey(key);
				if (i == keyset.KeySeq.KeySeq.Length - 1)
				{
					if (kn == null)
					{
						if (keyset.CmdType == 0)
						{
							mCurrentNode.AddChild(new KeyNode(key, keyset.Hint));
							return;
						}
						WKCommand cmd = mCmdFactoryManager.CreateCommand(keyset.CmdType, keyset.CmdArg);
						if (cmd == null)
						{
							WkLogger.LogError($"<color=yellow>Hint: {keyset.Hint}</color>||<color=green>Key:{keyset.KeySeq.KeyLabel}</color>||<color=red>Command:{keyset.CmdType}</color> has no valid command type,skipped ");
							return;
						}
						mCurrentNode.AddChild(new KeyNode(keyset, i, mCurrentNode, cmd));
					}
					else
					{
						if (kn.isLayer && string.IsNullOrEmpty(kn.Hint) && keyset.IsLayer)
						{
							kn.UpdateKeySet(keyset);
						}
						else
							WkLogger.LogError($"<color=yellow>Hint: {keyset.Hint}</color>||<color=green>Key:{keyset.KeySeq.KeyLabel}</color> already registered To \"<color=yellow>{kn.Hint}</color>\" ,skipped ");
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
