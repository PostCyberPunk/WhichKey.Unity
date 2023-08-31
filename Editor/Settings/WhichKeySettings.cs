using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace PCP.Tools.WhichKey
{
	[FilePath("Preferences/Whichkey.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class WhichKeyPreferences : ScriptableSingleton<WhichKeyPreferences>
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
		private void OnEnable()
		{
			hideFlags &= ~HideFlags.NotEditable;
		}
		internal void Save()
		{
			Undo.RegisterCompleteObjectUndo(this, "Save WhichKey Preferences");
			base.Save(true);
		}
		internal SerializedObject GetSerializedObject()
		{
			return new SerializedObject(this);
		}
	}
}
