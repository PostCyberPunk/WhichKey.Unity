using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
namespace PCP.Tools.WhichKey
{
    public class CmdFactoryManager
    {
        private Dictionary<int, WKCommandFactory> FactoryMap = new Dictionary<int, WKCommandFactory>();
        //BAD
        public static Dictionary<int, string> CommandTypeMap = new Dictionary<int, string>();
        public CmdFactoryManager()
        {
            RegisterCmdFactorires();
        }
        private void RegisterCmdFactorires()
        {
            var tList = TypeCache.GetTypesDerivedFrom<WKCommandFactory>();

            var fm = new Dictionary<int, WKCommandFactory>();
            var cm = new Dictionary<int, string>();
            cm.Add(0, "Layer");
            foreach (var item in tList)
            {
                if (item.IsDefined(typeof(WhichKeyIgnoreFactory), false)) continue;
                if (item.IsAbstract) continue;

                var factory = Activator.CreateInstance(item) as WKCommandFactory;
                int id = factory.TID;
                if (fm.ContainsKey(id))
                {
                    WhichKeyManager.LogWarning($"Command Factory <color=red>{id}</color> already registered");
                    return;
                }
                fm.Add(id, factory);
                cm.Add(id, factory.CommandName);
            }
            FactoryMap = new Dictionary<int, WKCommandFactory>(fm.OrderBy(x => x.Key));
            CommandTypeMap = new Dictionary<int, string>(cm.OrderBy(x => x.Key));
        }
        public WKCommand CreateCommand(int id, string arg)
        {
            if (FactoryMap.ContainsKey(id))
            {
                return FactoryMap[id].CreateCommand(arg);
            }
            else
            {
                WhichKeyManager.LogError($"Command Factory <color=red>{id}</color> not found");
                return null;
            }
        }
    }
}
