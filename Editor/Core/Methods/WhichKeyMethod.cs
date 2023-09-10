using System;

namespace PCP.Tools.WhichKey
{
    [AttributeUsage(AttributeTargets.Method)]
	public class WhichKeyMethod : Attribute
	{
		public int UID;
		/// <summary>
		/// a static method can be invoke by WhichKey,return void and has no parameters
		/// </summary>
		/// <param name="id">The UID for method</param>
		public WhichKeyMethod(int id)
		{
			UID = id;
			// var method = new StackTrace().GetFrame(1).GetMethod();
			// // check if it is static and has no arguments
			// if (!method.IsStatic || method.GetParameters().Length != 0)
			// 	throw new InvalidOperationException("MyAttribute can only be applied to static methods with no arguments.");
		}
	}
}
