using UnityEngine;
using PCP.WhichKey.Core;

namespace PCP.WhichKey.Types
{
	public abstract class WindowKeyHandler<T> : BaseWinKeyHandler where T : WkBaseWindow
	{
		protected T window;

		public override void ShowWindow()
		{
			if (window == null)
			{
				window = ScriptableObject.CreateInstance<T>();
			}

			window.OnActive();
			window.UpdateDelayTimer();

			window.ShowPopup();
			window.minSize = new Vector2(0, 0);
			window.position = new Rect(0, 0, 0, 0);
		}

	}
}