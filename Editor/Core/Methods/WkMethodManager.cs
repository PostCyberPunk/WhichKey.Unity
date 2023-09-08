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
                mMethodTable.Add(m.GetCustomAttribute<WhichKeyMethod>().UID, m);
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
