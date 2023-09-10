namespace PCP.WhichKey.Types
{
    public interface WKCommand
	{
		bool isEnd =>true;
		void Execute();
	}

}