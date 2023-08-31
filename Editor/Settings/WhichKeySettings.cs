using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace PCP.Tools.WhichKey
{
	[FilePath("Preferences/Whichkey.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class WhichKeyPreferences : ScriptableSingleton<WhichKeyPreferences>
	{
		public PreferencesWrapper Preferences;
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
