using System;
namespace PCP.Tools.WhichKey
{
	[Serializable]
	public class WKKeySet
	{
		public int[] KeySeq = new int[0];
		public string KeyLabel = "None";
		public string Hint;
		public int CmdType;
		public string CmdArg;
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
	}

}