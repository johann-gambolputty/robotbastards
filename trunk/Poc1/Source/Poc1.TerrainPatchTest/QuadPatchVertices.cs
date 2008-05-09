using System;
using System.Collections.Generic;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.TerrainPatchTest
{
	class QuadPatchVertices
	{
		public const int PoolSize = 256;

		public QuadPatchVertices( )
		{
			VertexBufferFormat format = new VertexBufferFormat( );
			format.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );
			
			m_Vb = Graphics.Factory.CreateVertexBuffer( );
			m_Vb.Create( format, QuadPatch.PatchResolution * QuadPatch.PatchResolution * PoolSize );

			int vertexIndex = 0;
			for ( int i = 0; i < PoolSize; ++i )
			{
				m_FreeList.Add( vertexIndex );
				vertexIndex += QuadPatch.PatchResolution * QuadPatch.PatchResolution;
			}
		}

		public int Allocate( )
		{
			if ( m_FreeList.Count == 0 )
			{
				throw new OutOfMemoryException( "QuadPatch vertex buffer ran out of free space" );
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

		private IVertexBuffer m_Vb;

		public readonly List< int > m_FreeList = new List< int >( );

		#endregion
	}
}
