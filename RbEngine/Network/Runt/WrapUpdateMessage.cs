using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// An UpdateMessage that wraps up a child message
	/// </summary>
	public class WrapUpdateMessage : UpdateMessage
	{
		/// <summary>
		/// Gets the wrapped message
		/// </summary>
		public Components.Message BaseMessage
		{
			get
			{
				return m_Message;
			}
		}

		/// <summary>
		/// Default constructor. Required for serialisation
		/// </summary>
		public WrapUpdateMessage( )
		{
		}

		/// <summary>
		/// Sets the wrapped message, and the target identifier
		/// </summary>
		public WrapUpdateMessage( Components.ObjectId targetId, Components.Message msg ) :
			base( targetId )
		{
			m_Message = msg;
		}

		/// <summary>
		/// Reads this message
		/// </summary>
		protected override void Read( System.IO.BinaryReader input )
		{
			base.Read( input );
			m_Message = Components.Message.ReadMessage( input );
		}

		/// <summary>
		/// Writes this message
		/// </summary>
		public override void Write( System.IO.BinaryWriter output )
		{
			base.Write( output );
			m_Message.Write( output );
		}

		private Components.Message m_Message;
	}
}
