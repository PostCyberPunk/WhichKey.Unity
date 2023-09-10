namespace PCP.Tools.WhichKey
{
    internal class MenuCommandFactory : WKCommandFactory
  {
    public override int TID => 1;
    public override string CommandName => "Menu";
    public override WKCommand CreateCommand(string arg)
    {
      return new MenuCommand(arg);
    }
  }
}