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
		private StringBuilder mKeySeq;
		private KeyNode mRoot;
		private KeyNode mCurrentNode;
		private StringBuilder sb;
		private void Awake()
		{
			Init();
		}
		[InitializeOnLoadMethod]
		public static void DebugInit()
		{
			if (instance.mRoot == null)
				instance.Init(); ;
		}
		private void DebugShowHints()
		{
			// Debug.Log($"{item.Key}:{item.Value}");
		}
		public void Init()
		{
			mKeySeq = new();
			sb = new();

			mRoot = new KeyNode("", "");
			foreach (var keySet in keySets)
			{
				AddKeySetToTree(keySet);
			}
		}
		public bool KeyHandler(KeyCode keyCode, bool shift)
		{
			if (ProcessRawKey(keyCode, shift))
			{
				Complete();
				return true;
			}
			else
				return false;
		}
		private void AddKeySetToTree(KeySet keyset)
		{
			mCurrentNode = mRoot;
			for (int i = keyset.key.Length - 1; i >= 0; i--)
			{
				char key = keyset.key[i];
				KeyNode childNode = mCurrentNode.GetChildByKey(key.ToString());

				if (i == 0)
				{
					if (childNode == null)
						childNode = new KeyNode(keyset);
					else
						if (childNode.Type == KeyCmdType.Layer && keyset.type == KeyCmdType.Layer)
						childNode.UpdateKeySet(keyset);
					else
						LogError($"Key {keyset.key} already registered,skip Hint: {keyset.HintText},args: {keyset.CmdArg}");
					return;
				}
				if (childNode == null)
				{
					childNode = new KeyNode(key.ToString(), "");
					mCurrentNode.AddChild(childNode);
				}
				mCurrentNode = childNode;
			}
		}
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
			return ProcessKey(shift ? key : key.ToLower());
		}
		private bool ProcessKey(string key)
		{
			mKeySeq.Append(key);
			KeyNode kn = mCurrentNode.GetChildByKey(key);
			if (kn == null)
			{
				if (LogUnregisteredKey)
					LogWarning($"Key {mKeySeq} not found(You can disable this warning in preference)");
				return true;
			}
			switch(kn.Type)
			{
				case KeyCmdType.Layer:
					ProcessLayer(kn);
					return false;
				case KeyCmdType.Menu:
					ProcessMenu(kn.CmdArg);
					return true;
				default:
					return true;
			}
		}
		void ProcessLayer(KeyNode kn)
		{
			mCurrentNode = kn;
		}
		void ProcessMenu(string menuName)
		{
			if (!EditorApplication.ExecuteMenuItem(menuName))
				LogError($"Menu {menuName} not found");
			Complete();
		}
		private void Complete()
		{
			mKeySeq.Clear();
			ResetRoot();
		}
		private void ResetRoot()
		{
			mCurrentNode = mRoot;
		}
		public static void ApplySettins()
		{
			// instance.Save();
			instance.Init();
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

