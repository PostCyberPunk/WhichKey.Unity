using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public class KeyMapWrapper
	{
		public List<KeySet> KeyMap;
		public KeyMapWrapper(List<KeySet> KeyMap)
		{
			this.KeyMap = KeyMap;
		}
	}

}