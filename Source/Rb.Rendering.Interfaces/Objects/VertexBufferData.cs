using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Rb.Core.Maths;

namespace Rb.Rendering.Interfaces.Objects
{

	/// <summary>
	/// Data used for constructing a vertex buffer
	/// </summary>
	[Serializable]
	public class VertexBufferData
	{
		/// <summary>
		/// Creates a byte array containing interleaved data from all the vertex field arrays
		/// </summary>
		public unsafe byte[] CreateInterleavedArray( )
		{
			byte[] mem = new byte[ Format.VertexSize * NumVertices ];
			fixed ( byte* vertexBytes = mem )
			{
				int offset = 0;
				foreach ( FieldValues field in SupportedFieldValues )
				{
					field.Interleave( vertexBytes, offset, Format.VertexSize, NumVertices );
					offset += field.Field.FieldSizeInBytes;
				}
			}
			return mem;
		}

		/// <summary>
		/// Returns the set of field values supported by this data
		/// </summary>
		public IEnumerable<FieldValues> SupportedFieldValues
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

		/// <summary>
		/// Builds a string representation of this object
		/// </summary>
		public override string ToString( )
		{
			return m_Format.ToString( );
		}

		#region Construction

		/// <summary>
		/// 
		/// </summary>
		/// <param name="format"></param>
		/// <param name="numVertices"></param>
		public VertexBufferData( VertexBufferFormat format, int numVertices )
		{
			m_Format = format;
			m_NumVertices = numVertices;
		}

		#endregion

		#region Buffer creation from vertex objects
		
		/// <summary>
		/// Creates vertex data from an array of vertex objects
		/// </summary>
		/// <typeparam name="T">Vertex type</typeparam>
		/// <param name="vertices">Vertex array</param>
		public static VertexBufferData FromVertexCollection< T >( ICollection< T > vertices )
		{
			List<FieldInfo> fields = new List<FieldInfo>( );
			List<ElementTypeConverter> converters = new List<ElementTypeConverter>( );
			List<FieldValues> valueArrays = new List<FieldValues>( );

			VertexBufferData buffer = new VertexBufferData( VertexBufferFormat.FromVertexClass< T >( ), vertices.Count );

			//	Run through all the fields in type T
			foreach ( FieldInfo field in typeof( T ).GetFields( BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic ) )
			{
				//	Check if the current field has a VertexField attribute
				VertexFieldAttribute[] attributes = ( VertexFieldAttribute[] )field.GetCustomAttributes( typeof( VertexFieldAttribute ), false );
				if ( attributes.Length == 0 )
				{
					continue;
				}

				VertexFieldSemantic semantic = attributes[ 0 ].Semantic;

				//	It does - add it to the fields list
				fields.Add( field );

				//	Add an element type converter (for transfering the field's type into a float/uint/byte array)
				ElementTypeConverter converter = GetDefaultConverter( field.FieldType );
				converters.Add( converter );

				Type elementType = VertexBufferFormat.GetElementType( converter.ElementTypeId );

				//	Create the FieldValues object
				Array values = Array.CreateInstance( elementType, converter.NumElements * vertices.Count );
				FieldValues fieldValues = new FieldValues( buffer.Format.GetDescriptor( semantic ), values );
				valueArrays.Add( fieldValues );
				buffer.m_FieldArrays[ ( int )semantic ] = fieldValues;
			}
			
			//	For each vertex in the source collection
			int[] fieldIndexes = new int[ fields.Count ];
			foreach ( T vertex in vertices )
			{
				//	Transfer the current vertex's marked fields into the FieldValues arrays
				for ( int fieldIndex = 0; fieldIndex < fields.Count; ++fieldIndex )
				{
					object value = fields[ fieldIndex ].GetValue( vertex );
					fieldIndexes[ fieldIndex ] = converters[ fieldIndex ].ToArray( value, valueArrays[ fieldIndex ].Values, fieldIndexes[ fieldIndex ] );
				}
			}

			return buffer;
		}
		
		/// <summary>
		/// Gets an element type converter for a given type
		/// </summary>
		/// <param name="type">Source type</param>
		/// <returns>Converter for source type</returns>
		public static ElementTypeConverter GetDefaultConverter( Type type )
		{
			return s_DefaultConverters[ type ];
		}
		
		#endregion

		#region Construction

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="numVertices">Number of vertices that the buffer can store</param>
		public VertexBufferData( int numVertices )
		{
			m_NumVertices = numVertices;
			m_FieldArrays = new FieldValues[ Enum.GetValues( typeof( VertexFieldSemantic ) ).Length ];
		}

		#region Private type conversion functions

		private static int Point2ToFloat32( Point2 src, float[] values, int index )
		{
			values[ index++ ]	= src.X;
			values[ index++ ]	= src.Y;
			return index;
		}

		private static int Point3ToFloat32( Point3 src, float[] values, int index )
		{
			values[ index++ ]	= src.X;
			values[ index++ ]	= src.Y;
			values[ index++ ]	= src.Z;
			return index;
		}

		private static int Vector2ToFloat32(Vector2 src, float[] values, int index)
		{
			values[ index++ ]	= src.X;
			values[ index++ ]	= src.Y;
			return index;
		}

		private static int Vector3ToFloat32( Vector3 src, float[] values, int index )
		{
			values[ index++ ]	= src.X;
			values[ index++ ]	= src.Y;
			values[ index++ ]	= src.Z;
			return index;
		}

		//private static int ColourToByte( Color src, byte[] values, int index)
		//{
		//    values[ index++ ] = src.R;
		//    values[ index++ ] = src.G;
		//    values[ index++ ] = src.B;
		//    values[ index++ ] = src.A;
		//    return index;
		//}

		private static int ColourToFloat32( Color src, float[] values, int index )
		{
			values[ index++ ] = src.R / 255.0f;
			values[ index++ ] = src.G / 255.0f;
			values[ index++ ] = src.B / 255.0f;
			values[ index++ ] = src.A / 255.0f;
			return index;
		}

		#endregion

		public delegate int ElementArrayWriterDelegate< SrcType, ElementType >( SrcType src, ElementType[] values, int index );

		public class ElementTypeConverter< SrcType, ElementType > : ElementTypeConverter
		{
			public ElementTypeConverter( int numElements, VertexFieldElementTypeId elementTypeId, ElementArrayWriterDelegate< SrcType, ElementType > writer ) :
				base( numElements, elementTypeId )
			{
				m_Writer = writer;
			}

			public override int ToArray( object src, Array values, int index )
			{
				return m_Writer( ( SrcType )src, ( ElementType[] )values, index );
			}

			private readonly ElementArrayWriterDelegate<SrcType, ElementType> m_Writer;

		}

		public abstract class ElementTypeConverter
		{
			public ElementTypeConverter( int numElements, VertexFieldElementTypeId elementTypeId )
			{
				m_NumElements = numElements;
				m_ElementTypeId = elementTypeId;
			}

			public abstract int ToArray( object src, Array values, int index );

			public int NumElements
			{
				get { return m_NumElements; }
			}

			public VertexFieldElementTypeId ElementTypeId
			{
				get { return m_ElementTypeId; }
			}

			private readonly int m_NumElements;
			private readonly VertexFieldElementTypeId m_ElementTypeId;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Static data flag
		/// </summary>
		public VertexBufferFormat Format
		{
			get { return m_Format; }
		}

		/// <summary>
		/// Returns the number of vertices stored in this buffer
		/// </summary>
		public int NumVertices
		{
			get { return m_NumVertices; }
		}

		#endregion

		#region Public methods

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
		public T[] Get< T >( VertexFieldSemantic field )
		{
			FieldValues array = m_FieldArrays[ ( int )field ];
			if ( array == null )
			{
				throw new ArgumentException( string.Format( "No value array exists for field {0}", field ), "field" );
			}
			if ( array.ElementTypeId != VertexBufferFormat.GetElementTypeId( typeof( T ) ) )
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
		public FieldValues Get( VertexFieldSemantic field )
		{
			return m_FieldArrays[ ( int )field ];
		}

		/// <summary>
		/// Adds an array of type T to the buffer. If an array has already been added for the specified semantic tag, that is returned instead
		/// </summary>
		/// <typeparam name="T">Array element type. Must be float, uint or byte.</typeparam>
		/// <param name="field">Vertex field type</param>
		/// <param name="numElements">Number of T elements per field</param>
		/// <returns>Returns an array of type T, that has been bound to the specified vertex field</returns>
		/// <exception cref="ArgumentException">Thrown if field has already been created with a different type of value array, or T is an invalid element type</exception>
		public T[] Add< T >( VertexFieldSemantic field, int numElements )
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

			VertexBufferFormat.FieldDescriptor desc = Format.Add< T >( field, numElements );

			T[] values = new T[ NumVertices * numElements ];
			array = new FieldValues( desc, values );
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
			/// <param name="field">Vertex field descriptor</param>
			/// <param name="values">Value array</param>
			public FieldValues( VertexBufferFormat.FieldDescriptor field, Array values )
			{
				m_Field = field;
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
			public VertexBufferFormat.FieldDescriptor Field
			{
				get { return m_Field; }
			}

			/// <summary>
			/// Gets the size of an individual element in this field
			/// </summary>
			public int ElementSize
			{
				get { return VertexBufferFormat.GetElementTypeSize( m_Field.ElementType ); }
			}
	
			/// <summary>
			/// Gets the number of elements making up this field in a single vertex
			/// </summary>
			public int NumElements
			{
				get { return m_Field.NumElements; }
			}

			/// <summary>
			/// Gets the element type
			/// </summary>
			public VertexFieldElementTypeId ElementTypeId
			{
				get { return m_Field.ElementType; }
			}

			/// <summary>
			/// Gets the element type
			/// </summary>
			public Type ElementType
			{
				get
				{
					return VertexBufferFormat.GetElementType( ElementTypeId );
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
					case VertexFieldElementTypeId.Byte :
					{
						byte[] values = ( byte[] )m_Values;
						int valueIndex = 0;
						for ( int vertexCount = 0; vertexCount < numVertices; ++vertexCount )
						{
							for ( int elementCount = 0; elementCount < m_Field.NumElements; ++elementCount )
							{
								elementBytes[ elementCount ] = values[ valueIndex++ ];
							}
							elementBytes += stride;
						}

						break;
					}

					case VertexFieldElementTypeId.Int32 :
					{
						int[] values = ( int[] )m_Values;
						int valueIndex = 0;
						for ( int vertexCount = 0; vertexCount < numVertices; ++vertexCount )
						{
							int* elements = ( int* )elementBytes;
							for ( int elementCount = 0; elementCount < m_Field.NumElements; ++elementCount )
							{
								elements[ elementCount ] = values[ valueIndex++ ];
							}
							elementBytes += stride;
						}

						break;
					}

					case VertexFieldElementTypeId.UInt32:
					{
						uint[] values = ( uint[] )m_Values;
						int valueIndex = 0;
						for ( int vertexCount = 0; vertexCount < numVertices; ++vertexCount )
						{
							uint* elements = ( uint* )elementBytes;
							for ( int elementCount = 0; elementCount < m_Field.NumElements; ++elementCount )
							{
								elements[ elementCount ] = values[ valueIndex++ ];
							}
							elementBytes += stride;
						}

						break;
					}

					case VertexFieldElementTypeId.Float32 :
					{
						float[] values = ( float[] )m_Values;
						int valueIndex = 0;
						for ( int vertexCount = 0; vertexCount < numVertices; ++vertexCount )
						{
							float* elements = ( float* )elementBytes;
							for ( int elementCount = 0; elementCount < m_Field.NumElements; ++elementCount )
							{
								elements[ elementCount ] = values[ valueIndex++ ];
							}
							elementBytes += stride;
						}

						break;
					}

				}
			}

			private readonly VertexBufferFormat.FieldDescriptor m_Field;
			private readonly Array m_Values;
		}

		#endregion

		#region Private stuff

		private readonly static Dictionary<Type, ElementTypeConverter> s_DefaultConverters;

		private readonly VertexBufferFormat m_Format;
		private readonly FieldValues[] m_FieldArrays = new FieldValues[ Enum.GetValues( typeof( VertexFieldSemantic ) ).Length ];
		private readonly int m_NumVertices;

		/// <summary>
		/// Adds a delegate that can convert from SrcType to ElementType
		/// </summary>
		private static void AddConverter< SrcType, ElementType >( int numElements, VertexFieldElementTypeId typeId, ElementArrayWriterDelegate< SrcType, ElementType > writer )
		{
			s_DefaultConverters.Add( typeof( SrcType ), new ElementTypeConverter< SrcType, ElementType >( numElements, typeId, writer ) );
		}
		
		/// <summary>
		/// Initializes default type converters
		/// </summary>
		static VertexBufferData( )
		{
			s_DefaultConverters = new Dictionary<Type, ElementTypeConverter>( );
			
			AddConverter<Point2, float>( 2, VertexFieldElementTypeId.Float32, Point2ToFloat32 );
			AddConverter<Point3, float>( 3, VertexFieldElementTypeId.Float32, Point3ToFloat32 );
			AddConverter<Vector2, float>( 2, VertexFieldElementTypeId.Float32, Vector2ToFloat32 );
			AddConverter<Vector3, float>( 3, VertexFieldElementTypeId.Float32, Vector3ToFloat32 );
		//	AddConverter<Color, byte>( 4, VertexFieldElementTypeId.Byte, ColourToByte );
			AddConverter<Color, float>( 4, VertexFieldElementTypeId.Float32, ColourToFloat32 );
		}

		#endregion
	}
}
