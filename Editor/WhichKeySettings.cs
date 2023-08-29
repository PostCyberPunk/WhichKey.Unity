using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace PCP.Tools.WhichKey
{
	[FilePath("WhichKey/Settings.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class WhichKeySettings : ScriptableSingleton<WhichKeySettings>
	{
		[SerializeField] public List<KeySet> keySets = new();
		[SerializeField] public bool ShowHint=true;
		[SerializeField] public float HintDelayTime=1;
		[SerializeField] public bool LogUnregisteredKey=true;
		[SerializeField] public bool WindowFollowMouse=true;
		[SerializeField] public Vector2 FixedPosition;
		[SerializeField] public int MaxHintLines=10;
		[SerializeField] public float MaxColWidth=250;
		[SerializeField] public float FontSize=20;
		private void OnEnable()
		{
			hideFlags &= ~HideFlags.NotEditable;
		}
		internal void Save()
		{
			Undo.RegisterCompleteObjectUndo(this, "Save WhichKey Settings");
			base.Save(true);
		}
		internal SerializedObject GetSerializedObject()
		{
			return new SerializedObject(this);
		}
	}
}
