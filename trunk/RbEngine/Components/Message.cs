using System;

namespace RbEngine.Components
{

	/// <summary>
	/// Base class for messages
	/// </summary>
	public abstract class Message
	{
		/// <summary>
		/// Gets the type identifier of this message
		/// </summary>
		public abstract ushort	TypeId
		{
			get;
		}

		/// <summary>
		/// Creates a message from a type ID
		/// </summary>
		/// <param name="typeId"> Message type identifier </param>
		/// <returns> Returns a new message of the specified type </returns>
		public static Message	CreateFromTypeId( ushort typeId )
		{
			return null;
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
	}
}
