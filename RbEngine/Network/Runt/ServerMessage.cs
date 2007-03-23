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
		/// Gets the base message attached to this server message
		/// </summary>
		public Components.Message BaseMessage
		{
			get
			{
				return m_BaseMessage;
			}
		}

		/// <summary>
		/// Sets up this message
		/// </summary>
		/// <param name="sequenceNumber">The server sequence number</param>
		/// <param name="baseMessage">The base message</param>
		public ServerMessage( uint sequence, int clientId, Components.Message baseMessage )
		{
			m_ClientId		= clientId;
			m_Sequence		= sequence;
			m_BaseMessage	= baseMessage;
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
            m_BaseMessage.Write( output );	
		}

		/// <summary>
		/// Reads a message from the specified stream
		/// </summary>
		/// <param name="input"> Input stream </param>
		protected override void	Read( System.IO.BinaryReader input )
		{
			m_ClientId		= input.ReadInt32( );
			m_Sequence		= input.ReadUInt32( );
			m_BaseMessage	= Components.Message.ReadMessage( input );
		}

		private int					m_ClientId;
		private uint				m_Sequence;
		private Components.Message	m_BaseMessage;
	}
}
