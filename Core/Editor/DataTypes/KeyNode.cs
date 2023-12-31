using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PCP.WhichKey.Types;
using PCP.WhichKey.Utils;

namespace PCP.WhichKey.Core
{
	internal class KeyNode
	{
		public static int maxLine;
		public int Key { private set; get; }
		public string Hint { private set; get; }
		public string CmdArg { private set; get; }
		public int Type { private set; get; }
		public KeyNode Parent { private set; get; }
		public LayerHint[] LayerHints { private set; get; }
		public List<KeyNode> Children { private set; get; } //OPT :Lets keep this list,maybe useful for fast reloading

		public bool hasChildren
		{
			get => Children.Count > 0;
		}

		public bool isLayer
		{
			get => Type == 0;
		}

		public WKCommand Command { private set; get; }

		/// <summary>
		/// Create a layer node
		/// </summary>
		/// <param name="key"></param>
		/// <param name="hintText"></param> 
		public KeyNode(int key, string hintText)
		{
			Key = key;
			Hint = hintText;
			Type = 0;
			Children = new List<KeyNode>();
		}

		public KeyNode(KeySet keySet, int index, WKCommand cmd)
		{
			Key = keySet.KeySeq.KeySeq[index];
			UpdateKeySet(keySet);
			Children = new List<KeyNode>();
			Command = cmd;
		}

		public KeyNode AddChild(KeyNode child)
		{
			Children.Add(child);
			return child;
		}

		public void UpdateKeySet(KeySet keySet)
		{
			if (!string.IsNullOrEmpty(keySet.Hint))
				Hint = keySet.Hint;
			CmdArg = keySet.CmdArg;
			Type = keySet.CmdType;
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
			if (!hasChildren)
				if (isLayer)
				{
					LayerHints = new LayerHint[0];
					return;
				}
				else
					return;
			LayerHints = new LayerHint[Children.Count];
			for (int i = 0; i < Children.Count; i++)
			{
				KeyNode child = Children[i];
				LayerHints[i] = new LayerHint(child.Key, child.Hint);
				child.SetLayerHints();
			}
		}

	}
}