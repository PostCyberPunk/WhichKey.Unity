using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using PCP.WhichKey.Log;

namespace PCP.WhichKey.Core
{
    internal class WkMethodManager
    {
        private Dictionary<int, MethodInfo> mMethodTable = new();
        public WkMethodManager()
        {
            var mlist = TypeCache.GetMethodsWithAttribute<WhichKeyMethod>();
            foreach (var m in mlist)
            {
                if (mMethodTable.ContainsKey(m.GetCustomAttribute<WhichKeyMethod>().UID))
                {
                    WkLogger.LogError($"Method with id <color=red>{m.GetCustomAttribute<WhichKeyMethod>().UID}</color> has been registered!");
                    continue;
                }
                if (m.IsStatic && m.GetParameters().Length == 0)
                    mMethodTable.Add(m.GetCustomAttribute<WhichKeyMethod>().UID, m);
                else
                    WkLogger.LogError($"Method <color=red>{m.Name}</color> can't be invoke by WhichKey!make sure it is static and has no arguments.");
            }
        }
        public void Invoke(int id)
        {
            if (mMethodTable.ContainsKey(id))
            {
                try
                {
                    mMethodTable[id].Invoke(null, null);
                }
                catch (System.Exception e)
                {
                    WkLogger.LogError($"Method with id <color=red>{id}</color> exception: {e.Message}");
                }
            }
            else
            {
                WkLogger.LogError($"Method with id <color=red>{id}</color> not found!");
            }
        }
    }
}
