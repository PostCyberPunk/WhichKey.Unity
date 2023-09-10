using PCP.WhichKey.Types;

namespace PCP.WhichKey.Core
{
    internal class MenuCommandFactory : WKCommandFactory
  {
    public override int TID => 1;
    public override string CommandName => "Menu";
    public override WKCommand CreateCommand(string arg)
    {
      return new MenuCommand(arg) as WKCommand;
    }
  }
}