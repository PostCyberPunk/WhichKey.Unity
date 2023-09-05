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
        public static void ChangeRoot() => BindingWindow.ShowWindow((key) => mManager.ChangeRoot(key));
        [MenuItem("WhichKey/ResetRoot")]
        public static void ResetRoot() => mManager.ChangeRoot(null);
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
        public static void Active(int[] key)
        {
            mManager.Active(key);
        }
        #endregion
        public static void CloseWin()
        {
            mManager.CloseHintsWindow();
        }
    }
}
