using System.Collections.Generic;
using UnityEngine;
using System;

namespace PCP.Tools.WhichKey
{
[Serializable]
	public class PreferencesWrapper
	{
		[SerializeField] public List<KeySet> keySets = new();
		[SerializeField] public bool ShowHint=true;
		[SerializeField] public float HintDelayTime=1;
		[SerializeField] public bool WindowFollowMouse=true;
		[SerializeField] public Vector2 FixedPosition;
		[SerializeField] public int MaxHintLines=10;
		[SerializeField] public float MaxColWidth=250;
		[SerializeField] public float FontSize=20;
		[SerializeField] public LoggingLevel LogLevel=LoggingLevel.Info;
	}
}