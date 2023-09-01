using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	internal class KeyNode
	{
		public static int maxLine;
		private static readonly string layerHintFormat = "<color=yellow>{0}</color>  {1}\n";
		public string KeySeq { private set; get; }
		public char Key { get => KeySeq[KeySeq.Length - 1]; }
		public string Hint { private set; get; }
		public string CmdArg { private set; get; }
		public KeyCmdType Type { private set; get; }
		public KeyNode Parent { get; }
		public string[] LayerHints { private set; get; }
		public List<KeyNode> Children { private set; get; } //OPT :Lets keep this list,maybe useful for fast reloading
		public bool hasChildren { get => Children.Count > 0; }

		public KeyNode(string key, string hintText)
		{
			KeySeq = key;
			Hint = hintText;
			Type = KeyCmdType.Layer;
			Children = new List<KeyNode>();
		}
		public KeyNode(KeySet keySet, KeyNode parent)
		{
			Parent = parent;
			KeySeq = keySet.KeySeq[keySet.KeySeq.Length - 1].ToString();
			UpdateKeySet(keySet);
			Children = new List<KeyNode>();
		}

		public KeyNode AddChild(KeyNode child)
		{
			Children.Add(child);
			return child;
		}

		public void UpdateKeySet(KeySet keySet)
		{
			if (Hint != null && keySet.HintText != null)
				Hint += "/" + keySet.HintText;
			else
				Hint = keySet.HintText;
			CmdArg = keySet.CmdArg;
			Type = keySet.type;
		}
		public KeyNode GetChildByKey(char key)
		{
			foreach (var child in Children)
			{
				if (child.Key == key)
				{
					return child;
				}
			}

			return null;
		}
		//OPT
		public void SetLayerHints()
		{
			if (!hasChildren) return;
			LayerHints = new string[Children.Count * 2];
			for (int i = 0; i < Children.Count; i++)
			{
				KeyNode child = Children[i];
				child.SetLayerHints();
				LayerHints[i * 2] = child.Key.ToString();
				LayerHints[i * 2 + 1] = child.Hint;
			}
		}
		public void SetCachedLayerHints()
		{
			if (!hasChildren) return;
			LayerHints = new string[Mathf.CeilToInt(Children.Count / (float)maxLine)];
			StringBuilder sb = new StringBuilder();
			int i = 1;
			foreach (var child in Children)
			{
				child.SetCachedLayerHints();
				sb.AppendFormat(layerHintFormat, child.Key, child.Hint);
				if (i % maxLine == 0)
				{
					LayerHints[i / maxLine - 1] = sb.ToString();
					sb.Clear();
				}
				i++;
			}
			if (sb.Length > 0)
			{
				LayerHints[Mathf.FloorToInt(i / maxLine)] = sb.ToString();
				sb.Clear();
			}
		}
	}
}

