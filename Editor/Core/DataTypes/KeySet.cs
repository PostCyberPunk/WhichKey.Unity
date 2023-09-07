using System;
using System.Drawing;

namespace PCP.Tools.WhichKey
{
	[Serializable]
	public class KeySet
	{
		public int[] KeySeq = new int[0];
		public string KeyLabel = "None";
		public string Hint;
		public string CmdArg;
		public int CmdType;
		public bool IsLayer => CmdType == 0;
	}
}
