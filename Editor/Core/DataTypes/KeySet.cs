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
		public void SetKeyLabel()
		{
			if (KeySeq.Length == 0)
				KeyLabel = "None";
			else
				KeyLabel = KeySeq.ToLabel();
		}
		public void Bind()
		{
			BindingWindow.ShowWindow((ks) =>
			{
				KeySeq = ks;
				SetKeyLabel();
			});
		}
		public bool IsLayer => CmdType == 0;
	}
}
