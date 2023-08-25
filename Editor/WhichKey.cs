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
		public string mLayerHint { get => mCurrentNode.LayerHints; }
		private void Awake()
		{
			Init();
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
			mRoot.SetLayerHints();
			Complete();
			Debug.Log(mRoot.LayerHints);
		}
		private void AddKeySetToTree(KeySet keyset)
		{
			mCurrentNode = mRoot;
			for (int i = 0; i < keyset.KeySeq.Length; i++)
			{
				char key = keyset.KeySeq[i];
				KeyNode childNode = mCurrentNode.GetChildByKey(key);
				if (i == keyset.KeySeq.Length - 1)
				{
					if (childNode == null)
						childNode = mCurrentNode.AddChild(new KeyNode(keyset));
					else
					{
						if (childNode.Type == KeyCmdType.Layer && keyset.type == KeyCmdType.Layer)
							childNode.UpdateKeySet(keyset);
						else
							LogError($"KeySeq {keyset.KeySeq} already registered,skip Hint: {keyset.HintText},args: {keyset.CmdArg}");
					}
					return;
				}
				if (childNode == null)
				{
					childNode = mCurrentNode.AddChild(new KeyNode(key.ToString(), ""));
				}
				mCurrentNode = childNode;
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
			if (key.Length > 1)
			{
				LogError($"Unsupported key found :{key}");
				return true;
			}
			return ProcessKey(shift ? key[0] : key.ToLower()[0]);
		}
		private bool ProcessKey(char key)
		{
			mKeySeq.Append(key);
			KeyNode kn = mCurrentNode.GetChildByKey(key);
			if (kn == null)
			{
				if (LogUnregisteredKey)
					LogWarning($"KeySeq {mKeySeq} not found(You can disable this warning in preference)");
				return true;
			}
			switch (kn.Type)
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
				LogWarning($"Menu {menuName} not available");
			Complete();
		}
		internal void Complete()
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
		internal static void LogError(string msg) => Debug.LogError("Whichkey:" + msg);
		internal static void LogWarning(string msg) => Debug.LogWarning("Whichkey:" + msg);
		//Debug
		[InitializeOnLoadMethod]
		public static void DebugInit()
		{
			if (instance.mRoot == null)
				instance.Init(); ;
		}
		private void DebugShowHints()
		{
			// Debug.Log($"{item.KeySeq}:{item.Value}");
		}
	}
}

