using System;
using RbEngine.Components;

namespace RbEngine.Network.Runt
{
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
		/// Default constructor. Required for serialisation
		/// </summary>
		public UpdateMessage( )
		{
		}

		/// <summary>
		/// Sets the target identifier
		/// </summary>
		public UpdateMessage( ObjectId targetId, Message payload )
		{
			m_TargetId	= targetId;
			m_Payload	= payload;
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

			m_Payload = Message.ReadMessage( input );
		}

		/// <summary>
		/// Writes this message
		/// </summary>
		public override void Write( System.IO.BinaryWriter output )
		{
			base.Write( output );
			output.Write( TargetId.Id );
			m_Payload.Write( output );
		}

		private ObjectId	m_TargetId;
		private Message		m_Payload;
	}
}
