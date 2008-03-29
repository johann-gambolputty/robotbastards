using System;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Vertex buffer object
	/// </summary>
	public class OpenGlVertexBuffer : IVertexBuffer
	{
		/// <summary>
		/// Creates this vertex buffer
		/// </summary>
		/// <param name="data">Vertex buffer creation data</param>
		public unsafe OpenGlVertexBuffer( VertexBufferData data )
		{
            int[] bufferHandles = new int[ 1 ] { 0 };
            Gl.glGenBuffersARB( 1, bufferHandles );
            m_Handle = bufferHandles[ 0 ];
			
			Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, m_Handle );

			int numSupportedFields = data.NumSupportedFields;
			m_Fields = new FieldInfo[ numSupportedFields ];

			byte[] mem = new byte[ data.VertexSize * data.NumVertices ];
			fixed( byte* vertexBytes = mem )
			{
				int fieldIndex = 0;
				int offset = 0;
				foreach ( VertexBufferData.FieldValues field in data.SupportedFieldValues )
				{
					FieldInfo info = new FieldInfo( );

					field.Interleave( vertexBytes, offset, data.VertexSize, data.NumVertices );
					switch ( field.ElementTypeId )
					{
						case VertexBufferData.ElementTypeId.Byte :
						{
							info.m_Type = Gl.GL_BYTE;
							break;
						}
						case VertexBufferData.ElementTypeId.Float32 :
						{
							info.m_Type = Gl.GL_FLOAT;
							break;
						}
						case VertexBufferData.ElementTypeId.UInt32 :
						{
							info.m_Type = Gl.GL_INT;
							break;
						}
						default :
						{
							throw new InvalidOperationException( string.Format( "No mapping for interleave type \"{0}\" to opengl type", field.ElementTypeId ) );
						}
					}
					switch( field.Field )
					{
						case VertexField.Position :
						{
							info.m_State = Gl.GL_VERTEX_ARRAY;
							break;
						}
						case VertexField.Normal :
						{
							info.m_State = Gl.GL_NORMAL_ARRAY;
							break;
						}
						case VertexField.Diffuse :
						case VertexField.Specular :
						{
							info.m_State = Gl.GL_COLOR_ARRAY;
							break;
						}
						case VertexField.Blend0 :
						case VertexField.Blend1 :
						case VertexField.Blend2 :
						case VertexField.Blend3 :
						{
							info.m_State = Gl.GL_WEIGHT_ARRAY_ARB;
							break;
						}
						case VertexField.Texture0 :
						case VertexField.Texture1 :
						case VertexField.Texture2 :
						case VertexField.Texture3 :
						case VertexField.Texture4 :
						case VertexField.Texture5 :
						case VertexField.Texture6 :
						case VertexField.Texture7 :
						{
							info.m_State = Gl.GL_TEXTURE_COORD_ARRAY;
							break;
						}
						default :
						{
							throw new InvalidOperationException( string.Format( "No mapping for field \"{0}\" to opengl client state ", field.Field ) );
						}
					}

					short fieldSize = ( short )( field.ElementSize * field.NumElements );

					info.m_NumElements = ( short )field.NumElements;
					info.m_Size = fieldSize;
					m_Fields[ fieldIndex++ ] = info;
					offset += fieldSize;
				}
			}

			m_Stride = data.VertexSize;
			
			IntPtr size = new IntPtr( data.VertexSize * data.NumVertices );
			Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, size, mem, data.Static ? Gl.GL_STATIC_DRAW_ARB : Gl.GL_DYNAMIC_DRAW_ARB );
		}


		/// <summary>
		/// Locks a region of the vertex buffer
		/// </summary>
		/// <param name="firstIndex">Index of the first vertex in the buffer to lock</param>
		/// <param name="count">Number of vertices to lock after the first</param>
		/// <returns>Returns a lock object that provides access to the buffer</returns>
		/// <remarks>
		/// Dispose of the lock object to commit the changes to the buffer
		/// </remarks>
		public IVertexBufferLock Lock( int firstIndex, int count )
		{
			throw new NotImplementedException( );
		}

		#region IPass Members

		/// <summary>
		/// Applies this vertex buffer
		/// </summary>
		public void Begin( )
		{
			Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, m_Handle );

			int offset = 0;
			for ( int fieldIndex = 0; fieldIndex < m_Fields.Length; ++fieldIndex )
			{
				FieldInfo field = m_Fields[ fieldIndex ];
				Gl.glEnableClientState( field.m_State );
				switch ( field.m_State )
				{
					case Gl.GL_VERTEX_ARRAY :
					{
						Gl.glVertexPointer( field.m_NumElements, field.m_Type, m_Stride, new IntPtr( offset ) );
						break;
					}
					case Gl.GL_NORMAL_ARRAY :
					{
						Gl.glNormalPointer( field.m_Type, m_Stride, new IntPtr( offset ) );
						break;
					}
					case Gl.GL_COLOR_ARRAY :
					{
						Gl.glColorPointer( field.m_NumElements, field.m_Type, m_Stride, new IntPtr( offset ) );
						break;
					}
					case Gl.GL_TEXTURE_COORD_ARRAY:
					{
						Gl.glTexCoordPointer( field.m_NumElements, field.m_Type, m_Stride, new IntPtr( offset ) );
						break;
					}
				}

				offset += field.m_Size;
			}
		}

		/// <summary>
		/// Un-applies this vertex buffer
		/// </summary>
		public void End( )
		{
			for ( int fieldIndex = 0; fieldIndex < m_Fields.Length; ++fieldIndex )
			{
				Gl.glDisableClientState( m_Fields[ fieldIndex ].m_State );
			}
		}

		#endregion

		#region Private stuff

		private struct FieldInfo
		{
			public int m_State;
			public int m_Type;
			public short m_NumElements;
			public short m_Size;
		}

		private readonly int m_Stride;
		private readonly int m_Handle;
		private readonly FieldInfo[] m_Fields;

		#endregion
	}
}
