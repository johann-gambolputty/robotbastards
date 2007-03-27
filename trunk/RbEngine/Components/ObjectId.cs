using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Class for uniquely identifying an IIdentifiedObject
	/// </summary>
	public class ObjectId
	{
		/// <summary>
		/// Automatically generates the ID
		/// </summary>
		public ObjectId( )
		{
			Id = --ms_UniqueId;
		}

		/// <summary>
		/// Sets the ID
		/// </summary>
		public ObjectId( int id )
		{
			Id = id;
		}

		/// <summary>
		/// Identifier value
		/// </summary>
		/// <remarks>
		/// This is a public field, and not a private field with property, because it's helpful to be able to "ref" or "out" qualify.
		/// </remarks>
		public int Id;

		/// <summary>
		/// Gets the ID as a string
		/// </summary>
		public override string ToString()
		{
			return "0x" + Id.ToString( "x" );
		}

		/// <summary>
		/// Gets the hash code for the ID
		/// </summary>
		public override int GetHashCode( )
		{
			return Id;
		}

		/// <summary>
		/// Returns true if this ID is equal to another
		/// </summary>
		public override bool Equals( object obj )
		{
			return Id == ( ( ObjectId )obj ).Id;
		}


		/// <summary>
		/// Parses an object identifier
		/// </summary>
		public static ObjectId Parse( string str )
		{
			return new ObjectId( int.Parse( str ) );
		}


		private static int	ms_UniqueId;
	}
}
