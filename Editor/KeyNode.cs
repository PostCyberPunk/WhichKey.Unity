using System.Collections.Generic;
using System.Text;
namespace PCP.Tools.WhichKey
{
	internal class KeyNode
	{
		public string KeySeq { private set; get; }
		public string Hint { private set; get; }
		public string CmdArg { private set; get; }
		public KeyCmdType Type { private set; get; }
		public KeyNode Parent { get; }
		public string LayerHints { private set; get; }
		public List<KeyNode> Children { private set; get; }
		public bool hasChildren { get => Children.Count > 0; }

		public KeyNode(string key, string hintText)
		{
			KeySeq = key;
			Hint = hintText;
			Type = KeyCmdType.Layer;
			Children = new List<KeyNode>();
		}
		public KeyNode(KeySet keySet)
		{
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
			if (Hint != string.Empty)
				Hint += "/" + keySet.HintText;
			else
				Hint = keySet.HintText;
			CmdArg = keySet.CmdArg;
			Type = keySet.type;
		}
		public KeyNode GetChildByKey(string key)
		{
			foreach (var child in Children)
			{
				if (child.KeySeq == key)
				{
					return child;
				}
			}

			return null;
		}
		public void SetLayerHints(StringBuilder sb)
		{

			if (!hasChildren) return;
			sb.Clear();
			foreach (var child in Children)
			{
				sb.Append(child.KeySeq);
				sb.Append(": ");
				sb.Append(child.Hint);
				sb.Append("...");
				child.SetLayerHints(sb);
			}
			LayerHints = sb.ToString();
		}
	}
}

