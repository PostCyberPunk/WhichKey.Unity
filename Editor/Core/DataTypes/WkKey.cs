using UnityEngine;

namespace PCP.WhichKey.Types
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
        /// <summary>
        /// Last key of seq, 0 if empty
        /// </summary>
        public int lastKey => _keySeq.Length == 0 ? 0 : _keySeq[_keySeq.Length - 1];

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
        public WkKeySeq(int key) : this(new int[] { key })
        {
        }
    }
}
