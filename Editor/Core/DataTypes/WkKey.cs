
namespace PCP.Tools.WhichKey
{
    [System.Serializable]
    public struct WkKeySeq
    {
        [UnityEngine.SerializeField]
        private int[] _keySeq;
        public int[] KeySeq => _keySeq == null ? _keySeq = new int[0] : _keySeq;
        public string KeyLabel;
        private void SetKeyLabel()
        {
            if (KeySeq.Length == 0)
                KeyLabel = "None";
            else
                KeyLabel = KeySeq.ToLabel();
        }
        public void Bind()
        {
            BindingWindow.ShowWindow((ks) =>
            {
                KeySeq = ks;
                SetKeyLabel();
            });
        }
    }
}
