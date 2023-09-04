using System;

namespace PCP.Tools.WhichKey
{
	[Serializable]
	public class KeySet
	{
		public int[] KeySeq=new int[0];
		public KeyCmdType type;
		public string HintText;
		public string CmdArg;
	}
}
