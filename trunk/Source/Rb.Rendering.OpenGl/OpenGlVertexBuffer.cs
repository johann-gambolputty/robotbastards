using System;
using System.Collections.Generic;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Vertex buffer object
	/// </summary>
	/// <remarks>
	/// Handy white paper on VBOs: http://developer.nvidia.com/object/using_VBOs.html
	/// </remarks>
	public class OpenGlVertexBuffer : IVertexBuffer
	{
		#region Vertex buffer creation

		/// <summary>
		/// Creates this vertex buffer with a given format and size
		/// </summary>
		/// <param name="format"></param>
		/// <param name="numVertices"></param>
		/// <remarks>
		/// Buffer must be filled using <see cref="Lock"/> prior to use
		/// </remarks>
		public void Create( VertexBufferFormat format, int numVertices )
		{
			CreateVertexBuffer( format, numVertices, null );
		}

		/// <summary>
		/// Creates this vertex buffer from a <see cref="VertexBufferData"/> object
		/// </summary>
		/// <param name="data">Vertex buffer data</param>
		public void Create( VertexBufferData data )
		{
			CreateVertexBuffer( data.Format, data.NumVertices, data.CreateInterleavedArray( ) );
		}

		/// <summary>
		/// Creates this vertex buffer from an array of vertices
		/// </summary>
		/// <typeparam name="T">Vertex type</typeparam>
		/// <param name="vertices">Vertex array</param>
		/// <remarks>
		/// The vertex type T must have one or more fields tagged with the <see cref="VertexFieldAttribute"/> attribute.
		/// At least one of those fields must have the semantic <see cref="VertexFieldSemantic.Position"/>
		/// </remarks>
		public void Create<T>( T[] vertices )
		{
			Create( VertexBufferData.FromVertexCollection( vertices ) );
		}

		#endregion

		/// <summary>
		/// Draws the contents of the vertex buffer directly
		/// </summary>
		public void Draw( PrimitiveType primType )
		{
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Locks a region of the vertex buffer
		/// </summary>
		/// <param name="firstIndex">Index of the first vertex in the buffer to lock</param>
		/// <param name="count">Number of vertices to lock after the first</param>
		/// <param name="read">If true, lock provides read access to the buffer</param>
		/// <param name="write">If true, lock provides write access to the buffer</param>
		/// <returns>Returns a lock object that provides access to the buffer</returns>
		/// <remarks>
		/// Dispose of the lock object to commit the changes to the buffer
		/// </remarks>
		public IVertexBufferLock Lock( int firstIndex, int count, bool read, bool write )
		{
			return new BufferLock( m_Handle, firstIndex, m_Stride, count, read, write );
		}

		#region BufferLock Class

		private unsafe class BufferLock : IVertexBufferLock
		{
			public BufferLock( int handle, int firstIndex, int stride, int count, bool read, bool write )
			{
				Gl.glBindBuffer( Gl.GL_ARRAY_BUFFER, handle );
				m_Handle = handle;
				m_FirstIndex = firstIndex;
				m_Count = count;
				int access;
				if ( !read && !write )
				{
					throw new ArgumentException( "Lock must be created with read and/or write access" );
				}
				else if ( read && write )
				{
					access = Gl.GL_READ_WRITE;
				}
				else if ( read )
				{
					access = Gl.GL_READ_ONLY;
				}
				else
				{
					access = Gl.GL_WRITE_ONLY;
				}
				m_Bytes = ( byte* )Gl.glMapBuffer( Gl.GL_ARRAY_BUFFER, access );
				m_Bytes += firstIndex * stride;
			}

			private readonly int m_FirstIndex;
			private readonly int m_Count;
			private readonly int m_Handle;
			private readonly byte* m_Bytes;

			#region IVertexBufferLock Members

			/// <summary>
			/// Gets the index of the vertex in the buffer where the lock starts
			/// </summary>
			public int FirstVertexIndex
			{
				get { return m_FirstIndex; }
			}

			/// <summary>
			/// Gets the number of vertices covered by the lock
			/// </summary>
			public int VertexCount
			{
				get { return m_Count; }
			}

			public unsafe byte* Bytes
			{
				get { return m_Bytes; }
			}

			public unsafe void Write<T>( int offset, IEnumerable<T> vertices )
			{
				throw new Exception( "The method or operation is not implemented." );
			}

			public unsafe System.IO.Stream ToStream( )
			{
				throw new Exception( "The method or operation is not implemented." );
			}

			#endregion

			#region IDisposable Members

			public void Dispose( )
			{
				Gl.glBindBuffer( Gl.GL_ARRAY_BUFFER, m_Handle );
				Gl.glUnmapBuffer( Gl.GL_ARRAY_BUFFER );
			}

			#endregion
		}

		#endregion

		#region IPass Members

		/// <summary>
		/// Applies this vertex buffer
		/// </summary>
		public void Begin( )
		{
			Gl.glBindBuffer( Gl.GL_ARRAY_BUFFER, m_Handle );

			int offset = 0;
			for ( int fieldIndex = 0; fieldIndex < m_Fields.Length; ++fieldIndex )
			{
				FieldInfo field = m_Fields[ fieldIndex ];

				//	TODO: AP: Can we move this client state enable to the constructor?
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
				//	TODO: AP: Can we remove this client state disable altogether?
				Gl.glDisableClientState( m_Fields[ fieldIndex ].m_State );
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Destroys the vertex buffer
		/// </summary>
		public void Dispose( )
		{
			DestroyBuffer( );
		}

		#endregion

		#region Private stuff

		private struct FieldInfo
		{
			public int		m_State;
			public int		m_Type;
			public short	m_NumElements;
			public short	m_Size;
		}

		private int m_Stride;
		private int m_Handle;
		private FieldInfo[] m_Fields;

		/// <summary>
		/// Maps a <see cref="VertexFieldElementTypeId"/> value to its associated GL value
		/// </summary>
		private static int GetGlFieldElementType( VertexFieldElementTypeId type )
		{
			switch ( type )
			{
				case VertexFieldElementTypeId.Byte		: return Gl.GL_BYTE;
				case VertexFieldElementTypeId.Float32	: return Gl.GL_FLOAT;
				case VertexFieldElementTypeId.Int32		: return Gl.GL_INT;
				case VertexFieldElementTypeId.UInt32	: return Gl.GL_INT;
			}
			throw new NotImplementedException( string.Format( "No mapping for field element type \"{0}\" to opengl type", type ) );
		}

		/// <summary>
		/// Maps a <see cref="VertexFieldSemantic"/> value to its associated GL client state value
		/// </summary>
		private static int GetGlFieldSemantic( VertexFieldSemantic field )
		{
			switch ( field )
			{
				case VertexFieldSemantic.Position: return Gl.GL_VERTEX_ARRAY;
				case VertexFieldSemantic.Normal: return Gl.GL_NORMAL_ARRAY;
				case VertexFieldSemantic.Diffuse:
				case VertexFieldSemantic.Specular: return Gl.GL_COLOR_ARRAY;
				case VertexFieldSemantic.Blend0:
				case VertexFieldSemantic.Blend1:
				case VertexFieldSemantic.Blend2:
				case VertexFieldSemantic.Blend3: return Gl.GL_WEIGHT_ARRAY;
				case VertexFieldSemantic.Texture0:
				case VertexFieldSemantic.Texture1:
				case VertexFieldSemantic.Texture2:
				case VertexFieldSemantic.Texture3:
				case VertexFieldSemantic.Texture4:
				case VertexFieldSemantic.Texture5:
				case VertexFieldSemantic.Texture6:
				case VertexFieldSemantic.Texture7: return Gl.GL_TEXTURE_COORD_ARRAY;
			}
			throw new NotImplementedException( string.Format( "No mapping for field \"{0}\" to opengl client state ", field ) );
		}

		/// <summary>
		/// Vertex buffer destruction
		/// </summary>
		private void DestroyBuffer( )
		{
			if ( m_Handle != 0 )
			{
				Gl.glDeleteBuffers( 1, new int[] { m_Handle } );
				m_Handle = 0;
			}
		}

		/// <summary>
		/// Vertex buffer creation
		/// </summary>
		private unsafe void CreateVertexBuffer( VertexBufferFormat format, int numVertices, byte[] memory )
		{
			DestroyBuffer( );

			format.ValidateFormat( );

			int[] bufferHandles = new int[ 1 ] { 0 };
			Gl.glGenBuffers( 1, bufferHandles );
			m_Handle = bufferHandles[ 0 ];

			Gl.glBindBuffer( Gl.GL_ARRAY_BUFFER, m_Handle );

			m_Stride = format.VertexSize;
			m_Fields = new FieldInfo[ format.NumFields ];

			int fieldIndex = 0;
			foreach ( VertexBufferFormat.FieldDescriptor desc in format.FieldDescriptors )
			{
				FieldInfo info = new FieldInfo( );
				info.m_NumElements = ( short )desc.NumElements;
				info.m_Type = GetGlFieldElementType( desc.ElementType );
				info.m_Size = ( short )desc.FieldSizeInBytes;
				info.m_State = GetGlFieldSemantic( desc.Field );
				m_Fields[ fieldIndex++ ] = info;
			}

			fixed ( byte* pMem = memory )
			{
				IntPtr memPtr = new IntPtr( pMem );
				IntPtr size = new IntPtr( format.VertexSize * numVertices );
				Gl.glBufferData( Gl.GL_ARRAY_BUFFER, size, memPtr, format.Static ? Gl.GL_STATIC_DRAW : Gl.GL_DYNAMIC_DRAW );
			}
		}

		#endregion
	}
}
