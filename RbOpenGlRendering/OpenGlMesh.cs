using System;
using Tao.OpenGl;

namespace RbOpenGlRendering
{
	/// <summary>
	/// Simple OpenGL mesh
	/// </summary>
	public class OpenGlMesh : RbEngine.Rendering.IRender
	{

		#region	Group setup

		/// <summary>
		/// Creates the mesh index buffer
		/// </summary>
		public void			CreateGroups( int size )
		{
			m_Groups = new Group[ size ];
		}

		/// <summary>
		/// Sets up a group's index buffer
		/// </summary>
		public void			CreateGroupIndexBuffer( int index, int[] indices, int primitiveType )
		{
			m_Groups[ index ].IndexBuffer	= indices;
			m_Groups[ index ].PrimitiveType	= primitiveType;
		}

		#endregion

		#region	Vertex buffer setup

		/// <summary>
		/// Creates
		/// </summary>
		/// <param name="size"></param>
		public void			CreateVertexBuffers( int size )
		{
			m_VertexBufferObjects = new VertexBufferObject[ size ];
		}

		/// <summary>
		/// Sets up a vertex buffer with a float array of data
		/// </summary>
		public void			SetupVertexBuffer( int index, int numVertices, int clientState, short stride, short numElements, int usage, float[] buffer )
		{
			VertexBufferObject curVbo;
			curVbo.ClientState	= clientState;
			curVbo.Stride		= stride;
			curVbo.NumElements	= numElements;
			curVbo.ElementType	= Gl.GL_FLOAT;

			//	Generate a VBO
			Gl.glGenBuffersARB( 1, out curVbo.Handle );

			//	Bind and fill the VBO
			Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, curVbo.Handle );
			Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 4 * curVbo.NumElements ) * numVertices, buffer, usage );

			m_VertexBufferObjects[ index ] = curVbo;
		}

		/// <summary>
		/// Sets up a vertex buffer with an int array of data
		/// </summary>
		public void			SetupVertexBuffer( int index, int numVertices, int clientState, short stride, short numElements, int usage, int[] buffer )
		{
			VertexBufferObject curVbo;
			curVbo.ClientState	= clientState;
			curVbo.Stride		= stride;
			curVbo.NumElements	= numElements;
			curVbo.ElementType	= Gl.GL_INT;

			//	Generate a VBO
			Gl.glGenBuffersARB( 1, out curVbo.Handle );

			//	Bind and fill the VBO
			Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, curVbo.Handle );
			Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 4 * curVbo.NumElements ) * numVertices, buffer, usage );

			m_VertexBufferObjects[ index ] = curVbo;
		}

		/// <summary>
		/// Sets up a vertex buffer with a byte array of data
		/// </summary>
		public void			SetupVertexBuffer( int index, int numVertices, int clientState, short stride, short numElements, int usage, byte[] buffer )
		{
			VertexBufferObject curVbo;
			curVbo.ClientState	= clientState;
			curVbo.Stride		= stride;
			curVbo.NumElements	= numElements;
			curVbo.ElementType	= Gl.GL_UNSIGNED_BYTE;

			//	Generate a VBO
			Gl.glGenBuffersARB( 1, out curVbo.Handle );

			//	Bind and fill the VBO
			Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, curVbo.Handle );
			Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 1 * curVbo.NumElements ) * numVertices, buffer, usage );

			m_VertexBufferObjects[ index ] = curVbo;
		}

		#endregion


		/// <summary>
		/// Indexed vertex group
		/// </summary>
		private struct Group
		{
			public int[]	IndexBuffer;
			public int		PrimitiveType;
		}

		/// <summary>
		/// Vertex buffer object
		/// </summary>
		private struct VertexBufferObject
		{
			public int		Handle;
			public int		ClientState;
			public int		ElementType;
			public short	Stride;
			public short	NumElements;
		}

		private Group[]					m_Groups;
		private VertexBufferObject[]	m_VertexBufferObjects;
		
		#region IRender Members

		/// <summary>
		/// Renders this mesh
		/// </summary>
		public void Render()
		{
			//	TODO: This is a bit rubbish - disables all client states, enables them as needed
			//	Can they be enabled, bound, then disabled?
			Gl.glDisableClientState( Gl.GL_VERTEX_ARRAY );
			Gl.glDisableClientState( Gl.GL_COLOR_ARRAY );
			Gl.glDisableClientState( Gl.GL_NORMAL_ARRAY );
			Gl.glDisableClientState( Gl.GL_TEXTURE_COORD_ARRAY );

			for ( int vboIndex = 0; vboIndex < m_VertexBufferObjects.Length; ++vboIndex )
			{
				VertexBufferObject curVbo = m_VertexBufferObjects[ vboIndex ];

				Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, curVbo.Handle );
				Gl.glEnableClientState( curVbo.ClientState );

				float[] nullArray = null;
				switch ( curVbo.ClientState )
				{
					case Gl.GL_VERTEX_ARRAY :
						Gl.glVertexPointer( curVbo.NumElements, curVbo.ElementType, 0, nullArray );
						break;

					case Gl.GL_NORMAL_ARRAY :
						Gl.glNormalPointer( curVbo.ElementType, 0, nullArray );
						break;

					case Gl.GL_COLOR_ARRAY :
						Gl.glColorPointer( curVbo.NumElements, curVbo.ElementType, 0, nullArray );
						break;
				}
			}

			for ( int groupIndex = 0; groupIndex < m_Groups.Length; ++groupIndex )
			{
				Gl.glDrawElements( m_Groups[ groupIndex ].PrimitiveType, m_Groups[ groupIndex ].IndexBuffer.Length, Gl.GL_UNSIGNED_INT, m_Groups[ groupIndex ].IndexBuffer );
			}
		}

		#endregion
	}
}
