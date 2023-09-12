namespace PCP.WhichKey.Core
{
	public abstract class BaseWinKeyHandler
	{
		public abstract void ShowWindow();
		public abstract void OnActive();
		public abstract void CloseWindow();
		public abstract void HandleKey(int key);
	}
}