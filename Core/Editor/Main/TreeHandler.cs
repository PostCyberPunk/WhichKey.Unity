using System.Linq;
using PCP.WhichKey.Types;
using PCP.WhichKey.Log;
using PCP.WhichKey.Utils;

namespace PCP.WhichKey.Core
{
	internal class TreeHandler : DepthKeyHandler<MainHintsWindow>, IWkHandler
	{
		private KeyNode mTreeRoot;
		private KeyNode mRoot;
		private KeyNode mCurrentNode;
		private IWkHandler mCurrentHandler;
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
				window.Close();
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
					window.Close();
					return;
				}
				try
				{
					cmd.Execute();
				}
				catch (System.Exception e)
				{
					WkLogger.LogError($"KeySeq {mKeyLabel} execute failed\n{e}");
				}
				if (cmd.isEnd) window.Close();
			}
		}

		protected override void UpdateWindow()
		{
			window.Hints = GetLayerHints();
			window.Title = GetLayerName();
		}

		public LayerHint[] GetLayerHints()
		{
			if (mCurrentHandler != this && mCurrentHandler != null)
				return mCurrentHandler.GetLayerHints();
			return mCurrentNode.LayerHints;
		}

		public string GetLayerName()
		{
			if (mCurrentHandler != this && mCurrentHandler != null)
				return mCurrentHandler.GetLayerName();
			var s = mCurrentNode.Hint;
			if (s == null)
				return "WhichKey";
			else
				return s;
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

		public void ChangeHandler(IWkHandler handler, int depth)
		{
			if (handler == null)
			{
				WkLogger.LogError("ChangeHandler handler is null");
				return;
			}

			mCurrentHandler = handler;
			if (depth > 0)
				maxDepth = mKeySeq.Count + depth;
			window.OverreideSets(handler.Timeout, handler.ColWidth);
			// if (handler is IWkWinModifier modifer)
			// {
			// 	if (mWindow == null)
			// 		WkLogger.LogError("ChangeHandler mWindow is null");
			// 	else
			// 		modifer.SetWindow(mWindow);
			// }
		}
	}
}