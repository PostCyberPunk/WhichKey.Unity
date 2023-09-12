namespace PCP.WhichKey.Types
{
	public interface IWkHandler
	{
		/// <summary>
		/// the timeout of the hint window, -1 means use default timeout
		/// </summary> 
		float Timeout => -1f;
		/// <summary>
		/// the colum width of the hint window, -1 means use default width
		/// </summary> <summary>
		/// 
		/// </summary>
		float ColWidth => -1f;
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
		LayerHint[] GetLayerHints();
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