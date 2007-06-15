using System;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Summary description for OpenGlVertexBuffer.
	/// </summary>
	public class OpenGlVertexBuffer
	{
		/// <summary>
		/// Access to the opengl vertex buffer handle
		/// </summary>
		public int					Handle
		{
			get
			{
				return m_Handle;
			}
		}

		/// <summary>
		/// Access to the opengl client state
		/// </summary>
		public int					ClientState
		{
			get
			{
				return m_ClientState;
			}
		}

		/// <summary>
		/// Access to the opengl element type (gl symbolic value for the base type of data in this buffer)
		/// </summary>
		public int					ElementType
		{
			get
			{
				return m_ElementType;
			}
		}

		/// <summary>
		/// Stride between vertices
		/// </summary>
		public short				Stride
		{
			get
			{
				return m_Stride;
			}
		}

		/// <summary>
		/// Number of elements in a vertex (of type ElementType)
		/// </summary>
		public short				NumElements
		{
			get
			{
				return m_NumElements;
			}
		}

		/// <summary>
		/// Sets up this vertex buffer
		/// </summary>
		/// <param name="numVertices">Number of vertices in the buffer</param>
		/// <param name="offset">Offset from the start of buffer to the usable vertices (measured in vertices, not elements)</param>
		/// <param name="clientState">OpenGL client state (e.g. GL_VERTEX_ARRAY)</param>
		/// <param name="stride">Stride between vertices</param>
		/// <param name="numElements">Number of floats per vertex</param>
		/// <param name="usage">Buffer usage (e.g. GL_STATIC_BUFFER_ARB)</param>
		/// <param name="buffer">Buffer data</param>
		public			OpenGlVertexBuffer( int numVertices, int offset, int clientState, short stride, short numElements, int usage, float[] buffer )
		{
			m_ClientState	= clientState;
			m_Stride		= stride;
			m_NumElements	= numElements;
			m_ElementType	= Gl.GL_FLOAT;

			//	Generate a VBO
			Gl.glGenBuffersARB( 1, out m_Handle );

			//	Bind and fill the VBO
			Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, m_Handle );

			if ( offset == 0 )
			{
				Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 4 * numElements ) * numVertices, buffer, usage );
			}
			else
			{
				float[] subBuffer = new float[ numVertices * numElements ];
				Array.Copy( buffer, offset * numElements, subBuffer, 0, numVertices * numElements );
				Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 4 * numElements ) * numVertices, subBuffer, usage );
			}
		}

		/// <summary>
		/// Sets up this vertex buffer
		/// </summary>
		/// <param name="numVertices">Number of vertices in the buffer</param>
		/// <param name="offset">Offset from the start of buffer to the usable vertices (measured in vertices, not elements)</param>
		/// <param name="clientState">OpenGL client state (e.g. GL_VERTEX_ARRAY)</param>
		/// <param name="stride">Stride between vertices</param>
		/// <param name="numElements">Number of integers per vertex</param>
		/// <param name="usage">Buffer usage (e.g. GL_STATIC_BUFFER_ARB)</param>
		/// <param name="buffer">Buffer data</param>
		public			OpenGlVertexBuffer( int numVertices, int offset, int clientState, short stride, short numElements, int usage, int[] buffer )
		{
			m_ClientState	= clientState;
			m_Stride		= stride;
			m_NumElements	= numElements;
			m_ElementType	= Gl.GL_INT;

			//	Generate a VBO
			Gl.glGenBuffersARB( 1, out m_Handle );

			//	Bind and fill the VBO
			Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, m_Handle );
			
			if ( offset == 0 )
			{
				Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 4 * numElements ) * numVertices, buffer, usage );
			}
			else
			{
				int[] subBuffer = new int[ numVertices * numElements ];
				Array.Copy( buffer, offset * numElements, subBuffer, 0, numVertices * numElements );
				Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 4 * numElements ) * numVertices, subBuffer, usage );
			}
		}

		/// <summary>
		/// Sets up this vertex buffer
		/// </summary>
		/// <param name="numVertices">Number of vertices in the buffer</param>
		/// <param name="offset">Offset from the start of buffer to the usable vertices (measured in vertices, not elements)</param>
		/// <param name="clientState">OpenGL client state (e.g. GL_VERTEX_ARRAY)</param>
		/// <param name="stride">Stride between vertices</param>
		/// <param name="numElements">Number of integers per vertex</param>
		/// <param name="usage">Buffer usage (e.g. GL_STATIC_BUFFER_ARB)</param>
		/// <param name="buffer">Buffer data</param>
		public			OpenGlVertexBuffer( int numVertices, int offset, int clientState, short stride, short numElements, int usage, byte[] buffer )
		{
			m_ClientState	= clientState;
			m_Stride		= stride;
			m_NumElements	= numElements;
			m_ElementType	= Gl.GL_UNSIGNED_BYTE;

			//	Generate a VBO
			Gl.glGenBuffersARB( 1, out m_Handle );

			//	Bind and fill the VBO
			Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, m_Handle );
			
			if ( offset == 0 )
			{
				Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 1 * numElements ) * numVertices, buffer, usage );
			}
			else
			{
				byte[] subBuffer = new byte[ numVertices * numElements ];
				Array.Copy( buffer, offset * numElements, subBuffer, 0, numVertices * numElements );
				Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 4 * numElements ) * numVertices, subBuffer, usage );
			}
		}

		/// <summary>
		/// Applies this vertex buffer
		/// </summary>
		public void		Begin( )
		{
			Gl.glEnableClientState( ClientState );
			Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, m_Handle );

			float[] nullArray = null;
			switch ( ClientState )
			{
				case Gl.GL_VERTEX_ARRAY :
					Gl.glVertexPointer( NumElements, ElementType, 0, nullArray );
					break;

				case Gl.GL_NORMAL_ARRAY :
					Gl.glNormalPointer( ElementType, 0, nullArray );
					break;

				case Gl.GL_COLOR_ARRAY :
					Gl.glColorPointer( NumElements, ElementType, 0, nullArray );
					break;

				case Gl.GL_TEXTURE_COORD_ARRAY :
					Gl.glTexCoordPointer( NumElements, ElementType, 0, nullArray );
					break;
			}
		}

		/// <summary>
		/// Disables the client state that was enabled by Begin()
		/// </summary>
		public void End( )
		{
			Gl.glDisableClientState( ClientState );
		}

		private	int		m_Handle;
		private int		m_ClientState;
		private int		m_ElementType;
		private short	m_Stride;
		private short	m_NumElements;
	}
}
