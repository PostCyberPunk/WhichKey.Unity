using System;

namespace PCP.Tools.WhichKey
{
    [Serializable]
	//OPT maybe claass is better
	public struct KeySet
	{
		public WkKeySeq KeySeq;
		public string Hint;
		public string CmdArg;
		public int CmdType;
		public bool IsLayer => CmdType == 0;
	}
}
