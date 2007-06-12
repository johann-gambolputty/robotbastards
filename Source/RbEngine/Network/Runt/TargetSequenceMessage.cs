using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Summary description for TargetSequenceMessage.
	/// </summary>
	public class TargetSequenceMessage : Components.Message, ISequenceMessage
	{
		#region	ISequenceMessage Members

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

		#endregion

		/// <summary>
		/// Default constructor for serialisation
		/// </summary>
		public TargetSequenceMessage( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public TargetSequenceMessage( uint sequence )
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
