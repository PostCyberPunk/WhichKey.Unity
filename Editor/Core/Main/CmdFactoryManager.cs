using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
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
            foreach (var item in tList)
            {
                var factory = Activator.CreateInstance(item) as WKCommandFactory;
                int id = factory.TID;
                if (FactoryMap.ContainsKey(id))
                {
                    WhichKeyManager.LogWarning($"Command Factory <color=red>{id}</color> already registered");
                    return;
                }
                FactoryMap.Add(id, factory);
                CommandTypeMap.Add(id, factory.CommandName);
            }
        }
        public WKCommand CreateCommand(int id, string arg)
        {
            if (FactoryMap.ContainsKey(id))
            {
                return FactoryMap[id].CreateCommand(arg);
            }
            else return null;
        }
    }
}
