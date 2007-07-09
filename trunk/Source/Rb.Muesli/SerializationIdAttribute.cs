using System;

namespace Rb.Muesli
{
	/// <summary>
	/// Attribute used to tag a type with a small unique identifier. If a type has this attribute, the ID is
	/// used when serializing references to the type, rather than the full assembly qualified type name
	/// </summary>
	public class SerializationIdAttribute : Attribute
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="id">Unique type id</param>
		public SerializationIdAttribute( uint id )
		{
			m_Id = id;
		}

		/// <summary>
		/// ID getter
		/// </summary>
		public uint Id
		{
			get { return m_Id; }
		}

		/// <summary>
		/// Returned by GetTypeId() if a type has no SerializationIdAttribute
		/// </summary>
		public const uint NoId = unchecked( ( uint )-1 );

		/// <summary>
		/// Helper method that returns the ID of a type tagged with the SerializationIdAttribute
		/// </summary>
		public static uint GetTypeId( Type type )
		{
			SerializationIdAttribute[] attribs = ( SerializationIdAttribute[] )type.GetCustomAttributes( typeof( SerializationIdAttribute ), false );
			return attribs.Length > 0 ? attribs[0].Id : NoId;
		}

		private uint m_Id;
	}
}
