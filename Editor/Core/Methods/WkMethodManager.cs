using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
namespace PCP.Tools.WhichKey
{
    public class WkMethodManager : MonoBehaviour
    {
        private Dictionary<int, MethodInfo> mMethodTable = new();
        public WkMethodManager()
        {
            var mlist = TypeCache.GetMethodsWithAttribute<WhichKeyMethod>();
            foreach (var m in mlist)
            {
                if (mMethodTable.ContainsKey(m.GetCustomAttribute<WhichKeyMethod>().UID))
                {
                    WhichKeyManager.LogError($"Method with id <color=red>{m.GetCustomAttribute<WhichKeyMethod>().UID}</color> has been registered!");
                    continue;
                }
                if (m.IsStatic && m.GetParameters().Length == 0)
                    mMethodTable.Add(m.GetCustomAttribute<WhichKeyMethod>().UID, m);
                else
                    WhichKeyManager.LogError($"Method <color=red>{m.Name}</color> can't be invoke by WhichKey!make sure it is static and has no arguments.");
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
                    WhichKeyManager.LogError($"Method with id <color=red>{id}</color> exception: {e.Message}");
                }
            }
            else
            {
                WhichKeyManager.LogError($"Method with id <color=red>{id}</color> not found!");
            }
        }
    }
}
