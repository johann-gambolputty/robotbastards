using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// A message sent from a server to a client
	/// </summary>
	public class ClientMessage : Components.Message
	{
		/// <summary>
		/// Default constructor, for serialisation
		/// </summary>
		public ClientMessage( )
		{
		}

		/// <summary>
		/// Sets up this client message
		/// </summary>
		public ClientMessage( uint sequence, UpdateMessage[] updateMessages )
		{
			m_Sequence			= sequence;
			m_UpdateMessages	= updateMessages;
		}

		/// <summary>
		/// Reads this message
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

			m_UpdateMessages = new UpdateMessage[ numMessages ];
			for ( int messageCount = 0; messageCount < numMessages; ++messageCount )
			{
				m_UpdateMessages[ messageCount ] = ( UpdateMessage )Components.Message.ReadMessage( input );
			}
		}

		/// <summary>
		/// Writes this message
		/// </summary>
		public override void Write( System.IO.BinaryWriter output )
		{
			base.Write( output );
			output.Write( m_Sequence );

			output.Write( m_UpdateMessages.Length );

			for ( int msgIndex = 0; msgIndex < m_UpdateMessages.Length; ++msgIndex )
			{
				m_UpdateMessages[ msgIndex ].Write( output );
			}
		}



		private uint			m_Sequence;
		private UpdateMessage[] m_UpdateMessages;
	}
}
