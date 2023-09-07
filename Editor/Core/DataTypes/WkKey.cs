
namespace PCP.Tools.WhichKey
{
    [System.Serializable]
    public struct WkKey
    {
        [UnityEngine.SerializeField]
        private int[] _keySeq;

        public int[] KeySeq
        {
            get => _keySeq == null ? _keySeq = new int[0] : _keySeq;
            set => _keySeq = value;
        }

        //FIXME: how can i make this only change by exention?
        private string _keyLabel;
        public string KeyLabel
        {
            get => _keyLabel;
            internal set => _keyLabel = value;
        }
        public WkKey(int[] keySeq)
        {
            _keySeq = keySeq;
            _keyLabel = keySeq.ToLabel();
        }
    }
    public static class WkKeyExtension
    {
        public static void SetLabel(ref this WkKey wkKey)
        {
            if (wkKey.KeySeq.Length == 0)
                wkKey.KeyLabel = "None";
            else
                wkKey.KeyLabel = wkKey.KeySeq.ToLabel();
        }
        public static void Bind(this WkKey wkKey, float depth = -1, string title = "WhichKey Binding")
        {
            BindingWindow.ShowWindow((ks) =>
            {
                wkKey.KeySeq = ks;
                wkKey.SetLabel();
            });
        }
    }
}
