namespace PCP.Tools.WhichKey
{
    internal class MenuCommandFactory : WKCommandFactory
    {
        protected override int TID { get => 1; }
        protected override string CommandName { get => "Menu"; }
        public override WKCommand CreateCommand(string arg)
        {
            return new MenuCommand(arg);
        }
    }
}