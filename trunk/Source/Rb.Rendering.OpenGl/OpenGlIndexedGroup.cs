using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
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
		public void Draw( )
		{
			Gl.glDrawElements( m_PrimitiveType, m_Indices.Length, Gl.GL_UNSIGNED_INT, m_Indices );
		}

		private readonly int	m_PrimitiveType;
		private readonly int[]	m_Indices;
	}
}
