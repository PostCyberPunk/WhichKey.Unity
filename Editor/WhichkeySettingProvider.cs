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
		public const string SettingPath = "Preferences/WhichKey";
		[SettingsProvider]
		public static SettingsProvider CreateSettings()
		{
			// WhichKey.instance.Save();

			var provider = new SettingsProvider(SettingPath, SettingsScope.User)
			{
				label = "WhichKey",
				activateHandler = (searchContext, rootElement) =>
				{
					var settings = WhichKeySettings.instance;

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
					var applyButton = new Button(WhichKey.ApplySettings);
					applyButton.text = "Apply";
					root.Add(applyButton);

					// Create the Save to JSON button
					var saveButton = new Button(WhichKey.SaveSettingToJSON);
					saveButton.text = "Save to JSON";
					root.Add(saveButton);

					// Create the Load from JSON button
					var loadButton = new Button(WhichKey.LoadSettingFromJSON);
					loadButton.text = "Load from JSON";
					root.Add(loadButton);

					// Add the root visual element to the settings window
					rootElement.Add(root);
				},
				deactivateHandler = () =>
				{
					WhichKey.ApplySettings();
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

	}
}