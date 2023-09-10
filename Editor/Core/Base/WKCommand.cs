namespace PCP.WhichKey
{
    public interface WKCommand
	{
		bool isEnd =>true;
		void Execute();
	}

}