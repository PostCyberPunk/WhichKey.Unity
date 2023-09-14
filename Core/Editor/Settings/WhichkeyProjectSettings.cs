using UnityEditor;
using UnityEngine.UIElements;

namespace PCP.WhichKey.Core
{
	[FilePath("ProjectSettings/Whichkey/WkProjectSettings", FilePathAttribute.Location.ProjectFolder)]
	public class WhichkeyProjectSettings : WkSettingBase<WhichkeyProjectSettings>
	{
		public StyleSheet HintLabelSS;
	}
}