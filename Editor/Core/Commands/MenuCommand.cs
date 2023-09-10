using UnityEditor;
using PCP.WhichKey.Types;

namespace PCP.WhichKey.Core
{
    internal class MenuCommand : WKCommand
    {
        private string menuPath;
        public MenuCommand(string arg)
        {
            menuPath = arg;
        }
        public void Execute()
        {
            if (!EditorApplication.ExecuteMenuItem(menuPath))
                WkLogger.LogWarning($"Menu {menuPath} not available");
        }
    }

}