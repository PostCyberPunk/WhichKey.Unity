using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace PCP.Tools.WhichKey
{
	[FilePath("Preferences/WhichKeySettings.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class WhichKeySettings : ScriptableSingleton<WhichKeySettings>
	{
		public List<KeySet> keySets = new List<KeySet>();
		public bool ShowHint;
		public float HintDelayTime;
		private void Awake()
		{
			// WhichKey.mSettings = this;
			HintDelayTime = 1;
			Debug.Log("WhichKeySettings Awake");
		}
	}
	static class WhichkeySettingProvider
	{
		private static ReorderableList mKeySetList;
		[SettingsProvider]
		public static SettingsProvider CreateSettings()
		{
			var provider = new SettingsProvider("Preferences/WhichKeySettings", SettingsScope.User)
			{
				label = "WhichKey",
				guiHandler = (searchContext) =>
				{
					var settings = WhichKeySettings.instance;
					settings.ShowHint = EditorGUILayout.Toggle("Show Hint", settings.ShowHint);
					settings.HintDelayTime = EditorGUILayout.FloatField("Hint Delay Time", settings.HintDelayTime);

					mKeySetList = new ReorderableList(settings.keySets, typeof(KeySet), true, true, true, true);

					mKeySetList.drawHeaderCallback = (Rect rect) =>
					{
						EditorGUI.LabelField(rect, "KeySets");
					};
					mKeySetList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
					{
						//FIX
						// need to fix this method to use property
						KeySet element = settings.keySets[index];

						float lineHeight = EditorGUIUtility.singleLineHeight;
						float padding = 2f;

						EditorGUI.LabelField(new Rect(rect.x, rect.y + padding, 50, lineHeight), "Key");
						element.key = EditorGUI.TextField(new Rect(rect.x + 50, rect.y + padding, 50, lineHeight), element.key);
						EditorGUI.LabelField(new Rect(rect.x + 100, rect.y + padding, 50, lineHeight), "Type");
						element.type = (KeyCmdType)EditorGUI.EnumPopup(new Rect(rect.x + 150, rect.y + padding, 100, lineHeight), element.type);
						EditorGUI.LabelField(new Rect(rect.x + 250, rect.y + padding, 50, lineHeight), "Hint");
						element.HintText = EditorGUI.TextField(new Rect(rect.x + 300, rect.y + padding, 100, lineHeight), element.HintText);
						EditorGUI.LabelField(new Rect(rect.x + 400, rect.y + padding, 50, lineHeight), "Arg0");
						element.CmdArg0 = EditorGUI.TextField(new Rect(rect.x + 450, rect.y + padding, 100, lineHeight), element.CmdArg0);
						EditorGUI.LabelField(new Rect(rect.x + 550, rect.y + padding, 50, lineHeight), "Arg1");
						element.CmdArg1 = EditorGUI.TextField(new Rect(rect.x + 600, rect.y + padding, 100, lineHeight), element.CmdArg1);

					};
					mKeySetList.DoLayoutList();
					if (GUILayout.Button("Save to JSON"))
					{
						WhichKey.SaveSettingToJSON();
					}
					if (GUILayout.Button("Load from JSON"))
					{
						WhichKey.LoadSettingFromJSON();
					}
				},
				keywords = new HashSet<string>(new[] { "WhichKey" })
			};
			return provider;
		}
	}
}
