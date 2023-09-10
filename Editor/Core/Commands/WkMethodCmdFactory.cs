namespace PCP.WhichKey.Types
{
    internal class WkMethodCmdFactory : IntParserCmdFactroy
	{
		public override int TID => 2;
		public override string CommandName => "Method";
		public override WKCommand CreateCommand(int arg)
		{
			return new WkMethodCmd(arg);
		}
	}
}