using UnityEditor;
using UnityEngine;
using PCP.WhichKey.Log;

namespace PCP.WhichKey.Core
{
	[FilePath("WhichKey/Preferences.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class WhichKeyPreferences : WkSettingBase<WhichKeyPreferences>
	{
		public float Timeout = 0;
		public bool WindowFollowMouse = true;
		public Vector2 FixedPosition;
		public int MaxHintLines = 10;
		public float ColWidth = 250;
		[SerializeField]
		internal LoggingLevel LogLevel = LoggingLevel.Info;
		public bool UseInPlaymode = false;
	}
}