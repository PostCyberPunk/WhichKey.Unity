using PCP.WhichKey.Core;
using PCP.WhichKey.Log;
namespace PCP.WhichKey.Types
{
    public abstract class IntParserCmdFactroy : WKCommandFactory
    {
        public override WKCommand CreateCommand(string arg)
        {
            if (int.TryParse(arg, out int index))
                return CreateCommand(index);
            WkLogger.LogError($"{CommandName} Command Invalid Index: {arg}");
            return null;
        }
        public abstract WKCommand CreateCommand(int arg);
    }

}

