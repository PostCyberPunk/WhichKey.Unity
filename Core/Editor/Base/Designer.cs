using System.Collections.Generic;
using PCP.WhichKey.Core;
using Debug = UnityEngine.Debug;

namespace PCP.WhichKey
{
	internal class Designer
	{

		public interface IWkHintWindow
		{
			int mDepth => -1;
			float maxCol => WhichKeyPreferences.instance.ColWidth;
			float timeOutLen => WhichKeyPreferences.instance.Timeout;
			void Close();
			void ForceClose();
		}

		// public abstract class WKCmdArg
		// {
		//     public string ArgStr;
		//     public int ArgInt;
		//     public abstract void Save();
		//     public abstract void Load();
		// }
		// public interface WKHintSource
		// {
		//     String[] GetHint();
		// }
		// Cmd Factory Vs Cmd
		public class SourceData
		{
			public List<string> mData = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
		}

		public class PlanA
		{
			public interface CMD
			{
				void Excute();
			}

			public abstract class CmdFactory
			{
				public abstract CMD Create(string data);
			}
		}

		public class ImplA
		{
			public class Cmd : PlanA.CMD
			{
				public int mInt;

				public Cmd(int arg)
				{
					this.mInt = arg;
				}

				public void Excute()
				{
					Debug.Log(mInt + 1);
				}
			}

			public class cmdfactoryImpl : PlanA.CmdFactory
			{
				public override PlanA.CMD Create(string data)
				{
					if (int.TryParse(data, out int intger))
						return (new Cmd(intger));
					else return null;
				}
			}
		}

		public class ApplicationA
		{
			public SourceData sourceData;
			public PlanA.CmdFactory cmdFactory;

			public List<PlanA.CMD> cmdList = new();

			void Init()
			{
				sourceData = new SourceData();
				cmdFactory = new ImplA.cmdfactoryImpl();
				foreach (var item in sourceData.mData)
				{
					var cmd = cmdFactory.Create(item);
					if (cmd != null)
						cmdList.Add(cmd);
				}
			}

			void Excute()
			{
				foreach (var item in cmdList)
				{
					item.Excute();
				}
			}
		}

		public class PlanB
		{
			public abstract class Cmd
			{
				public abstract Arg CreateMyArg(string arg);
				public abstract void Excute(Arg arg);
			}

			public class Arg
			{
				public int Data;
			}
		}

		public class ImplementB
		{
			public class Cmd : PlanB.Cmd
			{
				public override PlanB.Arg CreateMyArg(string arg)
				{
					if (int.TryParse(arg, out int intger))
						return (new PlanB.Arg() { Data = intger });
					else return null;
				}

				public override void Excute(PlanB.Arg arg)
				{
					Debug.Log(arg.Data + 1);
				}
			}
		}

		public class ApplicationB
		{
			public SourceData sourceData;
			public PlanB.Cmd mCmd;
			public List<PlanB.Arg> mArgList = new();

			void Init()
			{
				sourceData = new SourceData();
				mCmd = new ImplementB.Cmd();
				foreach (var item in sourceData.mData)
				{
					var arg = mCmd.CreateMyArg(item);
					if (arg != null)
						mArgList.Add(arg);
				}
			}

			void Excute()
			{
				foreach (var item in mArgList)
				{
					mCmd.Excute(item);
				}
			}
		}
	}
}