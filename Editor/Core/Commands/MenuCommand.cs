using UnityEditor;

namespace PCP.Tools.WhichKey
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
                WhichKeyManager.LogWarning($"Menu {menuPath} not available");
        }
    }

}