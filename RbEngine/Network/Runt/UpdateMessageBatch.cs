using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// A batch of update messages
	/// </summary>
	public class UpdateMessageBatch : Components.Message
	{
		/// <summary>
		/// Gets the sequence value of the local object
		/// </summary>
		public uint	Sequence
		{
			get
			{
				return m_Sequence;
			}
		}

		/// <summary>
		/// Gets the update message array
		/// </summary>
		public UpdateMessage[] Messages
		{
			get
			{
				return m_Messages;
			}
		}

		/// <summary>
		/// Default constructor, for serialisation
		/// </summary>
		public UpdateMessageBatch( )
		{
		}

		/// <summary>
		/// Sets up the message batch
		/// </summary>
		public UpdateMessageBatch( uint sequence, UpdateMessage[] updateMessages )
		{
			m_Sequence 			= sequence;
			m_Messages 			= updateMessages;
		}

		/// <summary>
		/// Reads the message batch from a binary input
		/// </summary>
		protected override void Read( System.IO.BinaryReader input )
		{
			base.Read( input );

			BinaryReaderHelpers.Read( input, out m_Sequence );

			int numMessages = input.ReadInt32( );
			if ( numMessages == 0 )
			{
				return;
			}
			m_Messages = new UpdateMessage[ numMessages ];
			for ( int messageCount = 0; messageCount < numMessages; ++messageCount )
			{
				m_Messages[ messageCount ] = ( UpdateMessage )Components.Message.ReadMessage( input );
			}
		}

		/// <summary>
		/// Writes this message batch to a binary output
		/// </summary>
		public override void Write( System.IO.BinaryWriter output )
		{
			base.Write( output );
			output.Write( m_Sequence );

			output.Write( m_Messages.Length );

			for ( int messageIndex = 0; messageIndex < m_Messages.Length; ++messageIndex )
			{
				m_Messages[ messageIndex ].Write( output );
			}
		}

		private uint			m_Sequence;
		private UpdateMessage[]	m_Messages;
	}

}
