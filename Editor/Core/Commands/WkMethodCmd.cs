namespace PCP.Tools.WhichKey
{
    internal class WkMethodCmd : WKCommand
	{
		private static WkMethodManager mMethodManager = new();
		private int methodID;
		public WkMethodCmd(int id)
		{
			methodID = id;
		}
		public void Execute()
		{
			mMethodManager.Invoke(methodID);
		}
	}
}