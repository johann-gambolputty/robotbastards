using System;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	public class OpenGlIndexBuffer : IIndexBuffer
	{
		public OpenGlIndexBuffer( IndexBufferFormat format, int numIndices ) :
			this( format, numIndices, null )
		{
		}

		public OpenGlIndexBuffer( IndexBufferData data ) :
			this( data.Format, data.Indices.Length, data.Indices )
		{
			
		}

		private unsafe OpenGlIndexBuffer( IndexBufferFormat format, int numIndices, int[] indices )
		{
			int[] bufferHandles = new int[ 1 ] { 0 };
			Gl.glGenBuffersARB( 1, bufferHandles );
			m_Handle = bufferHandles[ 0 ];
			m_NumIndices = indices.Length;

			Gl.glBindBuffer( Gl.GL_ELEMENT_ARRAY_BUFFER, m_Handle );
			fixed ( int* pIndices = indices )
			{
				IntPtr indicesPtr = new IntPtr( pIndices );
			//	IntPtr size = new IntPtr( 4 * numIndices ); // TODO: AP: Ignores format
				IntPtr size = new IntPtr( numIndices * 4 );
				Gl.glBufferData( Gl.GL_ELEMENT_ARRAY_BUFFER, size, indicesPtr, format.Static ? Gl.GL_STATIC_DRAW : Gl.GL_DYNAMIC_DRAW );
			}
		}

		private int m_Handle;
		private readonly int m_NumIndices;

		#region IIndexBuffer Members

		/// <summary>
		/// Gets the length of the index buffer
		/// </summary>
		public int Length
		{
			get { return m_NumIndices; }
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
						Gl.glDrawElements( Gl.GL_TRIANGLES, m_NumIndices, Gl.GL_UNSIGNED_INT, IntPtr.Zero );
						break;
					}
				case PrimitiveType.TriStrip :
					{
						Gl.glDrawElements( Gl.GL_TRIANGLE_STRIP, m_NumIndices, Gl.GL_UNSIGNED_INT, IntPtr.Zero );
						break;	
					}
				case PrimitiveType.TriFan :
					{
						Gl.glDrawElements( Gl.GL_TRIANGLE_FAN, m_NumIndices, Gl.GL_UNSIGNED_INT, IntPtr.Zero );
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
						Gl.glDrawElements( Gl.GL_TRIANGLES, count, Gl.GL_UNSIGNED_INT, pOffset );
						break;
					}
				case PrimitiveType.TriStrip:
					{
						Gl.glDrawElements( Gl.GL_TRIANGLE_STRIP, count, Gl.GL_UNSIGNED_INT, pOffset );
						break;
					}
				case PrimitiveType.TriFan:
					{
						Gl.glDrawElements( Gl.GL_TRIANGLE_FAN, count, Gl.GL_UNSIGNED_INT, pOffset );
						break;
					}
				default:
					throw new NotImplementedException( "Unhandled primitive type " + primType );
			}
			Gl.glBindBuffer( Gl.GL_ELEMENT_ARRAY_BUFFER, 0 );
		}

		public IIndexBufferLock Lock( int firstIndex, int count )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		#endregion

		#region IDisposable Members

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
