using System;
using RbEngine.Components;

namespace RbEngine.Network.Runt
{
	//	TODO: Update messages should batch up a set of child messages (like UpdateMessageBatch) - this is
	//	because sequential update messages tend to be sent to the same target, and have the same
	//	sequence number so it would be cheaper to store TargetId externally

	/// <summary>
	/// Base message class for update messages passed between client and server in ServerMessage objects
	/// </summary>
	public class UpdateMessage : Message
	{
		/// <summary>
		/// ID of the target of the message (updater ID)
		/// </summary>
		public ObjectId		TargetId
		{
			get
			{
				return m_TargetId;
			}
		}

		/// <summary>
		/// The paylod message, that actual contains useful shit
		/// </summary>
		public Message		Payload
		{
			get
			{
				return m_Payload;
			}
		}

		/// <summary>
		/// The sequence that this message was generated at
		/// </summary>
		public uint			Sequence
		{
			get
			{
				return m_Sequence;
			}
			set
			{
				m_Sequence = value;
			}
		}

		/// <summary>
		/// Default constructor. Required for serialisation
		/// </summary>
		public UpdateMessage( )
		{
		}

		/// <summary>
		/// Sets the target identifier
		/// </summary>
		public UpdateMessage( ObjectId targetId, uint sequence, Message payload )
		{
			m_TargetId	= targetId;
			m_Payload	= payload;
			m_Sequence	= sequence;
		}

		/// <summary>
		/// Reads this message
		/// </summary>
		protected override void Read( System.IO.BinaryReader input )
		{
			base.Read( input );

			ObjectId newId = new ObjectId( );
			BinaryReaderHelpers.Read( input, out newId.Id );
			m_TargetId = newId;

			BinaryReaderHelpers.Read( input, out m_Sequence );

			m_Payload = Message.ReadMessage( input );
		}

		/// <summary>
		/// Writes this message
		/// </summary>
		public override void Write( System.IO.BinaryWriter output )
		{
			base.Write( output );
			output.Write( TargetId.Id );
			output.Write( m_Sequence );
			m_Payload.Write( output );
		}

		private ObjectId	m_TargetId;
		private Message		m_Payload;
		private uint		m_Sequence;
	}
}
