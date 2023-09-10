using System.Linq;
using PCP.WhichKey.Types;
using PCP.WhichKey.Log;
using PCP.WhichKey.Utils;

namespace PCP.WhichKey.Core
{
	internal class TreeHandler : DepthKeyHandler<MainHintsWindow>, IWKHandler
	{
		private KeyNode mTreeRoot;
		private KeyNode mRoot;
		private KeyNode mCurrentNode;
		private IWKHandler mCurrentHandler;
		private string mKeyLabel => mKeySeq.Reverse().ToArray().ToLabel();

		public TreeHandler(KeyNode root)
		{
			mTreeRoot = root;
			Refesh();
		}

		public void Refesh()
		{
			mKeySeq = new();
			ResetRoot();
			Reset();
		}

		public override void OnActive()
		{
			Reset();
			UpdateWindow();
		}

		protected override void HandleKeyWithDepth(int key)
		{
			if (mCurrentHandler != this && mCurrentHandler != null)
			{
				mCurrentHandler.ProcessKey(key);
				return;
			}

			ProcessKey(key);
		}

		public void ProcessKey(int Key)
		{
			var kn = mCurrentNode.GetChildByKey(Key);
			if (kn == null)
			{
				WkLogger.LogInfo($"KeySeq {mKeyLabel} not found");
				mWindow.Close();
			}
			else if (kn.Type == 0)
			{
				mCurrentNode = kn;
				return;
			}
			else
			{
				var cmd = kn.Command;
				if (cmd == null)
				{
					WkLogger.LogError($"KeySeq {mKeyLabel} has no command");
					mWindow.Close();
					return;
				}

				cmd.Execute();
				if (cmd.isEnd) mWindow.Close();
			}
		}

		protected override void UpdateWindow()
		{
			mWindow.Hints = GetLayerHints();
		}

		public string[] GetLayerHints()
		{
			if (mCurrentHandler != this && mCurrentHandler != null)
				return mCurrentHandler.GetLayerHints();
			return mCurrentNode.LayerHints;
		}

		#region TreeManupulation

		public void Reset()
		{
			mKeySeq.Clear();
			mCurrentHandler = this;
			mCurrentNode = mRoot;
			maxDepth = -1;
		}

		public void Reset(int[] key)
		{
			Reset();
			mCurrentNode = GetKeyNodebyKeySeq(key);
		}

		public void ResetRoot()
		{
			mRoot = mTreeRoot;
		}

		public void ChagneRoot(int[] key)
		{
			if (key == null)
			{
				ResetRoot();
				return;
			}

			var kn = GetKeyNodebyKeySeq(key);
			if (kn == null) return;
			if (kn.Type != 0)
			{
				WkLogger.LogWarning($"Change root failed ,KeySeq {mKeyLabel} not a layer");
				return;
			}

			mRoot = kn;
			WkLogger.LogInfo($"Change root to {key.ToLabel()}");
		}

		private KeyNode GetKeyNodebyKeySeq(int[] key)
		{
			if (key.Length == 0)
			{
				ResetRoot();
				return null;
			}

			KeyNode kn = mTreeRoot;

			for (int i = 0; i < key.Length; i++)
			{
				kn = mCurrentNode.GetChildByKey(key[i]);
				if (kn == null)
				{
					WkLogger.LogWarning($"KeySeq {mKeyLabel} not found @key {key[i].ToLabel()}");
					return null;
				}
			}

			return kn;
		}

		#endregion TreeManupulation

		public void ChangeHandler(IWKHandler handler, int depth)
		{
			if (handler == null)
			{
				WkLogger.LogError("ChangeHandler handler is null");
				return;
			}

			mCurrentHandler = handler;
			maxDepth = mKeySeq.Count + depth;
		}

		public void OverrideTimeout(float time) => mWindow.OverrideTimeout(time);
	}
}