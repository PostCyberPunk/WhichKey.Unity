using UnityEditor;
using UnityEngine;
using PCP.WhichKey.Log;
using PCP.WhichKey.Utils;

namespace PCP.WhichKey.Core
{
	public class WkSettingBase<T> : ScriptableSingleton<T> where T : WkSettingBase<T>
	{
		public KeySet[] LayerMap = new KeySet[0];
		public KeySet[] MenuMap = new KeySet[0];
		public KeySet[] KeyMap = new KeySet[0];
		private string jsonName => typeof(T).Name;

		private void OnEnable()
		{
			hideFlags &= ~HideFlags.NotEditable;
		}

		public void Save()
		{
			Undo.RegisterCompleteObjectUndo(instance, $"Save {jsonName} Settings");
			instance.Save(true);
		}

		public SerializedObject GetSerializedObject()
		{
			return new SerializedObject(this);
		}

		public void Apply()
		{
			Save();
			WhichKeyManager.instance.Refresh();
		}

		public void LoadFromJson() => LoadFromJson($"Assets/{jsonName}.json");
		public void LoadFromJson(string path)
		{
			AssetDatabase.Refresh();
			TextAsset jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
			if (jsonFile == null)
			{
				WkLogger.LogError($"{path} not found");
				return;
			}

			LayerMap = JsonUtility.FromJson<JSONArrayWrapper<KeySet>>(jsonFile.text).LayerMap;
			MenuMap = JsonUtility.FromJson<JSONArrayWrapper<KeySet>>(jsonFile.text).MenuMap;
			KeyMap = JsonUtility.FromJson<JSONArrayWrapper<KeySet>>(jsonFile.text).KeyMap;
		}
		public void SaveToJson() => SaveToJson($"Assets/{jsonName}.json");
		public void SaveToJson(string path)
		{
			JSONArrayWrapper<KeySet> keySetsWrapper = new JSONArrayWrapper<KeySet>(LayerMap, MenuMap, KeyMap);
			string json = JsonUtility.ToJson(keySetsWrapper, true);
			WkLogger.LogInfo($"Saved {path}");
			System.IO.File.WriteAllText(path, json);
			AssetDatabase.Refresh();
		}
	}
}