namespace PCP.Tools.WhichKey
{
    internal class MenuCommandFactory : WKCommandFactory
    {
        public int TID { get; } = 1;
        public string CommandName { get; } = "Menu";
        public WKCommand CreateCommand(WKKeySet keySet)
        {
            return new MenuCommand(keySet);
        }
    }

}