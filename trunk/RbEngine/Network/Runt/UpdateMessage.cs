using System;
using RbEngine.Components;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Base message class for update messages passed between client and server in ServerMessage objects
	/// </summary>
	public class UpdateMessage : Components.Message
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
		/// Default constructor. Required for serialisation
		/// </summary>
		public UpdateMessage( )
		{
		}

		/// <summary>
		/// Sets the target identifier
		/// </summary>
		public UpdateMessage( ObjectId targetId )
		{
			m_TargetId = targetId;
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
		}

		/// <summary>
		/// Writes this message
		/// </summary>
		public override void Write( System.IO.BinaryWriter output )
		{
			base.Write( output );
			output.Write( TargetId.Id );
		}

		private ObjectId	m_TargetId;
	}
}
