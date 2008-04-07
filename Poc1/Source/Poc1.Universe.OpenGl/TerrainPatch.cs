using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.OpenGl
{
	internal class TerrainPatch
	{
		public TerrainPatch( )
		{
			m_IndexBuffer = Graphics.Factory.CreateIndexBuffer( new IndexBufferFormat( IndexBufferIndexSize.Int32 ), 129 * 129 );
		}

		public unsafe void Build( TerrainPatchBuilder builder )
		{
			using ( IVertexBufferLock vbLock = builder.AllocatePatchVertices( m_Lod ) )
			{
				TerrainVertex* curVertex = ( TerrainVertex* )vbLock.Bytes;
				int size = TerrainPatchBuilder.GetLevelSize( m_Lod );

				float xInc = 1;
				float zInc = 1;
				float z = 0;
				for ( int row = 0; row < size; ++row, z += zInc )
				{
					float x = 0;
					for ( int col = 0; col < size; ++col, x += xInc )
					{
						curVertex->X = x;
						curVertex->Y = 0;
						curVertex->Z = z;
						++curVertex;
					}
				}
			}
		}

		public void Render( )
		{
			m_IndexBuffer.Begin( );
			m_IndexBuffer.End( );
		}

		private readonly IIndexBuffer m_IndexBuffer;
		private int m_Lod;
	}
}
