using System;
using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.TerrainPatchTest
{
	/// <summary>
	/// Terrain patch
	/// </summary>
	internal class Patch
	{
		public Patch( Terrain terrain, int vbOffset, float x, float z, float w, float d, Color c )
		{
			m_VbOffset = vbOffset;
			m_X = x;
			m_Z = z;
			m_Width = w;
			m_Depth = d;
			m_Terrain = terrain;

			m_RsFilled.Colour = c;

			m_RsLines.FaceRenderMode = PolygonRenderMode.Lines;
			m_RsLines.Colour = Color.Red;
			m_RsLines.DepthOffset = -1.0f;

			for ( int i = 0; i < m_IncreaseDetailDistances.Length; ++i )
			{
				m_IncreaseDetailDistances[ i ] = float.MaxValue;
			}
		}

		public void Link( Patch left, Patch right, Patch up, Patch down )
		{
			m_Left = left;
			m_Right = right;
			m_Up = up;
			m_Down = down;
		}

		public void DetermineLod( Point3 cameraPos )
		{
			DetermineLod( cameraPos.DistanceTo( Point3.Origin ) );
		}

		public void SetLod( int lodLevel )
		{
			if ( lodLevel != m_Level )
			{
				m_RebuildVertices = m_RebuildIndices = true;
			}
			m_Level = lodLevel;
			m_Res = GetLevelResolution( m_Level );
		}
		
		public void DetermineLod( float distToPatch )
		{
			if ( m_Level == -1 )
			{
				m_Level = LowestDetailLod;
				m_Res = GetLevelResolution( m_Level );
				m_RebuildVertices = m_RebuildIndices = true;
				return;
			}

			if ( distToPatch < m_IncreaseDetailDistances[ m_Level ] )
			{
				IncreaseDetail( );
			}
			else if ( distToPatch > m_IncreaseDetailDistances[ m_Level + 1 ] )
			{
				ReduceDetail( );
			}
		}

		public void Update( IVertexBuffer vb )
		{
			if ( m_RebuildVertices )
			{
				float dist = BuildVertices( m_Terrain, vb );
				if ( m_IncreaseDetailDistances[ m_Level ] == float.MaxValue )
				{
					m_IncreaseDetailDistances[ m_Level ] = dist;
				}
				m_RebuildVertices = false;
			}
			if ( m_RebuildIndices )
			{
				BuildIndices( );
				m_RebuildIndices = false;
			}
		}

		public void Render( )
		{
			m_RsFilled.Begin( );
			m_Ib.Draw( PrimitiveType.TriList );
			m_RsFilled.End( );
			
			m_RsLines.Begin( );
			m_Ib.Draw( PrimitiveType.TriList );
			m_RsLines.End( );
		}

		public static int GetLevelResolution( int level )
		{
			return ( int )Math.Pow( 2, 2 + ( LowestDetailLod - level ) ) ;
		}

		public const int HighestDetailLod = 0;
		public const int LowestDetailLod = 4;
		private const float ErrorThreshold = 3.0f;

		private readonly int m_VbOffset;
		private readonly float m_X;
		private readonly float m_Z;
		private readonly float m_Width;
		private readonly float m_Depth;
		private readonly Terrain m_Terrain;
		private int m_Level = -1;
		private readonly float[] m_IncreaseDetailDistances = new float[ LowestDetailLod + 2 ];

		private Patch m_Left;
		private Patch m_Right;
		private Patch m_Up;
		private Patch m_Down;

		private bool m_RebuildVertices;
		private bool m_RebuildIndices;

		private int m_Res;

		private readonly IRenderState m_RsFilled = Graphics.Factory.CreateRenderState();
		private readonly IRenderState m_RsLines = Graphics.Factory.CreateRenderState();
		private readonly IIndexBuffer m_Ib = Graphics.Factory.CreateIndexBuffer( );

		private enum Side
		{
			Left,
			Top,
			Right,
			Bottom
		}
		
		private static Side OppositeSide( Side side )
		{
			switch ( side )
			{
				case Side.Left	: return Side.Right;
				case Side.Top	: return Side.Bottom;
				case Side.Right	: return Side.Left;
				case Side.Bottom: return Side.Top;
			}

			return Side.Right;
		}
		/// <summary>
		/// Gets the start index and next index offset for a given side
		/// </summary>
		private void GetSideVars( Side side, out int firstIndex, out int nextIndexOffset, int offset )
		{
			int vertexOffset = m_VbOffset;
			int res = m_Res;
			switch ( side )
			{
				default:
				case Side.Left:
					firstIndex = vertexOffset + offset;
					nextIndexOffset = res;
					break;
				case Side.Top:
					firstIndex = vertexOffset + res * offset;
					nextIndexOffset = 1;
					break;
				case Side.Right:
					firstIndex = vertexOffset + res - ( offset + 1 );
					nextIndexOffset = res;
					break;
				case Side.Bottom:
					firstIndex = vertexOffset + res * ( res - ( offset + 1 ) );
					nextIndexOffset = 1;
					break;
			}
		}

		/// <summary>
		/// Builds a connection strip between this patch and a neighbour patch
		/// </summary>
		private bool BuildConnectingStripIndexBuffer( Patch neighbour, Side side, ICollection<int> indices )
		{
			if ( neighbour == null )
			{
				return false;
			}
			if ( m_Level <= neighbour.m_Level )
			{
				//	Current patch has higher detail than neighbour patch
				return false;
			}

			int index;
			int nextIndexOffset;
			int neighbourIndex;
			int neighbourNextIndexOffset;

			GetSideVars( side, out index, out nextIndexOffset, 1 );
			neighbour.GetSideVars( OppositeSide( side ), out neighbourIndex, out neighbourNextIndexOffset, 0 );

			int startError = ( int )Functions.Pow( 2, m_Level - neighbour.m_Level );
			int error = startError;
			for ( int count = 1; count < neighbour.m_Res; ++count )
			{
				if ( error == 0 )
				{
					indices.Add( neighbourIndex );
					indices.Add( index );
					indices.Add( index + nextIndexOffset );

					index += nextIndexOffset;
					error = startError;
				}
				indices.Add( neighbourIndex );
				indices.Add( index );
				indices.Add( neighbourIndex + neighbourNextIndexOffset );

				neighbourIndex += neighbourNextIndexOffset;
				--error;
			}
			//indices.Add( neighbourIndex );
			//indices.Add( index );
			//indices.Add( index + nextIndexOffset );

			return true;
		}

		private static float DistanceFromError( float error )
		{
			//	Extract frustum from projection matrix:
			//	http://www.opengl.org/resources/faq/technical/transformations.htm
			//	http://www.opengl.org/discussion_boards/ubbthreads.php?ubb=showflat&Number=209274
			ProjectionCamera camera = ( ( ProjectionCamera )Graphics.Renderer.Camera );
			float near = camera.PerspectiveZNear;
			float top = Functions.Tan( camera.PerspectiveFovDegrees * Constants.DegreesToRadians * 0.5f ) * near;
			float a = near / top;
			float t = ( ErrorThreshold * 2 ) / Graphics.Renderer.ViewportHeight;
			float c = a / t;
			float d = error * c;

			return d;
		}

		private static void ForceNeighbourRebuild( Patch neighbour )
		{
			if ( neighbour != null ) //&& ( neighbour.m_Level <= m_Level ) )
			{
				neighbour.m_RebuildIndices = true;
			}
		}

		private void IncreaseDetail( )
		{
			if ( m_Level <= 0 )
			{
				return;
			}
			--m_Level;
			m_Res = GetLevelResolution( m_Level );
			m_RebuildVertices = m_RebuildIndices = true;

			ForceNeighbourRebuild( m_Left );
			ForceNeighbourRebuild( m_Right );
			ForceNeighbourRebuild( m_Up );
			ForceNeighbourRebuild( m_Down );
		}
		
		private void ReduceDetail( )
		{
			if ( m_Level >= LowestDetailLod )
			{
				return;
			}
			++m_Level;
			m_Res = GetLevelResolution( m_Level );
			m_RebuildVertices = m_RebuildIndices = true;
			
			ForceNeighbourRebuild( m_Left );
			ForceNeighbourRebuild( m_Right );
			ForceNeighbourRebuild( m_Up );
			ForceNeighbourRebuild( m_Down );
		}

		private void BuildIndices( )
		{
			int res = m_Res;
			List<int> indices = new List<int>( res * res * 3 );

			//	Add connecting strips to patches of lower levels of detail
			bool addedLeftStrip		= BuildConnectingStripIndexBuffer( m_Left, Side.Left, indices );
			bool addedTopStrip		= BuildConnectingStripIndexBuffer( m_Up, Side.Top, indices );
			bool addedRightStrip	= BuildConnectingStripIndexBuffer( m_Right, Side.Right, indices );
			bool addedBottomStrip	= BuildConnectingStripIndexBuffer( m_Down, Side.Bottom, indices );

			int startRow = addedTopStrip ? 1 : 0;
			int endRow = addedBottomStrip ? res - 2 : res - 1;
			int startCol = addedLeftStrip ? 1 : 0;
			int endCol = addedRightStrip ? res - 2 : res - 1;

			for ( int row = startRow; row < endRow; ++row )
			{
				int index = m_VbOffset + ( row * res ) + startCol;
				for ( int col = startCol; col < endCol; ++col )
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

		private unsafe float BuildVertices( Terrain terrain, IVertexBuffer vb )
		{
			int res = m_Res;

			float maxError = 0;
			using ( IVertexBufferLock vbLock = vb.Lock( m_VbOffset, res * res, false, true ) )
			{
				PatchVertex* vertices = ( PatchVertex* )vbLock.Bytes;
				PatchVertex* curVertex = vertices;

				float incX = m_Width / ( res - 1 );
				float incZ = m_Depth / ( res - 1 );
				float z = m_Z;
				for ( int row = 0; row < res; ++row, z += incZ )
				{
					float x = m_X;
					for ( int col = 0; col < res; ++col, x += incX )
					{
						float curHeight = m_Terrain.GetHeight(x, z);
						float nextHeightX = m_Terrain.GetHeight(x + incX, z);
						float nextHeightY = m_Terrain.GetHeight(x, z + incZ);

						float estXHeight = (curHeight + nextHeightX) / 2;
						float estYHeight = (curHeight + nextHeightY) / 2;
						float xError = Math.Abs(m_Terrain.GetHeight(x + incX / 2, z) - estXHeight);
						float yError = Math.Abs(m_Terrain.GetHeight(x, z + incZ / 2) - estYHeight);
						float error = Utils.Max( xError, yError );
						maxError = error > maxError ? error : maxError;

						curVertex->Position = new Point3( x, curHeight, z );
						++curVertex;
					}
				}
			}
			return DistanceFromError( maxError );
		}
	}
}
