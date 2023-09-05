namespace PCP.Tools.WhichKey
{
    internal class LayerCommandFactory : WKCommandFactory
    {
        public int TID { get; } = 0;
        public string CommandName { get; } = "Layer";
        public WKCommand CreateCommand(KeySet keySet)
        {
            return new LayerCommand(keySet);
        }
    }

}