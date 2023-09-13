namespace PCP.WhichKey.Types
{
	public abstract class BaseWinKeyHandler
	{
		public abstract void ShowWindow();
		public virtual void OnActive() { }
		public abstract void HandleKey(int key);
	}
}