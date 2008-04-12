using System;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	public class OpenGlIndexBuffer : IIndexBuffer
	{
		#region Private Members

		private int m_Handle;
		private int m_NumIndices;
		private int m_Type;

		/// <summary>
		/// Index buffer creation
		/// </summary>
		private unsafe void CreateIndexBuffer( IndexBufferFormat format, bool staticBuffer, int numIndices, IntPtr indices )
		{
			DestroyIndexBuffer( );

			m_Type = format == IndexBufferFormat.Int16 ? Gl.GL_UNSIGNED_SHORT : Gl.GL_UNSIGNED_INT;

			int[] bufferHandles = new int[ 1 ] { 0 };
			Gl.glGenBuffersARB( 1, bufferHandles );
			m_Handle = bufferHandles[ 0 ];
			m_NumIndices = numIndices;

			int indexSize = format == IndexBufferFormat.Int16 ? 2 : 4;
			IntPtr size = new IntPtr( numIndices * indexSize );

			Gl.glBindBuffer( Gl.GL_ELEMENT_ARRAY_BUFFER, m_Handle );
			Gl.glBufferData( Gl.GL_ELEMENT_ARRAY_BUFFER, size, indices, staticBuffer ? Gl.GL_STATIC_DRAW : Gl.GL_DYNAMIC_DRAW );
		}

		/// <summary>
		/// Index buffer destruction
		/// </summary>
		private void DestroyIndexBuffer( )
		{
			if ( m_Handle != 0 )
			{
				Gl.glDeleteBuffers( 1, new int[] { m_Handle } );
				m_Handle = 0;
			}
		}

		#endregion

		#region IIndexBuffer Members

		/// <summary>
		/// Gets the length of the index buffer
		/// </summary>
		public int Length
		{
			get { return m_NumIndices; }
		}

		/// <summary>
		/// Creates this index buffer
		/// </summary>
		public void Create( IndexBufferFormat indexSize, bool staticBuffer, int numIndices )
		{
			CreateIndexBuffer( indexSize, staticBuffer, numIndices, IntPtr.Zero );
		}

		/// <summary>
		/// Creates this index buffer from an index array
		/// </summary>
		public unsafe void Create( int[] indices, bool staticBuffer )
		{
			fixed ( int* pIndices = indices )
			{
				CreateIndexBuffer( IndexBufferFormat.Int32, staticBuffer, indices.Length, new IntPtr( pIndices ) );
			}
		}

		/// <summary>
		/// Creates this index buffer from an index array
		/// </summary>
		public unsafe void Create( ushort[] indices, bool staticBuffer )
		{
			fixed ( ushort* pIndices = indices )
			{
				CreateIndexBuffer( IndexBufferFormat.Int16, staticBuffer, indices.Length, new IntPtr( pIndices ) );
			}
		}

		/// <summary>
		/// Draws elements of the specified primitive type using the index buffer
		/// </summary>
		public void Draw( PrimitiveType primType )
		{
			Gl.glBindBuffer( Gl.GL_ELEMENT_ARRAY_BUFFER, m_Handle );
			switch ( primType )
			{
				case PrimitiveType.TriList :
					{
						Gl.glDrawElements( Gl.GL_TRIANGLES, m_NumIndices, m_Type, IntPtr.Zero );
						break;
					}
				case PrimitiveType.TriStrip :
					{
						Gl.glDrawElements( Gl.GL_TRIANGLE_STRIP, m_NumIndices, m_Type, IntPtr.Zero );
						break;	
					}
				case PrimitiveType.TriFan :
					{
						Gl.glDrawElements( Gl.GL_TRIANGLE_FAN, m_NumIndices, m_Type, IntPtr.Zero );
						break;	
					}
				default :
					throw new NotImplementedException( "Unhandled primitive type " + primType );
			}
			Gl.glBindBuffer( Gl.GL_ELEMENT_ARRAY_BUFFER, 0 );
		}

		/// <summary>
		/// Draws elements of the specified primitive type, using a range within the index buffer
		/// </summary>
		public void Draw( PrimitiveType primType, int firstIndex, int count )
		{
			Gl.glBindBuffer( Gl.GL_ELEMENT_ARRAY_BUFFER, m_Handle );
			IntPtr pOffset = new IntPtr( firstIndex );
			switch ( primType )
			{
				case PrimitiveType.TriList:
					{
						Gl.glDrawElements( Gl.GL_TRIANGLES, count, m_Type, pOffset );
						break;
					}
				case PrimitiveType.TriStrip:
					{
						Gl.glDrawElements( Gl.GL_TRIANGLE_STRIP, count, m_Type, pOffset );
						break;
					}
				case PrimitiveType.TriFan:
					{
						Gl.glDrawElements( Gl.GL_TRIANGLE_FAN, count, m_Type, pOffset );
						break;
					}
				default:
					throw new NotImplementedException( "Unhandled primitive type " + primType );
			}
			Gl.glBindBuffer( Gl.GL_ELEMENT_ARRAY_BUFFER, 0 );
		}

		/// <summary>
		/// Locks a region of the index buffer
		/// </summary>
		public IIndexBufferLock Lock( int firstIndex, int count )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes of this index buffer
		/// </summary>
		public void Dispose( )
		{
			if ( m_Handle != 0 )
			{
				Gl.glDeleteBuffers( 1, new int[] { m_Handle } );
				m_Handle = 0;
			}
		}

		#endregion
	}
}
