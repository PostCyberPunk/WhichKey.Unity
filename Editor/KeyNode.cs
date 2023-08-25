using System.Collections.Generic;
using System.Text;
namespace PCP.Tools.WhichKey
{
	internal class KeyNode
	{
		public string KeySeq { private set; get; }
		public char Key { get => KeySeq[KeySeq.Length - 1]; }
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
		public void SetLayerHints()
		{

			if (!hasChildren) return;
			StringBuilder sb = new StringBuilder();
			foreach (var child in Children)
			{
				child.SetLayerHints();
				sb.Append(child.Key);
				sb.Append(": ");
				sb.Append(child.Hint);
				sb.Append("...");
			}
			LayerHints = sb.ToString();
		}
	}
}

