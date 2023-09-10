namespace PCP.WhichKey.Utils
{
	public class JSONArrayWrapper<T>
	{
		public T[] LayerMap;
		public T[] MenuMap;
		public T[] KeyMap;

		public JSONArrayWrapper(T[] layer, T[] menu, T[] keymanp)
		{
			LayerMap = layer;
			MenuMap = menu;
			KeyMap = keymanp;
		}
	}
}