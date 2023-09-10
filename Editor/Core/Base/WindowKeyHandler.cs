using UnityEngine;

namespace PCP.Tools.WhichKey
{
    public abstract class WindowKeyHandler<T>:BaseKeyHandler where T : WkBaseWindow
    {
        protected T mWindow;
        public override void ShowWindow()
        {
            if (mWindow == null)
            {
                mWindow = ScriptableObject.CreateInstance<T>();
            }
            mWindow.OnActive();
            mWindow.UpdateDelayTimer();
            
            mWindow.minSize = new Vector2(0, 0);
            mWindow.position = new Rect(0, 0, 0, 0);

            mWindow.ShowPopup();
        }
        public override void CloseWindow()
        {
            mWindow.Close();
        }
    }
}