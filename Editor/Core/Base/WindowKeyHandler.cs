using UnityEngine;

namespace PCP.Tools.WhichKey
{
    public abstract class WindowKeyHandler<T> where T : BaseWKWindow
    {
        protected T mWindow;
        public void ShowWindow()
        {
            if (mWindow == null)
            {
                mWindow = ScriptableObject.CreateInstance<T>();
            }
            mWindow.ShowPopup();
            mWindow.minSize = new Vector2(0, 0);
            mWindow.position = new Rect(0, 0, 0, 0);
        }
        public abstract void HandleKey(int key);
    }
}