
namespace PCP.Tools.WhichKey
{
    internal class AssetsNavCommand : WKCommand
    {
        public IWKHandler Handler => mAssetsHandler;
        public bool isEnd => false;
        private readonly static AssetsHandler mAssetsHandler = new();
        private int mIndex;
        public AssetsNavCommand(int index)
        {
            mIndex = index;
        }
        public void Execute()
        {
            WhichKeyManager.instance.ChangeHanlder(Handler);

            mAssetsHandler.ProecessArg(mIndex);
        }
    }
    internal class AssetesNavCommandFactory : WKCommandFactory
    {
        public override int TID => 11;
        public override string CommandName => "AssetsNav";
        public override WKCommand CreateCommand(string arg)
        {
            if (int.TryParse(arg, out int index))
                return new AssetsNavCommand(index);
            WhichKeyManager.LogError($"AssetsNav Command Invalid Index: {arg}");
            return null;
        }
    }
}
