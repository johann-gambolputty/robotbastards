
using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using Rb.Core.Maths;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Legal types for elements making up vertex fields
	/// </summary>
	public enum VertexFieldElementTypeId
	{
		Byte,
		Float32,
		Int32,
		UInt32
	}

	/// <summary>
	/// Describes the format of a vertex buffer
	/// </summary>
	public class VertexBufferFormat
	{
		#region VertexFieldElementTypeId Helpers

		/// <summary>
		/// Converts a type into an <see cref="VertexFieldElementTypeId"/> value
		/// </summary>
		/// <param name="t">Type to convert</param>
		/// <returns>Returns the ElementType value that represents the specified type</returns>
		public static VertexFieldElementTypeId GetElementTypeId( Type t )
		{
			if ( t == typeof( byte ) )
			{
				return VertexFieldElementTypeId.Byte;
			}
			if ( t == typeof( float ) )
			{
				return VertexFieldElementTypeId.Float32;
			}
			if ( t == typeof( int ) )
			{
				return VertexFieldElementTypeId.Int32;
			}
			if ( t == typeof( uint ) )
			{
				return VertexFieldElementTypeId.UInt32;
			}
			throw new NotImplementedException( string.Format( "No conversion between type \"{0}\" and ElementTypeId value", t.Name ) );
		}

		/// <summary>
		/// Converts an <see cref="VertexFieldElementTypeId"/> into a type
		/// </summary>
		public static Type GetElementType( VertexFieldElementTypeId id )
		{
			switch ( id )
			{
				case VertexFieldElementTypeId.Byte: return typeof( byte );
				case VertexFieldElementTypeId.Int32: return typeof( int );
				case VertexFieldElementTypeId.UInt32: return typeof( uint );
				case VertexFieldElementTypeId.Float32: return typeof( float );
			}
			throw new NotImplementedException( string.Format( "No conversion between ElementTypeId  \"{0}\" and type ", id ) );
		}

		/// <summary>
		/// Gets the size, in bytes, of a given element type
		/// </summary>
		public static int GetElementTypeSize( VertexFieldElementTypeId elementType )
		{
			switch ( elementType )
			{
				case VertexFieldElementTypeId.Byte		: return 1;
				case VertexFieldElementTypeId.Float32	: return 4;
				case VertexFieldElementTypeId.Int32		: return 4;
				case VertexFieldElementTypeId.UInt32	: return 4;
			}

			throw new NotImplementedException( string.Format( "GetElementTypeSize({0}) not implemented", elementType ) );
		}

		#endregion

		#region FieldDescriptor Public Class

		/// <summary>
		/// Describes a field in the format
		/// </summary>
		[Serializable]
		public class FieldDescriptor
		{
			/// <summary>
			/// Sets up this field descriptor
			/// </summary>
			/// <param name="field">Vertex field semantic</param>
			/// <param name="fieldType">Field type</param>
			public FieldDescriptor( VertexFieldSemantic field, Type fieldType )
			{
				m_Field = field;
				if ( ( fieldType == typeof( Point3 ) ) || ( fieldType == typeof( Vector3 ) ) )
				{
					m_ElementType = VertexFieldElementTypeId.Float32;
					m_NumElements = 3;
				}
				else if ( ( fieldType == typeof( Point2 ) ) || ( fieldType == typeof( Vector2 ) ) )
				{
					m_ElementType = VertexFieldElementTypeId.Float32;
					m_NumElements = 2;
				}
				else if ( fieldType == typeof( Color ) )
				{
				//	m_ElementType = VertexFieldElementTypeId.Byte;	//	TODO: AP: Colour to byte array is not working
					m_ElementType = VertexFieldElementTypeId.Float32;
					m_NumElements = 4;
				}
				else if ( fieldType == typeof( float ) )
				{
					m_ElementType = VertexFieldElementTypeId.Float32;
					m_NumElements = 1;
				}
				else if ( fieldType == typeof( int ) )
				{
					m_ElementType = VertexFieldElementTypeId.Int32;
					m_NumElements = 1;
				}
			}

			/// <summary>
			/// Sets up this field descriptor
			/// </summary>
			/// <param name="field">Vertex field semantic</param>
			/// <param name="elementType">Field element type</param>
			/// <param name="numElements">Number of elements making up the field</param>
			public FieldDescriptor( VertexFieldSemantic field, VertexFieldElementTypeId elementType, int numElements )
			{
				m_Field = field;
				m_ElementType = elementType;
				m_NumElements = numElements;
			}

			/// <summary>
			/// Gets the size of this field in bytes
			/// </summary>
			public int FieldSizeInBytes
			{
				get
				{
					return GetElementTypeSize( m_ElementType ) * m_NumElements;
				}
			}

			/// <summary>
			/// Gets the field semantic
			/// </summary>
			public VertexFieldSemantic Field
			{
				get { return m_Field; }
			}

			/// <summary>
			/// Gets the type of elements making up this field
			/// </summary>
			public VertexFieldElementTypeId ElementType
			{
				get { return m_ElementType; }
			}

			/// <summary>
			/// Gets the number of elements in this field
			/// </summary>
			public int NumElements
			{
				get { return m_NumElements; }
			}

			#region Private Members

			private readonly VertexFieldSemantic m_Field;
			private readonly VertexFieldElementTypeId m_ElementType;
			private readonly int m_NumElements;

			#endregion
		}

		#endregion

		/// <summary>
		/// Creates a VertexBufferFormat instance by analyzing the makeup of a vertex class
		/// </summary>
		/// <typeparam name="T">
		/// Vertex class. Any fields tagged with the <see cref="VertexFieldAttribute"/> get added
		/// to the format</typeparam>
		public static VertexBufferFormat FromVertexClass< T >( )
		{
			VertexBufferFormat format = new VertexBufferFormat( );

			//	Run through all the fields in type T
			foreach ( FieldInfo field in typeof( T ).GetFields( BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic ) )
			{
				//	Check if the current field has a VertexField attribute
				VertexFieldAttribute[] attributes = ( VertexFieldAttribute[] )field.GetCustomAttributes( typeof( VertexFieldAttribute ), false );
				if ( attributes.Length == 0 )
				{
					continue;
				}
				format.Add( new FieldDescriptor( attributes[ 0 ].Semantic, field.FieldType ) );
			}
			try
			{
				format.ValidateFormat( );
			}
			catch ( Exception ex )
			{
				throw new ArgumentException( "Failed to build vertex buffer format from vertex class " + typeof( T ), ex );
			}

			return format;
		}

		/// <summary>
		/// Checks if this format is valid. If not, a <see cref="FormatException"/> is thrown
		/// </summary>
		public void ValidateFormat( )
		{
			if ( m_NumFields == 0 )
			{
				throw new FormatException( "Vertex format contained no vertex fields" );
			}
			if ( GetDescriptor( VertexFieldSemantic.Position ) == null )
			{
				throw new FormatException( "Vertex format contained no position field" );
			}
		}

		/// <summary>
		/// Gets the number of fields making up this format
		/// </summary>
		public int NumFields
		{
			get { return m_NumFields; }
		}

		/// <summary>
		/// Gets the size (in bytes) of a vertex created using this format
		/// </summary>
		public int VertexSize
		{
			get { return m_VertexSize; }
		}

		/// <summary>
		/// Static vertex buffers are very slow to lock, but faster to render. By default, Static is false
		/// </summary>
		public bool Static
		{
			get { return m_Static; }
			set { m_Static = value; }
		}

		/// <summary>
		/// Gets the list of field descriptors in this vertex format
		/// </summary>
		public IEnumerable< FieldDescriptor > FieldDescriptors
		{
			get
			{
				for ( int index = 0; index < m_Descriptors.Length; ++index )
				{
					if ( m_Descriptors[ index ] != null )
					{
						yield return m_Descriptors[ index ];
					}
				}
			}
		}

		/// <summary>
		/// Gets a field descriptor for a given semantic
		/// </summary> 
		public FieldDescriptor GetDescriptor( VertexFieldSemantic field )
		{
			return ( m_Descriptors[ ( int )field ] );
		}

		/// <summary>
		/// Checks for support of a given vertex field
		/// </summary>
		/// <param name="field">Field to check support for</param>
		/// <returns>true if field is supported</returns>
		public bool SupportsField( VertexFieldSemantic field )
		{
			return m_Descriptors[ ( int )field ] != null;
		}

		/// <summary>
		/// Converts this vertex buffer format to a string
		/// </summary>
		public override string ToString( )
		{
			StringBuilder str = new StringBuilder( );

			foreach ( FieldDescriptor descriptor in FieldDescriptors )
			{
				if ( str.Length > 0 )
				{
					str.Append( ',' );
				}
				str.AppendFormat( "[{0}:{1}x{2}]", descriptor.Field, descriptor.NumElements, descriptor.ElementType );
			}

			return str.ToString( );
		}

		/// <summary>
		/// Adds a field descriptor to the format
		/// </summary>
		/// <param name="fieldDescriptor">Field description</param>
		public void Add( FieldDescriptor fieldDescriptor )
		{
			int index = ( int )fieldDescriptor.Field;
			if ( m_Descriptors[ index ] != null )
			{
				throw new ArgumentException( string.Format( "Descriptor for field {0} has already been added", fieldDescriptor.Field ), "fieldDescriptor" );
			}
			++m_NumFields;
			m_VertexSize += fieldDescriptor.FieldSizeInBytes;
			m_Descriptors[ index ] = fieldDescriptor;
		}

		/// <summary>
		/// Adds a new field descriptor to the format
		/// </summary>
		/// <param name="field">Field semantic</param>
		/// <param name="elementType">Type of the elements making up the field</param>
		/// <param name="numElements">Number of elements in the field</param>
		public FieldDescriptor Add( VertexFieldSemantic field, VertexFieldElementTypeId elementType, int numElements )
		{
			FieldDescriptor desc = new FieldDescriptor( field, elementType, numElements );
			Add( desc );
			return desc;
		}

		/// <summary>
		/// Adds a new field descriptor to the format
		/// </summary>
		/// <typeparam name="T">Element type</typeparam>
		/// <param name="field">Field semantic</param>
		/// <param name="numElements">Number of elements in the field</param>
		public FieldDescriptor Add<T>( VertexFieldSemantic field, int numElements )
		{
			FieldDescriptor desc = new FieldDescriptor( field, GetElementTypeId( typeof( T ) ), numElements );
			Add( desc );
			return desc;
		}

		#region Private Members

		private bool m_Static;
		private int m_VertexSize;
		private int m_NumFields;
		private readonly FieldDescriptor[] m_Descriptors = new FieldDescriptor[ Enum.GetValues( typeof( VertexFieldSemantic ) ).Length ];

		#endregion
	}
}
