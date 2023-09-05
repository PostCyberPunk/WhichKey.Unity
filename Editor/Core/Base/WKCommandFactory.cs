using UnityEditor;

namespace PCP.Tools.WhichKey
{
    public abstract class WKCommandFactory
    {
        protected abstract int TID { get; }
        protected abstract string CommandName { get; }
        public abstract WKCommand CreateCommand(string arg);
        [InitializeOnLoadMethod]
        private static void Register()
        {
        }
    }

}