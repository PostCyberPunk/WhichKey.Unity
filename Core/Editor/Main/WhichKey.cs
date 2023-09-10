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

		[MenuItem("WhichKey/Utils/PrintAllMenuItem")]
		public static void PrintAllMenuItem()
		{
			var mlist = TypeCache.GetMethodsWithAttribute<MenuItem>();
			StringBuilder sb = new();
			var slist = new List<string>();
			foreach (var item in mlist)
			{
				var attribute = (MenuItem)item.GetCustomAttributes(typeof(MenuItem), false)[0];
				slist.Add(attribute.menuItem);
			}

			//sort slist by string
			slist.Sort();
			foreach (var item in slist)
			{
				sb.AppendLine(item);
			}

			//save to file
			System.IO.File.WriteAllText("Assets/AllMenuItem.txt", sb.ToString());
			WkLogger.LogInfo("All MenuItem saved to Assets/AllMenuItem.txt");
		}

		#endregion

		#region Pulic Methods

		public static void Active(int[] key)
		{
			mManager.Active(key);
		}

		#endregion

		public static void OverrideTimeout(float time)
		{
			mManager.OverrideWindowTimeout(time);
		}
	}
}