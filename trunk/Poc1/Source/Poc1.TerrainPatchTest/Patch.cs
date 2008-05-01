using System;
using System.Collections.Generic;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.TerrainPatchTest
{
	/// <summary>
	/// Terrain patch
	/// </summary>
	internal class Patch : IRenderable
	{
		public Patch( Terrain terrain )
		{
			m_Terrain = terrain;

			VertexBufferFormat format = new VertexBufferFormat( );
			format.Add( VertexFieldSemantic.Position, VertexFieldElementTypeId.Float32, 3 );

			int maxSize = GetLevelResolution( 0 );
			m_Vb.Create( format, maxSize * maxSize );

			m_Rs.FaceRenderMode = PolygonRenderMode.Lines;

			for ( int i = 0; i < m_IncreaseDetailDistances.Length; ++i )
			{
				m_IncreaseDetailDistances[ i ] = float.MaxValue;
			}

			m_IncreaseDetailDistances[ m_Level ] = Build( terrain, m_Level );
		}

		public void Update( Point3 cameraPos )
		{
			float distToPatch = cameraPos.DistanceTo( Point3.Origin );
			if ( distToPatch < m_IncreaseDetailDistances[ m_Level ] )
			{
				IncreaseDetail( );
			}
			else if ( distToPatch > m_IncreaseDetailDistances[ m_Level + 1 ] )
			{
				ReduceDetail( );
			}
		}

		public void Render( IRenderContext context )
		{
			Update( ( ( Camera3 )Graphics.Renderer.Camera ).Frame.Translation );

			m_Rs.Begin( );
			m_Vb.Begin( );
			m_Ib.Draw( PrimitiveType.TriList );
			m_Vb.End( );
			m_Rs.End( );
		}

		private static int GetLevelResolution( int level )
		{
			return ( int )Math.Pow( 2, 1 + ( LowestLod - level ) ) ;
		}

		private const float Width = 32;
		private const float Height = 32;
		private const int LowestLod = 5;
		private const float ErrorThreshold = 4.0f;

		private readonly Terrain m_Terrain;
		private int m_Level = LowestLod;
		private readonly float[] m_IncreaseDetailDistances = new float[ LowestLod + 2 ];

		private readonly IRenderState m_Rs = Graphics.Factory.CreateRenderState( );
		private readonly IVertexBuffer m_Vb = Graphics.Factory.CreateVertexBuffer( );
		private readonly IIndexBuffer m_Ib = Graphics.Factory.CreateIndexBuffer( );

		private float Build( Terrain terrain, int level )
		{
			int res = GetLevelResolution( level );
			BuildIndices( res );
			float maxError = BuildVertices( terrain, res );
			return DistanceFromError( maxError );
		}

		private static float DistanceFromError( float error )
		{
			//	Extract frustum from projection matrix:
		//	http://www.opengl.org/discussion_boards/ubbthreads.php?ubb=showflat&Number=209274
			float top = 1.0f;
			float a = 400 / top; // ((ProjectionCamera)Graphics.Renderer.Camera).PerspectiveZNear / top;
			float t = ( ErrorThreshold * 2 ) / Graphics.Renderer.ViewportHeight;
			float c = a / t;
			float d = error * c;

			return d;
		}

		private void IncreaseDetail( )
		{
			if ( m_Level <= 0 )
			{
				return;
			}
			--m_Level;
			m_IncreaseDetailDistances[ m_Level ] = Build( m_Terrain, m_Level );
		}
		
		private void ReduceDetail( )
		{
			if ( m_Level >= LowestLod )
			{
				return;
			}
			++m_Level;
		}

		private void BuildIndices( int res )
		{
			List<int> indices = new List<int>( res * res * 3 );
		
			for ( int row = 0; row < res - 1; ++row )
			{
				int index = ( row * res );
				for ( int col = 0; col < res - 1; ++col )
				{
					indices.Add( index );
					indices.Add( index + 1 );
					indices.Add( index + res );

					indices.Add( index + 1 );
					indices.Add( index + 1 + res );
					indices.Add( index + res );

					++index;
				}
			}

			m_Ib.Create( indices.ToArray( ), true );
		}

		private unsafe float BuildVertices( Terrain terrain, int res )
		{
			float maxError = 0;
			using ( IVertexBufferLock vbLock = m_Vb.Lock( 0, res * res, false, true ) )
			{
				PatchVertex* vertices = ( PatchVertex* )vbLock.Bytes;
				PatchVertex* curVertex = vertices;

				float incX = Width / ( res - 1 );
				float incZ = Height / ( res - 1 );
				float z = -Height / 2;
				float incN = 1.0f / ( res - 1 );
				float nZ = 0;
				for ( int row = 0; row < res; ++row, z += incZ, nZ += incN )
				{
					float x = -Width / 2;
					float nX = 0;
					for ( int col = 0; col < res; ++col, x += incX, nX += incN )
					{
						float curHeight = terrain.GetHeight( nX, nZ );
						float nextHeight = terrain.GetHeight( nX + incN, nZ );
						float estHeight = ( curHeight + nextHeight ) / 2;
						float error = Math.Abs( terrain.GetHeight( nX + incN / 2, nZ ) - estHeight );
						maxError = error > maxError ? error : maxError;

						curVertex->Position = new Point3( x, terrain.GetHeight( nX, nZ ), z );
						++curVertex;
					}
				}
			}
			return maxError;
		}
	}
}
