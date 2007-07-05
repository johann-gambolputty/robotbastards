using System;
using Rb.Core.Components;

namespace Rb.Network.Runt
{
	/// <summary>
	/// Stores a sequence value used to synchronise a remote machine
	/// </summary>
	[Serializable]
	public class TargetSequenceMessage : Message, ISequenceMessage
	{
		#region	ISequenceMessage Members

		/// <summary>
		/// Target sequence
		/// </summary>
		public uint Sequence
		{
			get { return m_Sequence; }
		}

		#endregion

		/// <summary>
		/// Setup constructor
		/// </summary>
		public TargetSequenceMessage( uint sequence )
		{
			m_Sequence = sequence;
		}

		private uint m_Sequence;
	}
}
