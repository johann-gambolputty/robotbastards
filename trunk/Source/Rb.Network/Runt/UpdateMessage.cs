using System;
using Rb.Core.Components;

namespace Rb.Network.Runt
{
	//	TODO: Update messages should batch up a set of child messages (like UpdateMessageBatch) - this is
	//	because sequential update messages tend to be sent to the same target, and have the same
	//	sequence number so it would be cheaper to store TargetId externally

	/// <summary>
	/// Base message class for update messages passed between client and server in ServerMessage objects
	/// </summary>
	[Serializable]
	public class UpdateMessage : Message
	{
		/// <summary>
		/// ID of the target of the message (updater ID)
		/// </summary>
		public Guid TargetId
		{
			get { return m_TargetId; }
			set { m_TargetId = value; }
		}

		/// <summary>
		/// The paylod message, that actual contains useful shit
		/// </summary>
		public Message Payload
		{
			get { return m_Payload; }
			set { m_Payload = value; }
		}

		/// <summary>
		/// The sequence that this message was generated at
		/// </summary>
		public uint Sequence
		{
			get { return m_Sequence; }
			set { m_Sequence = value; }
		}

		/// <summary>
		/// Sets the target identifier
		/// </summary>
		public UpdateMessage( Guid targetId, uint sequence, Message payload )
		{
			m_TargetId	= targetId;
			m_Payload	= payload;
			m_Sequence	= sequence;
		}

		private Guid		m_TargetId;
		private Message		m_Payload;
		private uint		m_Sequence;
	}
}
