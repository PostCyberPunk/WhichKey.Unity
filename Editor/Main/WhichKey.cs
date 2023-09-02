using System.Collections.Generic;
using UnityEditor;
using System.Text;

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

        [MenuItem("WhichKey/PrintAllMenuItem")]
        public static void PrintAllMenuItem()
        {

            var w = new PCP.Utils.BenchMark.StopWatch();
            var mlist = TypeCache.GetMethodsWithAttribute<MenuItem>();
            StringBuilder sb = new();
            var slist = new List<string>();
            foreach (var item in mlist)
            {
                var attribute = (MenuItem)item.GetCustomAttributes(typeof(MenuItem), false)[0];
                slist.Add(attribute.menuItem);
            }
            //sort slist by string
            slist.Sort();
            foreach (var item in slist)
            {
                sb.AppendLine(item);
            }
            //save to file
            System.IO.File.WriteAllText("Assets/AllMenuItem.txt", sb.ToString());
            WhichKeyManager.LogInfo("All MenuItem saved to Assets/AllMenuItem.txt");
            w.Finish();
        }
        #endregion

        #region Pulic Methods
        public static void Active(string key)
        {
            mManager.Active(key);
        }
        #endregion
    }
}
