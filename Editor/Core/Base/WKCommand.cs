namespace PCP.Tools.WhichKey
{
    public interface WKCommand
    {
        public string Hint { get; }
        void Execute();
    }

}