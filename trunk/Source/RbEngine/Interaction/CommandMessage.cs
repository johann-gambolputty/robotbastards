using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Message sent by command input bindings as an alternative to events
	/// </summary>
	public class CommandMessage : Components.Message
	{
		/// <summary>
		/// Default constructor - for serialisation
		/// </summary>
		public CommandMessage( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public CommandMessage( Command cmd )
		{
			m_CommandId = checked( ( byte )cmd.Id );
		}


		/// <summary>
		/// Gets the ID of the command
		/// </summary>
		public int CommandId
		{
			get
			{
				return m_CommandId;
			}
		}

		/// <summary>
		/// Reads this message
		/// </summary>
		protected override void Read( System.IO.BinaryReader input )
		{
			base.Read( input );
			BinaryReaderHelpers.Read( input, out m_CommandId );
		}

		/// <summary>
		/// Writes this message
		/// </summary>
		public override void Write( System.IO.BinaryWriter output )
		{
			base.Write( output );
			output.Write( m_CommandId );
		}

		private byte m_CommandId;
	}
}
