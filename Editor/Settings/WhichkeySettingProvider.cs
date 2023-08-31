using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
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

					// Create the root visual element
					var root = new VisualElement();
					root.style.flexDirection = FlexDirection.Column;
					root.style.paddingLeft = 10;
					root.style.paddingRight = 10;
					root.style.paddingTop = 10;
					root.style.paddingBottom = 10;

					// Create the Show KeyHint toggle
					AddControlToRoot<Toggle, bool>("Show KeyHint", settings.Preferences.ShowHint, root, (value) => settings.Preferences.ShowHint = value);
					AddControlToRoot<FloatField, float>("Hint dealy time", settings.Preferences.HintDelayTime, root, (value) => settings.Preferences.HintDelayTime = value);
					AddControlToRoot<Toggle, bool>("Window follow mouse", settings.Preferences.WindowFollowMouse, root, (value) => settings.Preferences.WindowFollowMouse = value);
					if (!settings.Preferences.WindowFollowMouse)
						AddControlToRoot<Vector2Field, Vector2>("Window postions", settings.Preferences.FixedPosition, root, (value) => settings.Preferences.FixedPosition = value);
					AddControlToRoot<IntegerField, int>("Max hint lines", settings.Preferences.MaxHintLines, root, (value) => settings.Preferences.MaxHintLines = value);
					AddControlToRoot<FloatField, float>("Max col width", settings.Preferences.MaxColWidth, root, (value) => settings.Preferences.MaxColWidth = value);
					AddControlToRoot<FloatField, float>("Font size", settings.Preferences.FontSize, root, (value) => settings.Preferences.FontSize = value);
					var logLevelField = new EnumField("Log level", settings.Preferences.LogLevel);
					logLevelField.RegisterValueChangedCallback(evt => settings.Preferences.LogLevel = (LoggingLevel)evt.newValue);
					root.Add(logLevelField);
					// Create the KeySets list view
					var scrollView = new ScrollView();
					scrollView.style.flexGrow = 1;
					var keySetsListView = new ListView();

					keySetsListView.reorderable = true;
					keySetsListView.showAddRemoveFooter = true;
					keySetsListView.reorderMode = ListViewReorderMode.Animated;
					keySetsListView.showFoldoutHeader = true;
					keySetsListView.selectionType = SelectionType.Multiple;

					keySetsListView.bindingPath = "keySets";
					keySetsListView.BindProperty(settings.GetSerializedObject().FindProperty("keySets"));
					VisualTreeAsset keyItem = Resources.Load<VisualTreeAsset>("KeySets");
					keySetsListView.makeItem = keyItem.CloneTree;

					scrollView.Add(keySetsListView);
					root.Add(scrollView);

					// Create the Apply button
					var applyButton = new Button(WhichKey.ApplyPreferences);
					applyButton.text = "Apply";
					root.Add(applyButton);

					// Create the Save to JSON button
					var saveButton = new Button(WhichKey.SavePreferenceToJSON);
					saveButton.text = "Save to JSON";
					root.Add(saveButton);

					// Create the Load from JSON button
					var loadButton = new Button(WhichKey.LoadPreferenceFromJSON);
					loadButton.text = "Load from JSON";
					root.Add(loadButton);

					// Add the root visual element to the settings window
					rootElement.Add(root);
				},
				deactivateHandler = () =>
				{
					WhichKey.ApplyPreferences();
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

					// Create the root visual element
					var root = new VisualElement();
					root.style.flexDirection = FlexDirection.Column;
					root.style.paddingLeft = 10;
					root.style.paddingRight = 10;
					root.style.paddingTop = 10;
					root.style.paddingBottom = 10;

					// Create the Show KeyHint toggle
					AddControlToRoot<Toggle, bool>("Show", "showHintInstant", root);
					// Create the KeySets list view
					var scrollView = new ScrollView();
					scrollView.style.flexGrow = 1;
					var keySetsListView = new ListView();

					keySetsListView.reorderable = true;
					keySetsListView.showAddRemoveFooter = true;
					keySetsListView.reorderMode = ListViewReorderMode.Animated;
					keySetsListView.showFoldoutHeader = true;
					keySetsListView.selectionType = SelectionType.Multiple;

					keySetsListView.bindingPath = "keySets";
					VisualTreeAsset keyItem = Resources.Load<VisualTreeAsset>("KeySets");
					keySetsListView.makeItem = keyItem.CloneTree;

					scrollView.Add(keySetsListView);
					root.Add(scrollView);

					// Create the Apply button
					var applyButton = new Button(WhichKey.ApplyPreferences);
					applyButton.text = "Apply";
					root.Add(applyButton);

					// Create the Save to JSON button
					var saveButton = new Button(WhichKey.SavePreferenceToJSON);
					saveButton.text = "Save to JSON";
					root.Add(saveButton);

					// Create the Load from JSON button
					var loadButton = new Button(WhichKey.LoadPreferenceFromJSON);
					loadButton.text = "Load from JSON";
					root.Add(loadButton);

					// Add the root visual element to the settings window
					root.Bind(settings);
					rootElement.Add(root);
				},
				deactivateHandler = () =>
				{
					WhichkeyProjectSettings.Save();
					WhichKey.Refresh();
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