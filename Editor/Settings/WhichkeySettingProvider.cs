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
					AddControlToRoot<Toggle, bool>("Show KeyHint", settings.ShowHint, root, (value) => settings.ShowHint = value);
					AddControlToRoot<FloatField, float>("Hint dealy time", settings.HintDelayTime, root, (value) => settings.HintDelayTime = value);
					AddControlToRoot<Toggle, bool>("Window follow mouse", settings.WindowFollowMouse, root, (value) => settings.WindowFollowMouse = value);
					if (!settings.WindowFollowMouse)
						AddControlToRoot<Vector2Field, Vector2>("Window postions", settings.FixedPosition, root, (value) => settings.FixedPosition = value);
					AddControlToRoot<IntegerField, int>("Max hint lines", settings.MaxHintLines, root, (value) => settings.MaxHintLines = value);
					AddControlToRoot<FloatField, float>("Max col width", settings.MaxColWidth, root, (value) => settings.MaxColWidth = value);
					AddControlToRoot<FloatField, float>("Font size", settings.FontSize, root, (value) => settings.FontSize = value);
					var logLevelField = new EnumField("Log level", settings.LogLevel);
					logLevelField.RegisterValueChangedCallback(evt => settings.LogLevel = (LoggingLevel)evt.newValue);
					root.Add(logLevelField);
					// Create the KeyMap list view
					var scrollView = new ScrollView();
					scrollView.style.flexGrow = 1;
					var keyMapListView = new ListView();

					keyMapListView.reorderable = true;
					keyMapListView.showAddRemoveFooter = true;
					keyMapListView.reorderMode = ListViewReorderMode.Animated;
					keyMapListView.showFoldoutHeader = true;
					keyMapListView.selectionType = SelectionType.Multiple;

					keyMapListView.bindingPath = "KeyMap";
					keyMapListView.BindProperty(settings.GetSerializedObject().FindProperty("KeyMap"));
					VisualTreeAsset keyItem = Resources.Load<VisualTreeAsset>("KeyMap");
					keyMapListView.makeItem = keyItem.CloneTree;

					scrollView.Add(keyMapListView);
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
					// Create the KeyMap list view
					var scrollView = new ScrollView();
					scrollView.style.flexGrow = 1;
					var keyMapListView = new ListView();

					keyMapListView.reorderable = true;
					keyMapListView.showAddRemoveFooter = true;
					keyMapListView.reorderMode = ListViewReorderMode.Animated;
					keyMapListView.showFoldoutHeader = true;
					keyMapListView.selectionType = SelectionType.Multiple;

					keyMapListView.bindingPath = "KeyMap";
					VisualTreeAsset keyItem = Resources.Load<VisualTreeAsset>("KeyMap");
					keyMapListView.makeItem = keyItem.CloneTree;

					scrollView.Add(keyMapListView);
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