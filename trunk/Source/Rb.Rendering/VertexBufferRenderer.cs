using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// A very simple class that calls the Draw() method on a vertex buffer when rendered
	/// </summary>
	public class VertexBufferRenderer : IRenderable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="vb">Vertex buffer to render</param>
		/// <param name="primitive">Type of primitive that orders the vertices in the buffer</param>
		public VertexBufferRenderer( IVertexBuffer vb, PrimitiveType primitive )
		{
			Arguments.CheckNotNull( vb, "vb" );
			m_VertexBuffer = vb;
			m_Primitive = primitive;
		}

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			m_VertexBuffer.Draw( m_Primitive );
		}

		#endregion

		#region Private Members

		private IVertexBuffer m_VertexBuffer;
		private PrimitiveType m_Primitive;

		#endregion
	}
}
