using System;

namespace PCP.Tools.WhichKey
{
	[Serializable]
	public class KeySet
	{
		public string KeySeq;
		public KeyCmdType type;
		public string HintText;
		public string CmdArg;
	}
}
