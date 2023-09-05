using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
	public class JSONArrayWrapper<T>
	{
		public T[] array;
		public JSONArrayWrapper(T[] array)
		{
			this.array = array;
		}
	}

}