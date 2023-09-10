using System;
using System.Collections;
using UnityEditor;
using UnityEditor.PackageManager.UI;

namespace PCP.Tools.WhichKey
{
    public abstract class WKCmdArg
    {
        public string ArgStr;
        public int ArgInt;
        public abstract void Save();
        public abstract void Load();
    }
    public interface WKHintSource
    {
        String[] GetHint();
    }
}