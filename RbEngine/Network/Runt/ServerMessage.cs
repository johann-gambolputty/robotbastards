using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// A message sent to the server, with some extra piggybacked data for the RUNT model
	/// </summary>
	public class ServerMessage : Components.Message
	{
		/// <summary>
		/// Gets the client ID
		/// </summary>
		public int	ClientId
		{
			get
			{
				return m_ClientId;
			}
		}

		/// <summary>
		/// Gets the sequence number of this message
		/// </summary>
		public uint Sequence
		{
			get
			{
				return m_Sequence;
			}
		}

		/// <summary>
		/// Gets the base update messages attached to this server message
		/// </summary>
		public UpdateMessage[] UpdateMessages
		{
			get
			{
				return m_UpdateMessages;
			}
		}

		/// <summary>
		/// Default constructor. Required for serialisation
		/// </summary>
		public ServerMessage( )
		{
		}

		/// <summary>
		/// Sets up this message
		/// </summary>
		/// <param name="sequenceNumber">The server sequence number</param>
		/// <param name="baseMessage">The base message</param>
		public ServerMessage( uint sequence, int clientId, UpdateMessage[] updateMessages )
		{
			m_ClientId			= clientId;
			m_Sequence			= sequence;
			m_UpdateMessages	= updateMessages;
		}
		
		/// <summary>
		/// Writes a message to the specified output stream
		/// </summary>
		/// <param name="output">Output stream</param>
		public override void		Write( System.IO.BinaryWriter output )
		{
			base.Write( output );

			output.Write( m_ClientId );
			output.Write( m_Sequence );
			output.Write( m_UpdateMessages.Length );

			for ( int msgIndex = 0; msgIndex < m_UpdateMessages.Length; ++msgIndex )
			{
				m_UpdateMessages[ msgIndex ].Write( output );	
			}
		}

		/// <summary>
		/// Reads a message from the specified stream
		/// </summary>
		/// <param name="input"> Input stream </param>
		protected override void	Read( System.IO.BinaryReader input )
		{
			m_ClientId		= input.ReadInt32( );
			m_Sequence		= input.ReadUInt32( );

			int numMessages = input.ReadInt32( );
			m_UpdateMessages	= new UpdateMessage[ numMessages ];

			for ( int msgIndex = 0; msgIndex < numMessages; ++msgIndex )
			{
				m_UpdateMessages[ msgIndex ] = ( UpdateMessage )Components.Message.ReadMessage( input );
			}
		}

		private int				m_ClientId;
		private uint			m_Sequence;
		private UpdateMessage[]	m_UpdateMessages;
	}
}
