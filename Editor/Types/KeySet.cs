using System;

namespace PCP.Tools.WhichKey
{
	[Serializable]
	public struct KeySet
	{
		public int[] KeySeq;
		public KeyCmdType type;
		public string HintText;
		public string CmdArg;
	}
}
