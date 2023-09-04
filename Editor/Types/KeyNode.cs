using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	internal class KeyNode
	{
		public static int maxLine;
		private static readonly string layerHintFormat = "<color=yellow>{0}</color>  {1}\n";
		public int Key { private set; get; }
		public string Hint { private set; get; }
		public string CmdArg { private set; get; }
		public KeyCmdType Type { private set; get; }
		public KeyNode Parent { private set; get; }
		public string[] LayerHints { private set; get; }
		public List<KeyNode> Children { private set; get; } //OPT :Lets keep this list,maybe useful for fast reloading
		public bool hasChildren { get => Children.Count > 0; }

		public KeyNode(int key, string hintText)
		{
			Key = key;
			Hint = hintText;
			Type = KeyCmdType.Layer;
			Children = new List<KeyNode>();
		}
		public KeyNode(KeySet keySet, int index, KeyNode parent)
		{
			Parent = parent;
			Key = keySet.KeySeq[index];
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
		public KeyNode GetChildByKey(int key)
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
		public void SetParent(KeyNode parent)
		{
			Parent = parent;
			foreach (var child in Children)
			{
				child.SetParent(this);
			}
		}
		public void SetLayerHints()
		{
			if (!hasChildren) return;
			LayerHints = new string[Children.Count * 2];
			for (int i = 0; i < Children.Count; i++)
			{
				KeyNode child = Children[i];
				LayerHints[i * 2] = child.Key.ToLabel();
				LayerHints[i * 2 + 1] = child.Hint;
				child.SetLayerHints();
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

