using System;
using System.Collections.Generic;
using System.Reflection;
using Rb.Core.Maths;

namespace Rb.Rendering
{
	/// <summary>
	/// Vertex field tags
	/// </summary>
	public enum VertexField
	{
		Position,
		Normal,
		Diffuse,
		Specular,
		Blend0,
		Blend1,
		Blend2,
		Blend3,
		Texture0,
		Texture1,
		Texture2,
		Texture3,
		Texture4,
		Texture5,
		Texture6,
		Texture7
	}

	/// <summary>
	/// Marks a class field as a vertex field
	/// </summary>
	public class VertexFieldAttribute : Attribute
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="field">The vertex field</param>
		public VertexFieldAttribute( VertexField field )
		{
			m_Field = field;
		}

		/// <summary>
		/// Gets the associated vertex field
		/// </summary>
		public VertexField VertexField
		{
			get { return m_Field; }
		}

		private readonly VertexField m_Field;
	}

	/// <summary>
	/// Data used for constructing a vertex buffer
	/// </summary>
	[Serializable]
	public class VertexBufferData
	{
		/// <summary>
		/// Types supported by the field <see cref="FieldValues.Interleave"/> method
		/// </summary>
		public enum ElementTypeId
		{
			Byte,
			Float32,
			UInt32,
		}

		#region Construction

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="numVertices">Number of vertices that the buffer can store</param>
		public VertexBufferData( int numVertices )
		{
			m_NumVertices = numVertices;
			m_FieldArrays = new FieldValues[ Enum.GetValues( typeof( VertexField ) ).Length ];
		}

		/// <summary>
		/// Creates vertex data from an array of vertex objects
		/// </summary>
		/// <typeparam name="T">Vertex type</typeparam>
		/// <param name="vertices">Vertex array</param>
		public void FromVertexCollection< T >( ICollection< T > vertices )
		{
			List<FieldInfo> fields = new List<FieldInfo>( );
			List<VertexField> vertexFields = new List<VertexField>( );
			List<ElementTypeConverter> converters = new List<ElementTypeConverter>( );
			List<FieldValues> valueArrays = new List<FieldValues>( );
			foreach ( FieldInfo field in typeof( T ).GetFields( BindingFlags.NonPublic ) )
			{
				VertexFieldAttribute[] attributes = ( VertexFieldAttribute[] )field.GetCustomAttributes( typeof( VertexFieldAttribute ), false );
				if ( attributes.Length > 0 )
				{
					fields.Add( field );
					vertexFields.Add( attributes[ 0 ].VertexField );

					ElementTypeConverter converter = GetDefaultConverter( field.FieldType );
					converters.Add( converter );

					Array values = Array.CreateInstance( GetElementType( converter.ElementTypeId ), converter.NumElements * vertices.Count );
					valueArrays.Add( new FieldValues( attributes[ 0 ].VertexField, converter.ElementTypeId, converter.NumElements, values) );
				}
			}

			int index = 0;
			foreach ( T vertex in vertices )
			{
				for ( int fieldIndex = 0; fieldIndex < fields.Count; ++fieldIndex )
				{
					object value = fields[ fieldIndex ].GetValue( vertex );
					index = converters[ fieldIndex ].ToArray( value, valueArrays[ fieldIndex ].Values, index );
				}
			}
		}

		public static ElementTypeConverter GetDefaultConverter( Type type )
		{
			return ms_DefaultConverters[ type ];
		}
		
		private readonly static Dictionary< Type, ElementTypeConverter > ms_DefaultConverters;

		static VertexBufferData( )
		{
			ms_DefaultConverters = new Dictionary<Type, ElementTypeConverter>( );
			ms_DefaultConverters.Add( typeof( Point3 ), new ElementTypeConverter( typeof( Point3 ), 3, ElementTypeId.Float32, Point3To3xFloat32 ) );
		}

		public static int Point3To3xFloat32( object src, Array values, int index )
		{
			( ( float[] )values )[ index ]		= ( ( Point3 )src ).X;
			( ( float[] )values )[ index + 1 ]	= ( ( Point3 )src ).Y;
			( ( float[] )values )[ index + 2 ]	= ( ( Point3 )src ).Z;
			return index + 3;
		}

		public delegate int ElementArrayWriterDelegate( object src, Array values, int index );

		public class ElementTypeConverter
		{
			public ElementTypeConverter( Type sourceType, int numElements, ElementTypeId elementTypeId, ElementArrayWriterDelegate writer )
			{
				m_SourceType = sourceType;
				m_NumElements = numElements;
				m_ElementTypeId = elementTypeId;
				m_Writer = writer;
			}

			public int ToArray( object src, Array values, int index )
			{
				return m_Writer( src, values, index );
			}

			public int NumElements
			{
				get { return m_NumElements; }
			}

			public ElementTypeId ElementTypeId
			{
				get { return m_ElementTypeId; }
			}

			private readonly Type m_SourceType;
			private readonly int m_NumElements;
			private readonly ElementTypeId m_ElementTypeId;
			private readonly ElementArrayWriterDelegate m_Writer;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Static data flag
		/// </summary>
		public bool Static
		{
			get { return m_Static; }
			set { m_Static = value; }
		}
		
		/// <summary>
		/// Read-Only data flag
		/// </summary>
		public bool ReadOnly
		{
			get { return m_ReadOnly; }
			set { m_ReadOnly = value; }
		}

		/// <summary>
		/// Returns the number of vertices stored in this buffer
		/// </summary>
		public int NumVertices
		{
			get { return m_NumVertices; }
		}

		/// <summary>
		/// Returns the size (in bytes) of a single vertex
		/// </summary>
		public int VertexSize
		{
			get { return m_VertexSize; }
		}

		/// <summary>
		/// Gets the number of supported fields
		/// </summary>
		public int NumSupportedFields
		{
			get
			{
				int count = 0;
				for ( int index = 0; index < m_FieldArrays.Length; ++index )
				{
					if ( m_FieldArrays[ index ] != null )
					{
						++count;
					}
				}
				return count;
			}
		}

		/// <summary>
		/// Enumerates the values of each of the currently supported fields
		/// </summary>
		public IEnumerable< FieldValues > SupportedFieldValues
		{
			get
			{
				for ( int index = 0; index < m_FieldArrays.Length; ++index )
				{
					if ( m_FieldArrays[ index ] != null )
					{
						yield return m_FieldArrays[ index ];
					}
				}
			}
		}

		#endregion

		#region Public methods
		
		/// <summary>
		/// Converts an <see cref="ElementTypeId"/> into a type
		/// </summary>
		public static Type GetElementType( ElementTypeId id )
		{
			switch ( id )
			{
				case ElementTypeId.Byte : return typeof( byte );
				case ElementTypeId.UInt32 : return typeof( uint );
				case ElementTypeId.Float32 : return typeof( float );
			}
			throw new ArgumentException( string.Format( "No conversion between ElementTypeId  \"{0}\" and type ", id ) );
		}

		/// <summary>
		/// Converts a type into an <see cref="ElementTypeId"/> value
		/// </summary>
		/// <param name="t">Type to convert</param>
		/// <returns>Returns the ElementType value that represents the specified type</returns>
		public static ElementTypeId GetElementTypeId( Type t )
		{
			if ( t ==  typeof( byte ) )
			{
				return ElementTypeId.Byte;
			}
			if ( t ==  typeof( float ) )
			{
				return ElementTypeId.Float32;
			}
			if ( t ==  typeof( uint ) )
			{
				return ElementTypeId.UInt32;
			}
			throw new ArgumentException( string.Format( "No conversion between type \"{0}\" and ElementTypeId value", t.Name ) );
		}

		/// <summary>
		/// Checks for support of a given vertex field
		/// </summary>
		/// <param name="field">Field to check support for</param>
		/// <returns>true if field is supported</returns>
		public bool SupportsField( VertexField field )
		{
			return m_FieldArrays[ ( int )field ] != null;
		}

		/// <summary>
		/// Gets an array of values for a given field
		/// </summary>
		/// <typeparam name="T">Array element type</typeparam>
		/// <param name="field">Vertex field</param>
		/// <returns>Returns the value array for a given field</returns>
		/// <exception cref="ArgumentException">Value array for field not yet created</exception>
		/// <remarks>
		/// Array must have been created using <see cref="VertexBufferData.Add{T}"/>
		/// </remarks>
		public T[] Get< T >( VertexField field )
		{
			FieldValues array = m_FieldArrays[ ( int )field ];
			if ( array == null )
			{
				throw new ArgumentException( string.Format( "No value array exists for field {0}", field ), "field" );
			}
			if ( array.ElementTypeId != GetElementTypeId( typeof( T ) ) )
			{
				throw new InvalidCastException( string.Format( "Value array for field {0} is type {1}, not {2}", field, array.ElementTypeId, typeof( T ).Name ) );
			}
			return ( T[] )array.Values;
		}

		/// <summary>
		/// Gets the underlying <see cref="FieldValues"/> for a given field
		/// </summary>
		/// <param name="field">Field to retrieve values for</param>
		/// <returns>Returns the values object for the specified field. Returns null if no values have been added for this field</returns>
		public FieldValues Get( VertexField field )
		{
			return m_FieldArrays[ ( int )field ];
		}

		/// <summary>
		/// Adds an array of type T to the buffer
		/// </summary>
		/// <typeparam name="T">Array element type. Must be float, uint or byte.</typeparam>
		/// <param name="field">Vertex field type</param>
		/// <param name="numElements">Number of T elements per field</param>
		/// <returns>Returns an array of type T, that has been bound to the specified vertex field</returns>
		/// <exception cref="ArgumentException">Thrown if field has already been created with a different type of value array, or T is an invalid element type</exception>
		public T[] Add< T >( VertexField field, int numElements )
		{
			FieldValues array = m_FieldArrays[ ( int )field ];
			if ( array != null )
			{
				if ( array.ElementType != typeof( T ) )
				{
					string msg = string.Format( "Can't create array of type \"{0}\" for field {1}: an array of type \"{2}\" already exists", typeof( T ).Name, field, array.ElementType.Name );
					throw new ArgumentException( msg );
				}
				return ( T[] )array.Values;
			}

			ElementTypeId typeId = GetElementTypeId( typeof( T ) );
			m_VertexSize += GetElementTypeSize( typeId ) * numElements;

			T[] values = new T[ NumVertices * numElements ];
			array = new FieldValues( field, typeId, numElements, values );
			m_FieldArrays[ ( int )field ] = array;
			return values;
		}

		#endregion

		#region FieldValues class

		/// <summary>
		/// Stores values for each vertex, for a given field
		/// </summary>
		[Serializable]
		public class FieldValues
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="field">Vertex field</param>
			/// <param name="elementType">Type of each element in the array</param>
			/// <param name="numElements">Number of elements making up the field</param>
			/// <param name="values">Value array</param>
			public FieldValues( VertexField field, ElementTypeId elementType, int numElements, Array values )
			{
				m_NumElements = numElements;
				m_Field = field;
				m_ElementType = elementType;
				m_Values = values;
			}

			/// <summary>
			/// Field value array
			/// </summary>
			public Array Values
			{
				get { return m_Values; }
			}

			/// <summary>
			/// Gets the supported field
			/// </summary>
			public VertexField Field
			{
				get { return m_Field; }
			}

			/// <summary>
			/// Gets the size of an individual element in this field
			/// </summary>
			public int ElementSize
			{
				get { return GetElementTypeSize( m_ElementType ); }
			}

			/// <summary>
			/// Gets the number of elements making up this field in a single vertex
			/// </summary>
			public int NumElements
			{
				get { return m_NumElements; }
			}

			/// <summary>
			/// Gets the element type
			/// </summary>
			public ElementTypeId ElementTypeId
			{
				get { return m_ElementType; }
			}

			/// <summary>
			/// Gets the element type
			/// </summary>
			public Type ElementType
			{
				get
				{
					return GetElementType( ElementTypeId );
				}
			}

			/// <summary>
			/// Interleaves data from this field into vertex buffer memory
			/// </summary>
			/// <param name="vertexBytes">Vertex memory</param>
			/// <param name="offset">Byte offset of field in vertex</param>
			/// <param name="stride">Vertex stride</param>
			/// <param name="numVertices">Number of vertices</param>
			public unsafe void Interleave( byte* vertexBytes, int offset, int stride, int numVertices )
			{
				byte* elementBytes = vertexBytes + offset;
				switch ( ElementTypeId )
				{
					case VertexBufferData.ElementTypeId.Byte :
					{
						byte[] values = ( byte[] )m_Values;
						int valueIndex = 0;
						for ( int vertexCount = 0; vertexCount < numVertices; ++vertexCount )
						{
							for ( int elementCount = 0; elementCount < m_NumElements; ++elementCount )
							{
								elementBytes[ elementCount ] = values[ valueIndex++ ];
							}
							elementBytes += stride;
						}

						break;
					}

					case VertexBufferData.ElementTypeId.UInt32 :
					{
						uint[] values = ( uint[] )m_Values;
						int valueIndex = 0;
						for ( int vertexCount = 0; vertexCount < numVertices; ++vertexCount )
						{
							uint* elements = ( uint* )elementBytes;
							for ( int elementCount = 0; elementCount < m_NumElements; ++elementCount )
							{
								elements[ elementCount ] = values[ valueIndex++ ];
							}
							elementBytes += stride;
						}

						break;
					}

					case VertexBufferData.ElementTypeId.Float32 :
					{
						float[] values = ( float[] )m_Values;
						int valueIndex = 0;
						for ( int vertexCount = 0; vertexCount < numVertices; ++vertexCount )
						{
							float* elements = ( float* )elementBytes;
							for ( int elementCount = 0; elementCount < m_NumElements; ++elementCount )
							{
								elements[ elementCount ] = values[ valueIndex++ ];
							}
							elementBytes += stride;
						}

						break;
					}

				}
			}

			private readonly VertexField m_Field;
			private readonly int m_NumElements;
			private readonly ElementTypeId m_ElementType;
			private readonly Array m_Values;
		}

		#endregion

		#region Private stuff

		private readonly FieldValues[] m_FieldArrays;
		private readonly int m_NumVertices;
		private bool m_Static;
		private bool m_ReadOnly;
		private int m_VertexSize;

		private static int GetElementTypeSize( ElementTypeId type )
		{
			switch ( type )
			{
				case ElementTypeId.Byte	: return 1;
				case ElementTypeId.UInt32	: return 4;
				case ElementTypeId.Float32: return 4;
			}
			throw new ArgumentException( string.Format( "Unknown element type \"{0}\"", "type" ) );
		}
		

		#endregion
	}
}
