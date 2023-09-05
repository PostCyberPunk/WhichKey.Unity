namespace PCP.Tools.WhichKey
{
    public interface WKCommandFactory
    {
        public int TID { get; }
        public string CommandName { get; }
        public WKCommand CreateCommand(WKKeySet keySet);
    }

}