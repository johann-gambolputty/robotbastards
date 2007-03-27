using System;

using MessageTypeId = System.UInt16;

namespace RbEngine.Components
{
	/// <summary>
	/// Base class for messages
	/// </summary>
	public abstract class Message
	{
		/// <summary>
		/// Gets the message type id associated with a message type
		/// </summary>
		public static MessageTypeId	IdFromType( Type messageType )
		{
			return ( MessageTypeId )( messageType.GetHashCode( ) );
		}

		/// <summary>
		/// Gets the type identifier of this message
		/// </summary>
		public MessageTypeId	TypeId
		{
			get
			{
				return IdFromType( GetType( ) );
			}
		}

		/// <summary>
		/// Creates a message from a type ID
		/// </summary>
		/// <param name="typeId"> Message type identifier </param>
		/// <returns> Returns a new message of the specified type </returns>
		public static Message	CreateFromTypeId( MessageTypeId typeId )
		{
			Type messageType = MessageTypeManager.Inst.GetMessageTypeFromId( typeId );
			return ( Message )Activator.CreateInstance( messageType );
		}

		/// <summary>
		/// Reads a message from a binary stream
		/// </summary>
		public static Message	CreateFromStream( System.IO.BinaryReader input )
		{
			Message msg = CreateFromTypeId( input.ReadUInt16( ) );
			msg.Read( input );
			return msg;
		}

		/// <summary>
		/// Writes a message to the specified output stream
		/// </summary>
		/// <param name="output">Output stream</param>
		/// <remarks>
		/// This implementation writes the message type ID to the stream
		/// </remarks>
		public virtual void		Write( System.IO.BinaryWriter output )
		{
			output.Write( TypeId );
		}

		/// <summary>
		/// Reads a message from the specified stream
		/// </summary>
		/// <param name="input"> Input stream </param>
		protected virtual void	Read( System.IO.BinaryReader input )
		{
		}

		/// <summary>
		/// Reads a message from the specified stream
		/// </summary>
		/// <param name="input">Input stream</param>
		/// <returns>Returns the new message</returns>
		public static Message	ReadMessage( System.IO.BinaryReader input )
		{
			Message msg = CreateFromTypeId( input.ReadUInt16( ) );
			msg.Read( input );
			return msg;
		}

		#region	Message recipient chain support

		/// <summary>
		/// Adds this message to a recipient list
		/// </summary>
		/// <param name="recipients">Recipient chain</param>
		public void					AddToRecipientChain( MessageRecipientChain recipients )
		{
			m_Recipients		= recipients;
			m_RecipientIndex	= 0;
		}

		/// <summary>
		/// Delivers this message to the next recipient in the recipient chain that it is attached to
		/// </summary>
		public void					DeliverToNextRecipient( )
		{
			if ( m_Recipients != null )
			{
				m_Recipients.DeliverToNext( ref m_RecipientIndex, this );
			}
		}

		/// <summary>
		/// Removes this message from a recipient list
		/// </summary>
		public void					RemoveFromRecipientChain( )
		{
			m_Recipients = null;
		}

		#endregion

		private MessageRecipientChain	m_Recipients;
		private int						m_RecipientIndex;
	}
}
