using UnityEngine;
namespace PCP.WhichKey.Types
{
	public abstract class WkArgFactory<T> : WKCommandFactory where T : WkArg
	{
		public T CreateArg(string arg)
		{
			return JsonUtility.FromJson<T>(arg);
		}
		public override WKCommand CreateCommand(string arg)
		{
			return CreateCommand(CreateArg(arg));
		}
		public abstract WKCommand CreateCommand(T arg);
	}
}