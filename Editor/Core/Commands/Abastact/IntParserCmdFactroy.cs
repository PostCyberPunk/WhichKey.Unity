namespace PCP.Tools.WhichKey
{
    public abstract class IntParserCmdFactroy : WKCommandFactory
    {
        public override WKCommand CreateCommand(string arg)
        {
            if (int.TryParse(arg, out int index))
                return CreateCommand(index);
            WhichKeyManager.LogError($"{CommandName} Command Invalid Index: {arg}");
            return null;
        }
        public abstract WKCommand CreateCommand(int arg);
    }

}

