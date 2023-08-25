using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace PCP.Tools.WhichKey
{
	static class WhichkeySettingProvider
	{
		private static ReorderableList mKeySetList;
		public const string SettingPath = "Preferences/WhichKey";
		[SettingsProvider]
		public static SettingsProvider CreateSettings()
		{
			WhichKey.instance.Save();
			var provider = new SettingsProvider(SettingPath, SettingsScope.User)
			{
				label = "WhichKey",
				guiHandler = (searchContext) =>
				{
					var settings = WhichKey.instance;
					settings.ShowHint = EditorGUILayout.Toggle("Show KeyHint", settings.ShowHint);
					settings.HintDelayTime = EditorGUILayout.FloatField("KeyHint Delay Time", settings.HintDelayTime);
					settings.LogUnregisteredKey = EditorGUILayout.Toggle("Log Unregistered KeySeq", settings.LogUnregisteredKey);

					mKeySetList = new(settings.keySets, typeof(KeySet), true, true, true, true)
					{
						drawHeaderCallback = (Rect rect) =>
					{
						EditorGUI.LabelField(rect, "KeySets");
					},
						drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
						{
							//FIX
							// need to fix this method to use property
							KeySet element = settings.keySets[index];

							float lineHeight = EditorGUIUtility.singleLineHeight;
							float padding = 1f;

							EditorGUI.LabelField(new Rect(rect.x, rect.y + padding, 50, lineHeight), "KeySeq");
							element.KeySeq = EditorGUI.TextField(new Rect(rect.x + 50, rect.y + padding, 40, lineHeight), element.KeySeq);
							EditorGUI.LabelField(new Rect(rect.x + 100, rect.y + padding, 50, lineHeight), "Type");
							element.type = (KeyCmdType)EditorGUI.EnumPopup(new Rect(rect.x + 150, rect.y + padding, 80, lineHeight), element.type);
							EditorGUI.LabelField(new Rect(rect.x + 250, rect.y + padding, 50, lineHeight), "KeyHint");
							element.HintText = EditorGUI.TextField(new Rect(rect.x + 300, rect.y + padding, 200, lineHeight), element.HintText);
							EditorGUI.LabelField(new Rect(rect.x + 510, rect.y + padding, 50, lineHeight), "Arg");
							element.CmdArg = EditorGUI.TextField(new Rect(rect.x + 550, rect.y + padding, 250, lineHeight), element.CmdArg);

						}
					};
					mKeySetList.DoLayoutList();
					if (GUILayout.Button("Apply"))
					{
						WhichKey.ApplySettins();
					}
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
