using System.Collections.Generic;
using System.Text;
namespace PCP.Tools.WhichKey
{
	public class KeyNode
	{
		public string Key { get; }
		public string Hint { get; }
		public string CmdArg { get; }
		public KeyCmdType Type { get; }
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
			Hint = keySet.HintText;
			CmdArg = keySet.CmdArg;
			Type = keySet.type;
			Children = new List<KeyNode>();
		}

		public void AddChild(KeyNode child)
		{
			Children.Add(child);
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

