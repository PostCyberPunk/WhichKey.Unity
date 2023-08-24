using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace PCP.Tools.WhichKey
{
	//
	[FilePath("Preferences/WhichKeySettings.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class WhichKeySettings : ScriptableSingleton<WhichKeySettings>
	{
		[SerializeField] public List<KeySet> keySets = new List<KeySet>();
		[SerializeField] public bool ShowHint;
		[SerializeField] public float HintDelayTime;
		private Dictionary<int, KeySet> mKeySetDict;
		private StringBuilder mKeySeq;
		private void Init()
		{
			mKeySetDict = new Dictionary<int, KeySet>();
			mKeySeq = new StringBuilder();

			foreach (var keySet in keySets)
			{
				mKeySetDict.Add(keySet.key.GetHashCode(), keySet);
			}
		}
		public bool ProcessKeySeq(string key)
		{
			mKeySeq.Append(key);
			mKeySeq.Replace("alpha", "");
			//find key in keyset
			KeySet keySet;
			mKeySetDict.TryGetValue(mKeySeq.ToString().GetHashCode(), out keySet);
			if (keySet == null)
			{
				mKeySeq.Clear();
				return false;
			}
			//process key
			switch (keySet.type)
			{
				case KeyCmdType.Layer:
					ProcessLayer(keySet.CmdArg0);
					break;
				case KeyCmdType.Menu:
					ProcessMenu(keySet.CmdArg0);
					break;
				// case KeyCmdType.File:
				// 	WhichKey.ProcessFile(keySet.CmdArg0);
				// 	break;
				// case KeyCmdType.ChangeRoot:
				// 	WhichKey.ProcessChangeRoot(keySet.CmdArg0);
				// 	break;
				default:
					return false;
			}
			return true;
		}
		void ProcessLayer(string layerName)
		{
			// LayerManager.instance.SetLayer(layerName);
		}
		void ProcessMenu(string menuName)
		{
			EditorApplication.ExecuteMenuItem(menuName);
		}
		public static void ApplySettins()
		{
			instance.Init();
		}
	}
}
