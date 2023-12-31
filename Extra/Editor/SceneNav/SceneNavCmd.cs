using PCP.WhichKey.Types;

namespace PCP.WhichKey.Extra
{
	internal class SceneNavCmd : ChangeHandlerCmd
	{
		public override IWkHandler Handler => mSceneHandler;
		public override int Depth => 1;
		private readonly static SceneNavHandler mSceneHandler = new();
		private bool set;
		public SceneNavCmd(bool isSst) => set = isSst;
		protected override void OnActive() => mSceneHandler.Set = set;
	}
	internal class SceneNavLoadCmdFactory : WKCommandFactory

	{
		public override int TID => 13;
		public override string CommandName => "SceneNav";
		public override WKCommand CreateCommand(string arg) => new SceneNavCmd(false);
	}
	internal class SceneNavSetCmdFactory : WKCommandFactory
	{
		public override int TID => 14;
		public override string CommandName => "SceneNavSet";
		public override WKCommand CreateCommand(string arg) => new SceneNavCmd(true);
	}
}