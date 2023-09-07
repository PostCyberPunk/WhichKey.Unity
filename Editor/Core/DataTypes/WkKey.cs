using UnityEngine;

namespace PCP.Tools.WhichKey
{
    [System.Serializable]
    public struct WkKeySeq
    {
        [SerializeField]
        private int[] _keySeq;

        public int[] KeySeq
        {
            get => _keySeq == null ? _keySeq = new int[0] : _keySeq;
            set => _keySeq = value;
        }

        [SerializeField]
        private string _keyLabel;
        public string KeyLabel
        {
            get => _keySeq == null ? "None" : _keyLabel;
            set => _keyLabel = value;
        }
        

        public static implicit operator WkKeySeq(int[] keySeq) => new(keySeq);
        public WkKeySeq(int[] keySeq)
        {
            _keySeq = keySeq;
            if (keySeq.Length == 0)
                _keyLabel = "None";
            else
                _keyLabel = keySeq.ToLabel();
        }
    }
}
