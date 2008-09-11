using System.Collections.Generic;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Spherical.Renderers.Patches
{
	public class TerrainPatchVertices
	{
		/// <summary>
		/// Maximum number of patches that can be supported by the pool
		/// </summary>
		public const int PoolSize = 300;

		/// <summary>
		/// Default constructor
		/// </summary>
		public TerrainPatchVertices( )
		{
			VertexBufferFormat format = new VertexBufferFormat( );
			format.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );
			format.Add( VertexFieldSemantic.Normal, VertexFieldElementTypeId.Float32, 3 );
			format.Add( VertexFieldSemantic.Texture0, VertexFieldElementTypeId.Float32, 2 );
			format.Add( VertexFieldSemantic.Texture1, VertexFieldElementTypeId.Float32, 2 );

			GraphicsLog.Info( "Creating terrain patch vertex pool using format {0}", format );

			m_Vb = Graphics.Factory.CreateVertexBuffer( );
			m_Vb.Create( format, TerrainPatchConstants.PatchTotalVertexCount * PoolSize );

			int vertexIndex = 0;
			for ( int i = 0; i < PoolSize; ++i )
			{
				m_FreeList.Add( vertexIndex );
				vertexIndex += TerrainPatchConstants.PatchTotalVertexCount;
			}
		}

		/// <summary>
		/// Allocates a range of vertices from the pool
		/// </summary>
		public int Allocate( )
		{
			if ( m_FreeList.Count == 0 )
			{
				if ( !m_LastAllocateFailed )
				{
					GraphicsLog.Warning( "Terrain patch vertex buffer ran out of free space" );
				}
				m_LastAllocateFailed = true;
				return -1;
			}
			m_LastAllocateFailed = false;
			int result = m_FreeList[ 0 ];
			m_FreeList.RemoveAt( 0 );
			return result;
		}

		/// <summary>
		/// Deallocates a vertex range
		/// </summary>
		public void Deallocate( int index )
		{
			m_FreeList.Insert( 0, index );
		}

		/// <summary>
		/// Gets the underlying vertex buffer
		/// </summary>
		public IVertexBuffer VertexBuffer
		{
			get { return m_Vb; }
		}

		#region Private Members

		private bool m_LastAllocateFailed;
		private readonly IVertexBuffer m_Vb;
		public readonly List<int> m_FreeList = new List<int>( );

		#endregion

	}
}
