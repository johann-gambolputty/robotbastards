using System.Collections.Generic;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.OpenGl
{
	internal class TerrainPatch
	{
		public const float PatchWidth = 64;
		public const float PatchHeight = PatchWidth;

		private readonly Matrix44 m_Transform = new Matrix44( );

		public Matrix44 Frame
		{
			get { return m_Transform; }
		}

		private void BuildLeftStripIndexBuffer( int offset, int size, List<int> indices )
		{
			if ( LeftPatch == null )
			{
				return;
			}
			if ( LeftPatch.LodLevel == LodLevel )
			{
				
			}
		}

		private IIndexBuffer BuildIndexBuffer( int offset, int size )
		{
			int numIndices = ( size - 1 ) * ( size - 1 ) * 6;
			List<int> indices = new List<int>( numIndices );

			for ( int row = 0; row < size - 1; ++row )
			{
				int index = offset + ( row * size );
				for ( int col = 0; col < size - 1; ++col )
				{
					indices.Add( index );
					indices.Add( index + 1 );
					indices.Add( index + size );

					indices.Add( index + 1 );
					indices.Add( index + 1 + size );
					indices.Add( index + size );

					++index;
				}
			}

			IndexBufferData data = new IndexBufferData( new IndexBufferFormat( IndexBufferIndexSize.Int32 ), indices.ToArray( ) );
			IIndexBuffer buffer = Graphics.Factory.CreateIndexBuffer( data );

			return buffer;
		}

		private void Destroy( )
		{
			if ( m_IndexBuffer != null )
			{
				m_IndexBuffer.Dispose( );
				m_IndexBuffer = null;
			}
		}

		public unsafe void Build( TerrainPatchBuilder builder )
		{
			Destroy( );

		//	m_IndexBuffer = builder.GetLevelIndexBuffer( m_Lod );
			using ( IVertexBufferLock vbLock = builder.AllocatePatchVertices( m_Lod ) )
			{
				int size = TerrainPatchBuilder.GetLevelSize( m_Lod );
				m_IndexBuffer = BuildIndexBuffer( vbLock.FirstVertexIndex, size );
				TerrainVertex* curVertex = ( TerrainVertex* )vbLock.Bytes;

				float scale = 1;
				float yScale = 10.0f;
				float y = -5;
				float z = ( -PatchHeight / 2 ) * scale;
				float xInc = PatchWidth / size;
				float zInc = PatchHeight / size;
				for ( int row = 0; row < size; ++row, z += zInc )
				{
					float x = ( -PatchWidth / 2 ) * scale;
					for ( int col = 0; col < size; ++col, x += xInc )
					{
						curVertex->X = x;
						curVertex->Y = y + Functions.Sin( col * Constants.TwoPi / size ) * yScale;
						curVertex->Z = z;
						++curVertex;
					}
				}
			}
		}

		public int LodLevel
		{
			get { return m_Lod; }
		}

		public TerrainPatch LeftPatch
		{
			get { return m_LeftPatch; }
			set { m_LeftPatch = value; }
		}

		public TerrainPatch TopPatch
		{
			get { return m_TopPatch; }
			set { m_TopPatch = value; }
		}

		public void Render( )
		{
			Graphics.Renderer.PushTransform( Transform.LocalToWorld, Frame );
			m_IndexBuffer.Draw( PrimitiveType.TriList );
			Graphics.Renderer.PopTransform( Transform.LocalToWorld );
		}

		private IIndexBuffer m_IndexBuffer;
		private int m_Lod = 3;
		private TerrainPatch m_LeftPatch;
		private TerrainPatch m_TopPatch;
	}
}
