using UnityEngine;
using System;
using System.Collections;
using System.Net;

namespace PCP.Tools.WhichKey
{
    public abstract class WKCmdArg
    {
        [SerializeField]
        public string ArgStr;
        [SerializeField]
        public int ArgInt;
        public abstract void Save();
        public abstract void Load();
    }
    public interface WKHintSource
    {
        String[] GetHint();
    }

}