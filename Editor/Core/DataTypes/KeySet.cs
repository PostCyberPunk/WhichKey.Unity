using System;

namespace PCP.Tools.WhichKey
{
	[Serializable]
	public class KeySet
	{
		public WkKey Keys;
		public string Hint;
		public string CmdArg;
		public int CmdType;
		public bool IsLayer => CmdType == 0;
	}
}
