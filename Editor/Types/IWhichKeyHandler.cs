using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public interface IWhichKeyHandler
	{
		public bool ProcessKey(char key);

		public string[] GetLayerHints();
	}
}
