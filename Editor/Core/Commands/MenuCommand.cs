using UnityEditor;

namespace PCP.Tools.WhichKey
{
    internal class MenuCommand : WKCommand
    {

        public string Hint { get; }
        private string menuPath;
        public MenuCommand(WKKeySet keySet)
        {
            Hint = keySet.Hint;
            menuPath = keySet.CmdArg;
        }
        public void Execute()
        {
            if (!EditorApplication.ExecuteMenuItem(menuPath))
                WhichKeyManager.LogWarning($"Menu {menuPath} not available");
        }
    }

}