namespace PCP.WhichKey
{
    public abstract class ChangeHandlerCmd : WKCommand
    {
        public abstract IWKHandler Handler { get; }
        public abstract bool isEnd { get; }
        public abstract int Depth { get; }
        public void Execute()
        {
            ActiveHandler();
            WhichKeyManager.instance.ChangeHanlder(Handler,Depth);
        }
        /// <summary>
        /// make you handler ready for processkey in this method e.g. Reset the state of the handler
        /// </summary> <summary>
        /// 
        /// </summary>
        protected virtual void ActiveHandler() { }
    }

}

