using System.Collections.Generic;
using System.Text;
using UnityEditor;
using PCP.WhichKey.Core;
using PCP.WhichKey.UI;
using PCP.WhichKey.Log;

namespace PCP.WhichKey
{
	[InitializeOnLoad]
	public static class WhichKey
	{
		private readonly static WhichKeyManager mManager;
		private readonly static UILoader mUILoader;

		static WhichKey()
		{
			mUILoader = new();
			mManager = new();
		}

		#region MenuItems

		public static void Refresh() => mManager.Refresh();

		[MenuItem("WhichKey/ChangeRoot",false,2)]
		public static void ChangeRoot() =>
			BindingWindow.ShowWindow((key) => mManager.ChangeRoot(key), -1, "<color=green>Change Root</color>");

		[MenuItem("WhichKey/ResetRoot",false,1)]
		public static void ResetRoot() => mManager.ChangeRoot(null);

		[MenuItem("WhichKey/Active", false, 0)]
		public static void Active() => mManager.ShowWindow();

		#endregion

		#region Pulic Methods

		//Temp
		public static void Active(int[] key)
		{
			mManager.Active(key);
		}

		#endregion
	}
}