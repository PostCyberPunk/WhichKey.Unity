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
		public KeySet[] LayerMap = new KeySet[0];
		public KeySet[] MenuMap = new KeySet[0];
		public KeySet[] KeyMap = new KeySet[0];
		public float Timeout = 1;
		public bool WindowFollowMouse = true;
		public Vector2 FixedPosition;
		public int MaxHintLines = 10;
		public float ColWidth = 250;
		public LoggingLevel LogLevel = LoggingLevel.Info;

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
