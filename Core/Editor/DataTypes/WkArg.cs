using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PCP.WhichKey.Types
{
	[System.Serializable]
	public class WkArg
	{
		public virtual VisualElement CreateGUI()
		{
			return new PropertyField();
		}
	}
}