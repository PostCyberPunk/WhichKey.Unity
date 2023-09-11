using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using PCP.WhichKey.Types;
using PCP.WhichKey.Log;

namespace PCP.WhichKey.Core
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
					WkLogger.LogWarning($"Command Factory <color=red>{id}</color> already registered");
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
				WKCommand cmd = null;
				try
				{
					cmd = FactoryMap[id].CreateCommand(arg);
				}
				catch (Exception e)
				{
					WkLogger.LogError($"Command Factory <color=red>{id}</color> create command failed\n{e}");
				}
				return cmd;
			}
			else
			{
				WkLogger.LogError($"Command Factory <color=red>{id}</color> not found");
				return null;
			}
		}
	}
}