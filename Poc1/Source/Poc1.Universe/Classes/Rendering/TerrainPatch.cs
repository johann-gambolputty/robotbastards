using System.Collections.Generic;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Terrain patch
	/// </summary>
	internal class TerrainPatch
	{
		#region Public Members

		/// <summary>
		/// Gets the current lod level of the patch
		/// </summary>
		public int LodLevel
		{
			get { return m_Lod; }
			set { m_Lod = value; }
		}

		/// <summary>
		/// Gets the neighbouring patch connected to the left of this one
		/// </summary>
		public TerrainPatch LeftPatch
		{
			get { return m_LeftPatch; }
			set { m_LeftPatch = value; }
		}

		/// <summary>
		/// Gets the neighbouring patch connected to the top of this one
		/// </summary>
		public TerrainPatch TopPatch
		{
			get { return m_TopPatch; }
			set { m_TopPatch = value; }
		}

		/// <summary>
		/// Gets the neighbouring patch connected to the right of this one
		/// </summary>
		public TerrainPatch RightPatch
		{
			get { return m_RightPatch; }
			set { m_RightPatch = value; }
		}

		/// <summary>
		/// Gets the neighbouring patch connected to the bottom of this one
		/// </summary>
		public TerrainPatch BottomPatch
		{
			get { return m_BottomPatch; }
			set { m_BottomPatch = value; }
		}

		/// <summary>
		/// Gets/sets patch visibility
		/// </summary>
		public bool Visible
		{
			get { return m_Visible; }
			set { m_Visible = value; }
		}

		/// <summary>
		/// Sets the patch bounds
		/// </summary>
		public void SetBounds( Point3 topLeft, Point3 topRight, Point3 bottomLeft )
		{
			m_TopLeft = topLeft;
			m_TopRight = topRight;
			m_BottomLeft = bottomLeft;
			m_PatchXDir = m_TopRight - m_TopLeft;
			m_PatchZDir = m_BottomLeft - m_TopLeft;
			m_PatchWidth = m_PatchXDir.Length;
			m_PatchHeight = m_PatchZDir.Length;
			m_PatchXDir /= m_PatchWidth;
			m_PatchZDir /= m_PatchHeight;
		}

		/// <summary>
		/// Called prior to Build(). Allocates memory from the geometry manager
		/// </summary>
		public void PreBuild( ITerrainPatchGeometryManager geometryManager )
		{
			m_Geometry = geometryManager.CreateGeometry( m_Lod );
		}

		/// <summary>
		/// Builds this patch
		/// </summary>
		public unsafe void Build( IPlanetTerrain planetTerrain )
		{
			//	Correct normal calculations
			//http://www.gamedev.net/reference/articles/article2264.asp
			m_Geometry.SetIndexBuffer( PrimitiveType.TriList, BuildIndexBuffer( ) );
			try
			{
				int res = m_Geometry.Resolution;

				TerrainVertex* firstVertex = m_Geometry.LockVertexBuffer( false, true );
				
				Vector3 uStep = m_PatchXDir * ( m_PatchWidth / ( res - 1 ) );
				Vector3 vStep = m_PatchZDir * ( m_PatchHeight / ( res - 1 ) );
				planetTerrain.GenerateTerrainPatchVertices( m_TopLeft, uStep, vStep, res, firstVertex );

				/*
				TerrainVertex* curVertex = firstVertex;

				Vector3 xInc = m_PatchXDir * ( m_PatchWidth / ( res - 1 ) );
				Vector3 zInc = m_PatchZDir * ( m_PatchHeight / ( res - 1 ) );

				Point3 rowStart = m_TopLeft;
				for ( int row = 0; row < res; ++row )
				{
					Point3 curPt = rowStart;
					for ( int col = 0; col < res; ++col )
					{
						planetTerrain.MakeTerrainVertexFromPatchPoint( curPt, curVertex );
						++curVertex;
						curPt += xInc;
					}

					rowStart += zInc;
				}

				for ( int row = 0; row < res - 1; ++row )
				{
					curVertex = firstVertex + ( row * res );
					for ( int col = 0; col < res - 1; ++col )
					{
						Point3 pt0 = curVertex->Position;
						Point3 pt1 = ( curVertex + res )->Position;
						Point3 pt2 = ( curVertex + 1 )->Position;
						Vector3 vec = Vector3.Cross( pt1 - pt0, pt2 - pt0 ).MakeNormal( );

						//	TODO: Make correct normal calculation
						curVertex->SetNormal( vec.X, vec.Y, vec.Z );
						++curVertex;
					}
				}
				*/
			}
			finally
			{
				m_Geometry.UnlockVertexBuffer( );
			}
		}

		/// <summary>
		/// Links two patches by their top and bottom edges
		/// </summary>
		public static void LinkTopAndBottomPatches( TerrainPatch topPatch, TerrainPatch bottomPatch )
		{
			if ( topPatch != null )
			{
				topPatch.BottomPatch = bottomPatch;
			}
			if ( bottomPatch != null )
			{
				bottomPatch.TopPatch = topPatch;
			}
		}

		/// <summary>
		/// Links two patches by their left and right edges
		/// </summary>
		public static void LinkLeftAndRightPatches( TerrainPatch leftPatch, TerrainPatch rightPatch )
		{
			if ( leftPatch != null )
			{
				leftPatch.RightPatch = rightPatch;
			}
			if ( rightPatch != null )
			{
				rightPatch.LeftPatch = leftPatch;
			}
		}

		/// <summary>
		/// Renders this patch
		/// </summary>
		public void Render( )
		{
			if ( !Visible )
			{
				return;
			}
			m_Geometry.Draw( );
		}

		#endregion


		#region Private Members

		private Point3					m_TopLeft;
		private Point3					m_TopRight;
		private Point3					m_BottomLeft;
		private Vector3 				m_PatchXDir;
		private Vector3 				m_PatchZDir;
		private float					m_PatchWidth;
		private float					m_PatchHeight;
		private bool					m_Visible;

		private int						m_Lod = 3;
		private TerrainPatch 			m_LeftPatch;
		private TerrainPatch 			m_TopPatch;
		private TerrainPatch 			m_RightPatch;
		private TerrainPatch 			m_BottomPatch;

		private ITerrainPatchGeometry	m_Geometry;
		
		/// <summary>
		/// Gets the resolution of this patch
		/// </summary>
		private int Resolution
		{
			get { return m_Geometry.Resolution; }
		}

		/// <summary>
		/// Patch sides
		/// </summary>
		private enum Side
		{
			Left,
			Top,
			Right,
			Bottom
		}

		/// <summary>
		/// Gets the side opposite a given side
		/// </summary>
		private static Side OppositeSide( Side side )
		{
			switch ( side )
			{
				case Side.Left: return Side.Right;
				case Side.Top: return Side.Bottom;
				case Side.Right: return Side.Left;
				case Side.Bottom: return Side.Top;
			}

			return Side.Right;
		}

		/// <summary>
		/// Gets the start index and next index offset for a given side
		/// </summary>
		private void GetSideVars( Side side, out int firstIndex, out int nextIndexOffset, int offset )
		{
			int vertexOffset = m_Geometry.FirstVertexIndex;
			int res = m_Geometry.Resolution;
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
		private bool BuildConnectingStripIndexBuffer( TerrainPatch neighbour, Side side, ICollection<int> indices )
		{
			if ( neighbour == null )
			{
				return false;
			}

			if ( LodLevel <= neighbour.LodLevel )
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

			int startError = ( int )Functions.Pow( 2, LodLevel - neighbour.LodLevel );
			int error = startError;
			for ( int count = 1; count < neighbour.Resolution; ++count )
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
			indices.Add( neighbourIndex );
			indices.Add( index );
			indices.Add( index + nextIndexOffset );

			return true;
		}

		/// <summary>
		/// Builds the index buffer for this patch
		/// </summary>
		private int[] BuildIndexBuffer( )
		{
			int res = Resolution;
			int offset = m_Geometry.FirstVertexIndex;
			int numIndices = ( res - 1 ) * ( res - 1 ) * 6;
			List<int> indices = new List<int>( numIndices );

			//	Add connecting strips to patches of lower levels of detail
			bool addedLeftStrip = BuildConnectingStripIndexBuffer( LeftPatch, Side.Left, indices );
			bool addedTopStrip = BuildConnectingStripIndexBuffer( TopPatch, Side.Top, indices );
			bool addedRightStrip = BuildConnectingStripIndexBuffer( RightPatch, Side.Right, indices );
			bool addedBottomStrip = BuildConnectingStripIndexBuffer( BottomPatch, Side.Bottom, indices );

			int startRow = addedTopStrip ? 1 : 0;
			int endRow = addedBottomStrip ? res - 2 : res - 1;
			int startCol = addedLeftStrip ? 1 : 0;
			int endCol = addedRightStrip ? res - 2 : res - 1;

			//	Fill in the central part of the patch
			for ( int row = startRow; row < endRow; ++row )
			{
				int index = offset + ( row * res ) + startCol;
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

			return indices.ToArray( );
		}

		#endregion
	}
}
