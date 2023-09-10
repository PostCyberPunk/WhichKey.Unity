namespace PCP.WhichKey
{
    public abstract class BaseKeyHandler
    {
        public abstract void ShowWindow();
        public abstract void OnActive();
        public abstract void CloseWindow();
        public abstract void HandleKey(int key);
    }
}