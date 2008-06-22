using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.PlanetBuilder
{
	/// <summary>
	/// Mesh representing the sea
	/// </summary>
	class SeaMesh
	{
		public SeaMesh( float width, float depth )
		{
			VertexBufferFormat vbFormat = new VertexBufferFormat( );
			vbFormat.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );
			vbFormat.Add( VertexFieldSemantic.Normal, VertexFieldElementTypeId.Float32, 3 );

			m_Vertices = Graphics.Factory.CreateVertexBuffer( );

		}

		#region Private Members

		#region Vertex Struct

		private struct Vertex
		{
			
		}

		#endregion

		private readonly IVertexBuffer m_Vertices;

		#endregion
	}
}
