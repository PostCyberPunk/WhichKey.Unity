using PCP.WhichKey.Core;
namespace PCP.WhichKey.Types
{
	public abstract class ChangeWinHandlerCmd:WKCommand
	{
		public abstract BaseWinKeyHandler Handler { get; }
		public virtual bool isEnd => true;
		public void Execute()
		{
			OnActive();
			WhichKeyManager.instance.ChangeWinKeyHandler(Handler);
		}
		public virtual void OnActive()
		{

		}
	}
}