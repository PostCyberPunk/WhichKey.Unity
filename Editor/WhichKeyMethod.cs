using System;
namespace PCP.Tools.WhichKey
{
	[AttributeUsage(AttributeTargets.Method)]
	public class WhichKeyMethod : Attribute
	{
		public int UID;
		/// <summary>
		/// a static method can be called by WhichKey,return void and has no parameters
		/// </summary>
		/// <param name="id">The UID for method</param>
		public WhichKeyMethod(int id)
		{
			UID = id;
		}
	}
}
