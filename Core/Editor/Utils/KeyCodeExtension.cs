using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PCP.WhichKey.Utils
{
	public static class KeyCodeExtension
	{
		static StringBuilder sb = new StringBuilder();

		public static int ToAscii(this KeyCode keyCode, bool shift = false)
		{
			int result = (int)keyCode;
			if (result >= 303 && result <= 313)
				result = 0;
			if (result == 0 || result == 8 || result == 13 || result == 127)
				result = 0;
			if (shift && result >= 97 && result <= 122)
				result -= 32;
			return result;
		}

		public static bool IsValid(this KeyCode keyCode)
		{
			int num = (int)keyCode;
			if (num >= 303 && num <= 313 || num == 0)
				return false;
			return true;
		}

		public static string ToLabel(this KeyCode keyCode)
		{
			return keyCode.ToAscii().ToLabel();
		}

		public static string ToLabel(this int num)
		{
			if (num >= 33 && num <= 126)
				return ((char)num).ToString();
			else
				return ((KeyCode)num).ToString();
		}

		public static string ToLabel(this List<int> list)
		{
			sb.Clear();
			foreach (var item in list)
			{
				sb.AppendFormat("{0} ", item.ToLabel());
			}

			return sb.ToString();
		}

		public static string ToLabel(this int[] array)
		{
			sb.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				int item = array[i];
				sb.AppendFormat("{0} ", item.ToLabel());
			}

			return sb.ToString();
		}
	}
}