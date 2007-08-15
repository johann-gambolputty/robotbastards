using System;
using Tao.OpenGl;

namespace RbOpenGlRendering
{
	/// <summary>
	/// Index buffer and other stuff
	/// </summary>
	public class OpenGlIndexedGroup
	{
		/// <summary>
		/// Index buffer setup
		/// </summary>
		public OpenGlIndexedGroup( int primitiveType, int[] indices )
		{
			m_PrimitiveType = primitiveType;
			m_Indices		= indices;
		}

		/// <summary>
		/// Draws elements in this group
		/// </summary>
		public void		Draw( )
		{
			Gl.glDrawElements( m_PrimitiveType, m_Indices.Length, Gl.GL_UNSIGNED_INT, m_Indices );
		}

		private int		m_PrimitiveType;
		private int[]	m_Indices;
	}
}