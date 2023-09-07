using UnityEditor;

namespace PCP.Tools.WhichKey
{
    internal class AssetsNavCommand : ChangeHandlerCmd
    {
        public override IWKHandler Handler => mAssetsHandler;
        public override bool isEnd => false;
        private readonly static AssetsHandler mAssetsHandler = new();
        private int mIndex;
        protected bool save = false;
        public override int Depth => 1; 
        public AssetsNavCommand(int index)
        {
            mIndex = index;
        }
        public AssetsNavCommand(int index, bool saving) : this(index)
        {
            save = saving;
        }
        protected override void ActiveHandler()
        {
            mAssetsHandler.ChangeAction(save);
            mAssetsHandler.ProecessArg(mIndex);
        }
    }
    internal class AssetsNavSaveCommand : AssetsNavCommand
    {
        public AssetsNavSaveCommand(int index) : base(index, true) { }
    }
    internal class AssetsNavCommandFactory : IntParserCmdFactroy
    {
        public override int TID => 11;
        public override string CommandName => "AssetsNav";
        public override WKCommand CreateCommand(int arg)
        {
            return new AssetsNavCommand(arg);
        }
    }
    internal class AssetsNavSaveCommandFactory : IntParserCmdFactroy
    {
        public override int TID => 12;
        public override string CommandName => "AssetsNavSave";
        public override WKCommand CreateCommand(int arg)
        {
            return new AssetsNavSaveCommand(arg);
        }
    }

}

