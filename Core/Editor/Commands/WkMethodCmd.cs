using PCP.WhichKey.Core;

namespace PCP.WhichKey.Types
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