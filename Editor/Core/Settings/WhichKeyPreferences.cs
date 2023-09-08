using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
namespace PCP.Tools.WhichKey
{
	[FilePath("Preferences/WhichkeyPreferences.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class WhichKeyPreferences : ScriptableSingleton<WhichKeyPreferences>
	{
		[SerializeField] public KeySet[] LayerMap = new KeySet[0];
		[SerializeField] public KeySet[] MenuMap = new KeySet[0];
		[SerializeField] public KeySet[] KeyMap = new KeySet[0];
		[SerializeField] public float Timeout = 1;
		[SerializeField] public bool WindowFollowMouse = true;
		[SerializeField] public Vector2 FixedPosition;
		[SerializeField] public int MaxHintLines = 10;
		[SerializeField] public float ColWidth = 250;
		[SerializeField] public LoggingLevel LogLevel = LoggingLevel.Info;

		//Hint Label Style
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
