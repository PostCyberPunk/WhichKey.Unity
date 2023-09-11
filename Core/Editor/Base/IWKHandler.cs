namespace PCP.WhichKey.Types
{
	public interface IWkHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">key to handle</param>
		/// <returns>true if you want to close hint window</returns>
		void ProcessKey(int key);

		/// <summary>
		/// the Hints you want to show in hint window
		/// </summary>
		/// <returns>odd elements are key,even elments are hint</returns>
		string[] GetLayerHints();
		/// <summary>
		/// the name of the layer, shows in hint window title
		/// </summary>
		/// <returns></returns> <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		string GetLayerName();
	}
}