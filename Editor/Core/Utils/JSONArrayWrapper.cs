using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCP.Tools.WhichKey
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