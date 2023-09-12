using PCP.WhichKey.Core;

namespace PCP.WhichKey.Types
{
	public abstract class ChangeHandlerCmd : WKCommand
	{
		public abstract IWkHandler Handler { get; }
		public virtual bool isEnd => false;
		public abstract int Depth { get; }

		public void Execute()
		{
			OnActive();
			WhichKeyManager.instance.ChangeHanlder(Handler, Depth);
		}

		/// <summary>
		/// make you handler ready for processkey in this method e.g. Reset the state of the handler
		/// </summary> <summary>
		/// 
		/// </summary>
		protected virtual void OnActive()
		{
		}
	}
}