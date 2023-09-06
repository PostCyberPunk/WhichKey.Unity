using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCP.Tools.WhichKey
{
    public class TreeHandler : IWKHandler
    {
        private readonly TreeBuilder mTreeBuilder = new TreeBuilder();
        private Stack<int> mKeySeq;
        private KeyNode mTreeRoot => mTreeBuilder.TreeRoot;
        private KeyNode mRoot;
        private KeyNode mCurrentNode;

        private IWKHandler mCurrentHandler;
        void Refesh()
        {
            mTreeBuilder.Build();
            Reset();
        }

        public void ProcesRawKey(KeyCode keyCode, bool shift)
        {
            int key = keyCode.ToAscii(shift);
            //TODO: Add support for backspace?
            if (key != 0)
                mCurrentHandler.ProcessKey(key);
        }
        public void ProcessKey(int Key)
        {
            mKeySeq.Push(Key);
            var kn = mCurrentNode.GetChildByKey(Key);
            if (kn == null)
            {
                WhichKeyManager.LogInfo($"KeySeq {mKeySeq.ToArray().ToLabel()} not found");
                CloseWindow();
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
                    WhichKeyManager.LogError($"KeySeq {mKeySeq.ToArray().ToLabel()} has no command");
                    CloseWindow();
                    return;
                }
                cmd.Execute();
                if (cmd.isEnd) CloseWindow();
            }
        }
        private void CloseWindow()
        {
            WhichKeyManager.instance.CloseHintsWindow();
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
                WhichKeyManager.LogWarning($"Change root failed ,KeySeq {mKeySeq.ToArray().ToLabel()} not a layer");
                return;
            }
            mRoot = kn;
            WhichKeyManager.LogInfo($"Change root to {key.ToLabel()}");
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
                    WhichKeyManager.LogWarning($"KeySeq {mKeySeq.ToArray().ToLabel()} not found @key {key[i].ToLabel()}");
                    return null;
                }
            }
            return kn;
        }

        #endregion TreeManupulation
    }
}
