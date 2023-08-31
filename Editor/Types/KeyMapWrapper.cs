using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public class KeyMapWrapper
	{
		public KeySet[] KeyMap;
		public KeyMapWrapper(KeySet[] KeyMap)
		{
			this.KeyMap = KeyMap;
		}
	}

}