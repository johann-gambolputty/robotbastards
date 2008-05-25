using System.Collections.Generic;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	class TerrainQuadPatchVertices
	{
		public const int PoolSize = 300;

		public TerrainQuadPatchVertices( )
		{
			VertexBufferFormat format = new VertexBufferFormat( );
			format.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );
			format.Add( VertexFieldSemantic.Normal, VertexFieldElementTypeId.Float32, 3 );
			format.Add( VertexFieldSemantic.Texture0, VertexFieldElementTypeId.Float32, 2 );
			format.Add( VertexFieldSemantic.Texture1, VertexFieldElementTypeId.Float32, 2 );

			m_Vb = Graphics.Factory.CreateVertexBuffer( );
			m_Vb.Create( format, TerrainQuadPatch.TotalVerticesPerPatch * PoolSize );

			int vertexIndex = 0;
			for ( int i = 0; i < PoolSize; ++i )
			{
				m_FreeList.Add( vertexIndex );
				vertexIndex += TerrainQuadPatch.TotalVerticesPerPatch;
			}
		}

		public int Allocate( )
		{
			if ( m_FreeList.Count == 0 )
			{
				GraphicsLog.Warning( "QuadPatch vertex buffer ran out of free space" );
				return -1;
			}
			int result = m_FreeList[ 0 ];
			m_FreeList.RemoveAt( 0 );
			return result;
		}

		public void Deallocate( int index )
		{
			m_FreeList.Insert( 0, index );
		}

		public IVertexBuffer VertexBuffer
		{
			get { return m_Vb; }
		}

		#region Private Members

		private readonly IVertexBuffer m_Vb;
		public readonly List<int> m_FreeList = new List<int>( );

		#endregion

	}
}
