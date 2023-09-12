using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine.UIElements;
using PCP.WhichKey.UI;

namespace PCP.WhichKey.Core
{
	static class WhichkeySettingProvider
	{
		private static ReorderableList mKeySetList;
		public const string PreferencePath = "Preferences/WhichKey";
		public const string ProjectSettingPath = "Project/WhichKey";
		public static WhichKeyPreferences Preferences => WhichKeyPreferences.instance;
		public static WhichkeyProjectSettings ProjectSettings => WhichkeyProjectSettings.instance;

		[SettingsProvider]
		public static SettingsProvider CreatePreference()
		{
			return new WkKeySetsProvider<WhichKeyPreferences>(PreferencePath, SettingsScope.User, true, Preferences, UILoader.instance.Preferences);
		}

		[SettingsProvider]
		public static SettingsProvider CreateProjectSettings()
		{
			return new WkKeySetsProvider<WhichkeyProjectSettings>(ProjectSettingPath, SettingsScope.Project, false, ProjectSettings, UILoader.instance.ProjectSettings);
		}

		private class WkKeySetsProvider<T> : SettingsProvider where T : WkSettingBase<T>
		{
			public WkKeySetsProvider(string path, SettingsScope scopes, bool isPref, WkSettingBase<T> wks, VisualTreeAsset vts,
			IEnumerable<string> keywords = null) : base(path, scopes, keywords)
			{
				label = "WhichKey";
				activateHandler = (searchContext, rootElement) =>
				{
					var uil = UILoader.instance;
					// Create the root visual element
					var root = vts.CloneTree();


					var layerMap = root.Q<ListView>("LayerMap");
					layerMap.makeItem = () => WkBinderUtil.SetBinder(-1, uil.LayerSet);

					var menuMap = root.Q<ListView>("MenuMap");
					menuMap.makeItem = () =>
					{
						var e = WkBinderUtil.SetBinder(-1, uil.MenuSet);
						var btn = e.Q<Button>("Select");
						btn.clickable.clicked += () =>
						{
							MenuHelper.ShowWindow((path) => { e.Q<TextField>("Arg").value = path; });
						};
						return e;
					};
					menuMap.itemsAdded += (list) =>
					{
						foreach (var i in list)
						{
							wks.MenuMap[i].CmdType = 1;
						}
					};

					var keymap = root.Q<ListView>("KeyMap");
					keymap.makeItem = () => WkBinderUtil.SetBinder(-1, uil.KeySet);

					if (isPref)
					{
						//Show/Hide position field by FollowMouse toggle
						var winPosElement = root.Q<Vector2Field>("FixedPosition");
						winPosElement.style.display = Preferences.WindowFollowMouse ? DisplayStyle.None : DisplayStyle.Flex;
						root.Q<Toggle>("WindowFollowMouse").RegisterValueChangedCallback(evt =>
						{
							winPosElement.style.display = evt.newValue ? DisplayStyle.None : DisplayStyle.Flex;
							winPosElement.MarkDirtyRepaint();
						});
					}

					var applyButton = new Button(wks.Apply);
					applyButton.text = "Apply";
					root.Add(applyButton);

					var saveButton = new Button(wks.SaveToJson);
					saveButton.text = "Save to JSON";
					root.Add(saveButton);

					var loadButton = new Button(wks.LoadFromJson);
					loadButton.text = "Load from JSON";
					root.Add(loadButton);

					root.Bind(wks.GetSerializedObject());
					rootElement.Add(root);
				};
				deactivateHandler = wks.Apply;
				keywords = new HashSet<string>(new[] { "WhichKey" });
			}
		}


	}
}