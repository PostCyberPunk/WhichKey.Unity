using UnityEditor;
using UnityEngine;
using PCP.WhichKey.Log;

namespace PCP.WhichKey.Core
{
    [FilePath("Preferences/WhichkeyPreferences.asset", FilePathAttribute.Location.PreferencesFolder)]
	internal class WhichKeyPreferences : WkSettingBase<WhichKeyPreferences>
	{
		public float Timeout = 1;
		public bool WindowFollowMouse = true;
		public Vector2 FixedPosition;
		public int MaxHintLines = 10;
		public float ColWidth = 250;
		public LoggingLevel LogLevel = LoggingLevel.Info;
	}
}
