using PCP.WhichKey.Utils;

namespace PCP.WhichKey.Types
{
	[System.Serializable]
	public struct LayerHint
	{
		public string KeyLabel;
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