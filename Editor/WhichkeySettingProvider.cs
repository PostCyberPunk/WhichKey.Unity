using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

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
					var settings = WhichKey.instance;

					// Create the root visual element
					var root = new VisualElement();
					root.style.flexDirection = FlexDirection.Column;
					root.style.paddingLeft = 10;
					root.style.paddingRight = 10;
					root.style.paddingTop = 10;
					root.style.paddingBottom = 10;

					// Create the Show KeyHint toggle
					var showHintToggle = new Toggle("Show KeyHint");
					showHintToggle.value = settings.ShowHint;
					showHintToggle.RegisterValueChangedCallback(evt => settings.ShowHint = evt.newValue);
					root.Add(showHintToggle);

					// Create the KeyHint Delay Time field
					var hintDelayField = new FloatField("KeyHint Delay Time");
					hintDelayField.value = settings.HintDelayTime;
					hintDelayField.RegisterValueChangedCallback(evt => settings.HintDelayTime = evt.newValue);
					root.Add(hintDelayField);

					// Create the Log Unregistered KeySeq toggle
					var logUnregisteredToggle = new Toggle("Log Unregistered KeySeq");
					logUnregisteredToggle.value = settings.LogUnregisteredKey;
					logUnregisteredToggle.RegisterValueChangedCallback(evt => settings.LogUnregisteredKey = evt.newValue);
					root.Add(logUnregisteredToggle);

					// Create the KeySets list view
					var scrollView = new ScrollView();
					scrollView.style.flexGrow = 1;
					var keySetsListView = new ListView(settings.keySets, -1, MakeKeySetItem, BindKeySetItem);
					keySetsListView.reorderable = true;
					keySetsListView.showAddRemoveFooter = true;
					scrollView.Add(keySetsListView);
					root.Add(scrollView);

					// Create the Apply button
					var applyButton = new Button(WhichKey.ApplySettins);
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
					WhichKey.ApplySettins();
				},
				keywords = new HashSet<string>(new[] { "WhichKey" })
				
			};

			return provider;
		}
		private static VisualElement MakeKeySetItem()
		{
			var container = new VisualElement();
			container.style.flexDirection = FlexDirection.Row;
			// container.style.paddingTop = 5;
			// container.style.paddingBottom = 5;

			var keySeqField = new TextField();
			keySeqField.style.width = 50;
			container.Add(keySeqField);

			var typeField = new EnumField(KeyCmdType.Layer);
			typeField.style.width = 80;
			container.Add(typeField);

			var hintTextField = new TextField();
			hintTextField.style.width = 200;
			container.Add(hintTextField);

			var cmdArgField = new TextField();
			cmdArgField.style.width = 250;
			cmdArgField.style.flexGrow = 1;
			container.Add(cmdArgField);

			return container;
		}

		private static void BindKeySetItem(VisualElement element, int index)
		{
			var keySet = WhichKey.instance.keySets[index];

			var keySeqField = element.ElementAt(0) as TextField;
			keySeqField.value = keySet.KeySeq;

			var typeField = element.ElementAt(1) as EnumField;
			typeField.value = keySet.type;

			var hintTextField = element.ElementAt(2) as TextField;
			hintTextField.value = keySet.HintText;

			var cmdArgField = element.ElementAt(3) as TextField;
			cmdArgField.value = keySet.CmdArg;
		}
	}
}
