using UnityEngine;
using UnityEditor;

namespace PCP.Tools.WhichKey
{
    [InitializeOnLoad]
    public static class WhichKey
    {
        private readonly static WhichKeyManager mManager;

        static WhichKey()
        {
            mManager = WhichKeyManager.instance;
            mManager.Init();
        }

        #region MenuItems

        [MenuItem("WhichKey/Refresh")]
        public static void Refresh() => mManager.Refresh();
        [MenuItem("WhichKey/ChangeRoot")]
        public static void ChangeRoot() => FloatingTextField.ShowInputField(mManager.ChangeRoot, "Change Root To:");
        [MenuItem("WhichKey/ResetRoot")]
        public static void ResetRoot() => mManager.ChangeRoot("");
        [MenuItem("WhichKey/Active")]
        public static void Active() => mManager.ShowWindow();

        #endregion

        #region Pulic Methods
        public static void Active(string key)
        {
            mManager.Active(key);
        }
        #endregion
    }
}
