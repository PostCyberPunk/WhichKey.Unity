using System.Collections.Generic;
using System.Text;
namespace PCP.Tools.WhichKey
{
	public class KeyNode
	{
		public string Key;
		public string Hint;
		public string CmdArg;
		public KeyCmdType Type;
		public KeyNode Parent { get; }
		public string HintsOfChildren { private set; get; }
		public List<KeyNode> Children { get; }
		public bool hasChildren { get => Children.Count > 0; }

		public KeyNode(string key, string hintText)
		{
			Key = key;
			Hint = hintText;
			Type = KeyCmdType.Layer;
			Children = new List<KeyNode>();
		}
		public KeyNode(KeySet keySet)
		{
			Key = keySet.key;
			UpdateKeySet(keySet);
			Children = new List<KeyNode>();
		}

		public void AddChild(KeyNode child)
		{
			Children.Add(child);
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
				if (child.Key == key)
				{
					return child;
				}
			}

			return null;
		}
		public void SetHintsOfChildren()
		{
			StringBuilder sb = new StringBuilder();

			foreach (var child in Children)
			{
				sb.Append(child.Key);
				sb.Append(": ");
				sb.Append(child.Hint);
				sb.Append("\n");
			}

			HintsOfChildren = sb.ToString();
		}
	}
}

