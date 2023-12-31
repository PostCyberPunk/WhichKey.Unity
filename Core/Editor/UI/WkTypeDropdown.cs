using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using PCP.WhichKey.Core;

namespace PCP.WhichKey.UI
{
	public class WkTypeDropdown : PopupField<int>
	{
		private static Dictionary<int, string> mDict => CmdFactoryManager.CommandTypeMap;

		public new class UxmlFactory :
			UxmlFactory<WkTypeDropdown, BaseFieldTraits<int, UxmlIntAttributeDescription>>
		{
		}

		public WkTypeDropdown() : base(mDict.Keys.ToList(), 0, Format, Format)
		{
		}

		private static string Format(int i)
		{
			if (mDict.ContainsKey(i))
				return mDict[i];
			else
				return $"<color=red>Not Found {i.ToString()}</color>";
		}
	}
}