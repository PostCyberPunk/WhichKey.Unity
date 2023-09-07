using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace PCP.Tools.WhichKey
{
	static class WhichkeySettingProvider
	{
		private static ReorderableList mKeySetList;
		public const string PreferencePath = "Preferences/WhichKey";
		public const string ProjectSettingPath = "Project/WhichKey";
		[SettingsProvider]
		public static SettingsProvider CreatePreference()
		{
			// WhichKey.instance.Save();

			var provider = new SettingsProvider(PreferencePath, SettingsScope.User)
			{
				label = "WhichKey",
				activateHandler = (searchContext, rootElement) =>
				{
					var settings = WhichKeyPreferences.instance;
					var vts = WhichKeyManager.mUILoader;
					// Create the root visual element
					var root = vts.Preferences.CloneTree();
					root.Q<ListView>("KeyMap").makeItem = vts.KeySet.CloneTree;
					var keymap = root.Q<ListView>("KeyMap");
					keymap.bindItem = (e, i) =>
					{
						var visualElement = e as BindableElement;
						var p = settings.GetSerializedObject().FindProperty("KeyMap").GetArrayElementAtIndex(i);
						e.Q<VisualElement>("Base").Insert(0, new WkBinder().CreatePropertyGUI(p.FindPropertyRelative("Keys")));

						//2 use custom control to bind
						// var wkb = new WkBinder(() =>
						// {
						// 	BindingWindow.ShowWindow((int[] ks) =>
						// 	{
						// 		settings.KeyMap[i].Keys = ks;
						// 	}
						// 	, -1, "WhichKey Binding");
						// });
						// e.Q<VisualElement>("Base").Insert(0, wkb);


						//1 Use uxml temaple to bind
						// var wkbind = e.Q<VisualElement>("WKBind") as BindableElement;
						// wkbind.BindProperty(p.FindPropertyRelative("Keys"));
						// var btn = wkbind.Q<Button>("Bind");
						// btn.clickable = new Clickable(() =>
						// {
						// 	BindingWindow.ShowWindow((int[] ks) =>
						// 	{
						// 		settings.KeyMap[i].Keys = ks;
						// 	}, -1, "WhichKey Binding");
						// });


						visualElement.BindProperty(p);
					};
					// keymap.itemsSource = settings.KeyMap;
					//Show/Hide position field by FollowMouse toggle
					var winPosElement = root.Q<Vector2Field>("FixedPosition");
					winPosElement.style.display = settings.WindowFollowMouse ? DisplayStyle.None : DisplayStyle.Flex;
					root.Q<Toggle>("WindowFollowMouse").RegisterValueChangedCallback(evt =>
					{
						winPosElement.style.display = evt.newValue ? DisplayStyle.None : DisplayStyle.Flex;
						winPosElement.MarkDirtyRepaint();
					});

					// Create the Apply button
					var applyButton = new Button(WhichKeyManager.instance.ApplyPreferences);
					applyButton.text = "Apply";
					root.Add(applyButton);

					// Create the Save to JSON button
					var saveButton = new Button(WhichKeyManager.instance.SavePreferenceToJSON);
					saveButton.text = "Save to JSON";
					root.Add(saveButton);

					// Create the Load from JSON button
					var loadButton = new Button(WhichKeyManager.instance.LoadPreferenceFromJSON);
					loadButton.text = "Load from JSON";
					root.Add(loadButton);
					root.Bind(settings.GetSerializedObject());

					// Add the root visual element to the settings window
					rootElement.Add(root);
				},
				deactivateHandler = () =>
				{
					WhichKeyManager.instance.ApplyPreferences();
				},
				keywords = new HashSet<string>(new[] { "WhichKey" })

			};

			return provider;
		}
		[SettingsProvider]
		public static SettingsProvider CreateProjectSettings()
		{
			// WhichKey.instance.Save();

			var provider = new SettingsProvider(ProjectSettingPath, SettingsScope.Project)
			{
				label = "WhichKey",
				activateHandler = (searchContext, rootElement) =>
				{
					var settings = WhichkeyProjectSettings.instance.GetSerializedObject();
					var vts = WhichKeyManager.mUILoader;

					var root = vts.ProjectSettings.CloneTree();

					ListView keymap = root.Q<ListView>("KeyMap");
					keymap.makeItem = vts.KeySet.CloneTree;

					root.Bind(settings);
					rootElement.Add(root);
				},
				deactivateHandler = () =>
				{
					WhichkeyProjectSettings.Save();
					// WhichKey.Refresh();
				},
				keywords = new HashSet<string>(new[] { "WhichKey" })

			};

			return provider;
		}

		private static void AddControlToRoot<T, U>(string label, U value, VisualElement root, Action<U> callback) where T : BaseField<U>, new()
		{
			var field = new T();
			field.label = label;
			field.value = value;
			field.RegisterValueChangedCallback(evt => callback(evt.newValue));
			root.Add(field);
		}

		private static void AddControlToRoot<T, U>(string label, string bindPath, VisualElement root) where T : BaseField<U>, new()
		{
			var field = new T();
			field.label = label;
			field.bindingPath = bindPath;
			root.Add(field);
		}
	}
}