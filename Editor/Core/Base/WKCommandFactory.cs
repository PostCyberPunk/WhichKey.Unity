namespace PCP.WhichKey.Types
{
    public abstract class WKCommandFactory
	{
		public abstract int TID { get; }
		public abstract string CommandName { get; }
		public abstract WKCommand CreateCommand(string arg);
	}

}