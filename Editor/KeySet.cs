using System;

namespace PCP.Tools.WhichKey
{
	[Serializable]
	public class KeySet
	{
		public string key;
		public KeyCmdType type;
		public string HintText;
		public string CmdArg0;
		public string CmdArg1;
	}
}
