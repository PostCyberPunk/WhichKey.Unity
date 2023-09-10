using System.Collections.Generic;

namespace PCP.WhichKey
{
    public abstract class DepthKeyHandler<T> : WindowKeyHandler<T> where T : WkBaseWindow
    {
        protected Stack<int> mKeySeq;
        protected int maxDepth = -1;
        public override sealed void HandleKey(int key)
        {
            mKeySeq.Push(key);
            HandleKeyWithDepth(key);
            if (maxDepth > 0 && CheckDepth())
                mWindow.Close();
            else
            {
                UpdateWindow();
                mWindow.UpdateHints();
            }
        }
        protected abstract void HandleKeyWithDepth(int key);
        private bool CheckDepth()
        {
            if (mKeySeq.Count >= maxDepth) return true;
            return false;
        }
        protected abstract void UpdateWindow();
    }
}