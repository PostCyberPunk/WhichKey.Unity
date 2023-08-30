using System;

namespace PCP.Tools.WhichKey
{
	[Serializable]
	public struct KeySet
	{
		public string KeySeq;
		public KeyCmdType type;
		public string HintText;
		public string CmdArg;
	}
}
