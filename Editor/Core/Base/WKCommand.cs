namespace PCP.Tools.WhichKey
{
	public interface WKCommand
	{
		bool isEnd { get; }
		void Execute();
	}

}