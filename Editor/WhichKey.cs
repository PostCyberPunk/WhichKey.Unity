using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace PCP.Tools.WhichKey
{
	//
	[FilePath("Preferences/WhichKey.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class WhichKey : ScriptableSingleton<WhichKey>
	{
		[SerializeField] public List<KeySet> keySets = new();
		[SerializeField] public bool ShowHint;
		[SerializeField] public float HintDelayTime;
		[SerializeField] public bool LogUnregisteredKey;
		private Dictionary<int, KeySet> mKeySetDict;
		private StringBuilder mKeySeq;
		private void Awake()
		{
			Init();
		}
		public void Init()
		{
			mKeySetDict = new Dictionary<int, KeySet>();
			mKeySeq = new StringBuilder();

			foreach (var keySet in keySets)
			{
				try
				{
					mKeySetDict.Add(keySet.key.GetHashCode(), keySet);
				}
				catch (System.Exception e)
				{
					LogWarning(e.Message);
					continue;
				}
			}
		}
		private void Complete() => mKeySeq.Clear();
		public bool ProcessRawKey(KeyCode keyCode, bool shift)
		{
			//Oh bad bad bad code
			string key = keyCode.ToString();
			if (key.Length > 1)
			{
				if (key.StartsWith("Alpha"))
				{
					key = key.Replace("Alpha", "");
				}
				else if (key.StartsWith("Keypad"))
				{
					key = key.Replace("Keypad", "");
				}
				else
				{
					LogError($"Key {key} not supported");
					return true;
				}
			}
			return ProcessKeySeq(shift ? key : key.ToLower());
		}

		private bool ProcessKeySeq(string key)
		{
			mKeySeq ??= new();
			Debug.Log(key);
			mKeySeq.Append(key);
			//find key in keyset
			//wooo this is bad , maybe use some kind of tree to store keyset
			//OPT
			mKeySetDict.TryGetValue(mKeySeq.ToString().GetHashCode(), out KeySet keySet);

			if (keySet == null)
			{
				if (LogUnregisteredKey)
					LogWarning($"Key {mKeySeq} not found(You can disable this warning in prefecence)");
				Complete();
				return true;
			}
			//process key
			switch (keySet.type)
			{
				case KeyCmdType.Layer:
					ProcessLayer(keySet.CmdArg);
					return false;
				case KeyCmdType.Menu:
					ProcessMenu(keySet.CmdArg);
					return true;
				// case KeyCmdType.File:
				// 	WhichKey.ProcessFile(keySet.CmdArg0);
				// 	break;
				// case KeyCmdType.ChangeRoot:
				// 	WhichKey.ProcessChangeRoot(keySet.CmdArg0);
				// 	break;
				default:
					return true;
			}
		}
		void ProcessLayer(string layerName)
		{
			// LayerManager.instance.SetLayer(layerName);
		}
		void ProcessMenu(string menuName)
		{
			if (!EditorApplication.ExecuteMenuItem(menuName))
				LogError($"Menu {menuName} not found");
			Complete();
		}
		public static void ApplySettins()
		{
			instance.Init();
			instance.Save();
		}
		internal void Save()
		{
			Undo.RegisterCompleteObjectUndo(this, "Save WhichKey Settings");
			base.Save(true);
		}
		[MenuItem("Tools/WhichKey/LoadSettingFromJSON")]
		public static void LoadSettingFromJSON()
		{
			TextAsset jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/WhichKey.json");
			if (jsonFile == null)
			{
				LogError("WhichKey.json not found");
				return;
			}
			WhichKey.instance.keySets = JsonUtility.FromJson<WhichKey>(jsonFile.text).keySets;
		}
		[MenuItem("Tools/WhichKey/SaveSettingToJSON")]
		public static void SaveSettingToJSON()
		{
			string json = JsonUtility.ToJson(WhichKey.instance);
			System.IO.File.WriteAllText("Assets/WhichKey.json", json);
		}
		private static void LogError(string msg) => Debug.LogError("Whichkey:" + msg);
		private static void LogWarning(string msg) => Debug.LogWarning("Whichkey:" + msg);
	}
}
