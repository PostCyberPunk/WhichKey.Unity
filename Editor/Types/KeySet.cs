using System;

namespace PCP.Tools.WhichKey
{
	[Serializable]
	public class KeySet
	{
		public int[] KeySeq = new int[0];
		public string KeyLabel = "None";
		public KeyCmdType type;
		public string HintText;
		public string CmdArg;
		public void SetKeyLabel()
		{
			if (KeySeq.Length == 0)
				KeyLabel = "None";
			else
				KeyLabel = KeySeq.ToLabel();
		}
	}
}
