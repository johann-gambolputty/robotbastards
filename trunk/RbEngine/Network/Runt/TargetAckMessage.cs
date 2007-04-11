using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// UpdateTarget acknowledgement
	/// </summary>
	public class TargetAckMessage : Components.Message
	{
		/// <summary>
		/// Target sequence
		/// </summary>
		public uint Sequence
		{
			get
			{
				return m_Sequence;
			}
		}

		/// <summary>
		/// Default constructor for serialisation
		/// </summary>
		public TargetAckMessage( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public TargetAckMessage( uint sequence )
		{
			m_Sequence = sequence;
		}

		/// <summary>
		/// Reads this message from a binary stream
		/// </summary>
		protected override void Read( System.IO.BinaryReader input )
		{
			base.Read( input );
			BinaryReaderHelpers.Read( input, out m_Sequence );
		}

		/// <summary>
		/// Writes this message to a binary stream
		/// </summary>
		public override void Write( System.IO.BinaryWriter output )
		{
			base.Write( output );
			output.Write( m_Sequence );
		}

		private uint m_Sequence;
	}
}
