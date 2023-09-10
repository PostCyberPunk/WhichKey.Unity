using PCP.WhichKey.Types;
namespace PCP.WhichKey.Extra
{
    internal class AssetsNavCommand : ChangeHandlerCmd
    {
        public override IWkHandler Handler => mAssetsHandler;
        public override bool isEnd => false;
        private readonly static AssetsHandler mAssetsHandler = new();
        private int mIndex;
        protected bool Save;
        public override int Depth => 1; 
        public AssetsNavCommand(int index,bool save)
        {
            mIndex = index;
            Save = save;
        }
        protected override void ActiveHandler()
        {
            mAssetsHandler.ChangeAction(Save);
            mAssetsHandler.ProecessArg(mIndex);
        }
    }
    internal class AssetsNavCommandFactory : IntParserCmdFactroy
    {
        public override int TID => 11;
        public override string CommandName => "AssetsNav";
        public override WKCommand CreateCommand(int arg)
        {
            return new AssetsNavCommand(arg,false);
        }
    }
    internal class AssetsNavSaveCommandFactory : IntParserCmdFactroy
    {
        public override int TID => 12;
        public override string CommandName => "AssetsNavSave";
        public override WKCommand CreateCommand(int arg)
        {
            return new AssetsNavCommand(arg,true);
        }
    }

}

