using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
    [CreateAssetMenu(fileName = "WhichKeySettings", menuName = "WhichKey/WhichKeySettings")]
    public class WhichKeySettings : ScriptableObject
    {
        public KeySet[] keySets;
        public bool ShowHint;
        public float HintDelayTime;
    }
}
