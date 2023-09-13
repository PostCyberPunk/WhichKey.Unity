using UnityEngine;
using PCP.WhichKey.Core;

namespace PCP.WhichKey.Types
{
	public abstract class WindowKeyHandler<T> : BaseWinKeyHandler where T : WkBaseWindow
	{
		protected T MyWindow;

		public override void ShowWindow()
		{
			if (MyWindow == null)
			{
				MyWindow = ScriptableObject.CreateInstance<T>();
			}

			MyWindow.OnActive();
			MyWindow.UpdateDelayTimer();

			MyWindow.ShowPopup();
			MyWindow.minSize = new Vector2(0, 0);
			MyWindow.position = new Rect(0, 0, 0, 0);
		}

		public override void CloseWindow()
		{
			MyWindow.Close();
		}
	}
}