using System.Collections.Generic;
using UnityEditor;

namespace PCP.Tools.WhichKey
{
	public abstract class WKCommand
	{
		public abstract int TID { get; }
		public abstract string CommandName { get; }
		public virtual bool isEnd => true;
		public abstract WKCommand Init();
		public abstract void Execute(string arg);
	}

}