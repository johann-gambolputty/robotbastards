using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rb.Core.Maths;
using Rb.Core.Utils;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Simple vertex buffer builder class
	/// </summary>
	public class VertexBufferBuilder
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public VertexBufferBuilder( VertexBufferFormat format )
		{
			Arguments.CheckNotNull( format, "format" );
			format.ValidateFormat( );
			m_Format = format;
			foreach ( VertexBufferFormat.FieldDescriptor field in format.FieldDescriptors )
			{
				m_Streams.Add( field.Field, new List<float>( ) );
			}
		}

		public void Add( VertexFieldSemantic field, float x )
		{
			CheckAdd( field, 1 );
			List<float> stream = m_Streams[ field ];
			stream.Add( x );
		}

		public void Add( VertexFieldSemantic field, float x, float y )
		{
			CheckAdd( field, 2 );
			List<float> stream = m_Streams[ field ];
			stream.Add( x );
			stream.Add( y );
		}

		public void Add( VertexFieldSemantic field, float x, float y, float z )
		{
			CheckAdd( field, 3 );
			List<float> stream = m_Streams[ field ];
			stream.Add( x );
			stream.Add( y );
			stream.Add( z );
		}
		public void Add( VertexFieldSemantic field, float x, float y, float z, float w )
		{
			CheckAdd( field, 4 );
			List<float> stream = m_Streams[ field ];
			stream.Add( x );
			stream.Add( y );
			stream.Add( z );
			stream.Add( w );
		}

		public void Add( VertexFieldSemantic field, Point2 pt ) { Add( field, pt.X, pt.Y ); }
		public void Add( VertexFieldSemantic field, Point3 pt ) { Add( field, pt.X, pt.Y, pt.Z ); }
		public void Add( VertexFieldSemantic field, Vector3 vec ) { Add( field, vec.X, vec.Y, vec.Z ); }

		/// <summary>
		/// Creates a vertex buffer from the data added so far
		/// </summary>
		public IVertexBuffer Build( )
		{
			int numVertices = -1;
			foreach ( KeyValuePair<VertexFieldSemantic, List<float>> kvp in m_Streams )
			{
				int streamVertexCount = kvp.Value.Count / m_Format.GetDescriptor( kvp.Key ).NumElements;
				if ( numVertices == -1 )
				{
					numVertices = streamVertexCount;
					continue;
				}
				if ( numVertices != streamVertexCount )
				{
					throw new InvalidOperationException( string.Format( "Expected stream for field \"{0}\" to contain {1} vertices, not {2}", kvp.Key, numVertices, streamVertexCount ) );
				}
			}

			VertexBufferData data = new VertexBufferData( m_Format, numVertices );
			foreach ( KeyValuePair<VertexFieldSemantic, List<float>> kvp in m_Streams )
			{
				float[] elements = data.Add<float>( kvp.Key, m_Format.GetDescriptor( kvp.Key ).NumElements );
				Debug.Assert( elements.Length == kvp.Value.Count );

				for ( int elementIndex = 0; elementIndex < elements.Length; ++elementIndex )
				{
					elements[ elementIndex ] = kvp.Value[ elementIndex ];
				}
			}

			IVertexBuffer vb = Graphics.Factory.CreateVertexBuffer( );
			vb.Create( data );
			return vb;
		}

		private readonly VertexBufferFormat m_Format;
		private readonly Dictionary<VertexFieldSemantic, List<float>> m_Streams = new Dictionary<VertexFieldSemantic, List<float>>( );

		private void CheckAdd( VertexFieldSemantic fieldType, int numElements )
		{
			VertexBufferFormat.FieldDescriptor field = m_Format.GetDescriptor( fieldType );
			if ( field == null )
			{
				throw new InvalidOperationException( string.Format( "Field for \"{0}\" was not specified in format \"{1}\"", field, m_Format ) );
			}
			if ( field.NumElements != numElements )
			{

				throw new InvalidOperationException( string.Format( "Expected field \"{0}\" in format \"{1}\" to contain {2} elements, not {3}", field, m_Format, field.NumElements, numElements ) );
			}
		}

	}
}
