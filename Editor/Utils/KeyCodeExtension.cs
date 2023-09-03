using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public static class KeyCodeExtension
{
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
    internal static string ToLabel(this int num)
    {
        if (num >= 33 && num <= 126)
            return ((char)num).ToString();
        else
            return ((KeyCode)num).ToString();
    }

}
