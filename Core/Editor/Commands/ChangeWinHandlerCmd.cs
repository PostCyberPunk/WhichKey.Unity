using PCP.WhichKey.Core;
namespace PCP.WhichKey.Types
{
	public abstract class ChangeWinHandlerCmd
	{
		public abstract BaseWinKeyHandler Handler { get; }
		public virtual bool isEnd => true;
		public void Excute()
		{
			OnActive();
			WhichKeyManager.instance.ChangeWinKeyHandler(Handler);
		}
		public virtual void OnActive()
		{

		}
	}
}