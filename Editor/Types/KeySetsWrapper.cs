using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public class KeySetsWrapper
	{
		public List<KeySet> keySets;
		public KeySetsWrapper(List<KeySet> keySets)
		{
			this.keySets = keySets;
		}
	}

}