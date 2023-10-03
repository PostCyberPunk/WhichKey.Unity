using PCP.WhichKey.Utils;
using UnityEngine;

namespace PCP.WhichKey.Types
{
	[System.Serializable]
	public struct LayerHint
	{
		[SerializeField]
		public string KeyLabel;
		[SerializeField]
		public string Hint;
		public LayerHint(int key, string hint)
		{
			KeyLabel = key.ToLabel();
			Hint = hint;
		}
		public LayerHint(WkKeySeq keys, string hint)
		{
			KeyLabel = keys.KeyLabel;
			Hint = hint;
		}
	}
}