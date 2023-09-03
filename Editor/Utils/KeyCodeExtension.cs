using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class KeyCodeExtension
{
    static StringBuilder sb = new StringBuilder();
    internal static int ToAscii(this KeyCode keyCode, bool shift = false)
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
    internal static bool IsValid(this KeyCode keyCode)
    {
        int num = (int)keyCode;
        if (num >= 303 && num <= 313 || num == 0)
            return false;
        return true;
    }
    internal static string ToLabel(this KeyCode keyCode)
    {
        return keyCode.ToAscii().ToLabel();
    }
    internal static string ToLabel(this int num)
    {
        if (num >= 33 && num <= 126)
            return ((char)num).ToString();
        else
            return ((KeyCode)num).ToString();
    }

    internal static string ToLabel(this List<int> list)
    {
        sb.Clear();
        foreach (var item in list)
        {
            sb.Append(item.ToLabel());
        }
        return sb.ToString();
    }
    internal static string ToLabel(this int[] array)
    {
        sb.Clear();
        for (int i = 0; i < array.Length; i++)
        {
            int item = array[i];
            sb.Append(item.ToLabel());
        }
        return sb.ToString();
    }
}
