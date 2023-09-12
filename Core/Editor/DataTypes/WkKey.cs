using UnityEngine;
using PCP.WhichKey.Utils;
using System;

namespace PCP.WhichKey.Types
{
	[Serializable]
	public struct WkKeySeq : IEquatable<WkKeySeq>, IEquatable<int>
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
		public WkKeySeq(int key) : this(new int[] { key }) { }
		public WkKeySeq(int[] keySeq)
		{
			_keySeq = keySeq;
			if (keySeq.Length == 0)
				_keyLabel = "None";
			else
				_keyLabel = keySeq.ToLabel();
		}


		//Equals
		//Operator
		public static bool operator ==(WkKeySeq seq, int key) => Equals(seq, key);
		public static bool operator !=(WkKeySeq seq, int key) => !Equals(seq, key);
		public static bool operator ==(WkKeySeq lhs, WkKeySeq rhs) => lhs.Equals(rhs);
		public static bool operator !=(WkKeySeq lhs, WkKeySeq rhs) => !(lhs == rhs);

		public override int GetHashCode() => KeySeq.GetHashCode();
		public override bool Equals(object other) => other is WkKeySeq seq && Equals(seq) || other is int key && Equals(key);
		public bool Equals(int other) => KeySeq.Length == 1 && KeySeq[0] == other;
		public bool Equals(WkKeySeq other)
		{
			if (KeySeq.Length != other.KeySeq.Length)
				return false;
			else
			{
				for (int i = 0; i < this.KeySeq.Length; i++)
				{
					if (KeySeq[i] != other.KeySeq[i])
						return false;
				}
				return true;
			}
		}
	}
}