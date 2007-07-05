using System;
using Rb.Core.Components;

namespace Rb.Network.Runt
{
	/// <summary>
	/// A batch of update messages
	/// </summary>
	[Serializable]
	public class UpdateMessageBatch : Message, ISequenceMessage
	{
		#region	ISequenceMessage Members

		/// <summary>
		/// Gets the sequence value of the local object
		/// </summary>
		public uint	Sequence
		{
			get { return m_Sequence; }
		}

		#endregion

		/// <summary>
		/// Gets the update message array
		/// </summary>
		public UpdateMessage[] Messages
		{
			get { return m_Messages; }
		}

		/// <summary>
		/// Sets up the message batch
		/// </summary>
		public UpdateMessageBatch( uint sequence, UpdateMessage[] updateMessages )
		{
			m_Sequence	= sequence;
			m_Messages	= updateMessages;
		}

		private uint			m_Sequence;
		private UpdateMessage[]	m_Messages;
	}

}
