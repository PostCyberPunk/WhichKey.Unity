namespace PCP.Tools.WhichKey
{
	internal class SceneNavCmd : ChangeHandlerCmd
	{
		public override IWKHandler Handler => mSceneHandler;
		public override bool isEnd => false;
		public override int Depth => 1;
		private readonly static SceneNavHandler mSceneHandler = new();
		private bool set = false;
		public SceneNavCmd(bool isSst) => set = isEnd;
		protected override void ActiveHandler() => mSceneHandler.Set = set;
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