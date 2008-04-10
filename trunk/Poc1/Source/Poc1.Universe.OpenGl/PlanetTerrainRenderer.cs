
using System;
using System.Collections.Generic;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.OpenGl
{
	/// <summary>
	/// Only one planet at a time can be close enough to render using terrain patches. This class manages the rendering of the planet
	/// </summary>
	public class PlanetTerrainRenderer
	{
		private TerrainPatch[,] MakePatchSide( int res, Point3 topLeft, Point3 topRight, Point3 bottomLeft, int testLodLevel )
		{
			Vector3 xInc = ( topRight - topLeft ) / res;
			Vector3 yInc = ( bottomLeft - topLeft ) / res;

			Point3 rowStart = topLeft;

			TerrainPatch[,] patches = new TerrainPatch[ res, res ];
			Random rnd = new Random( );
			for ( int row = 0; row < res; ++row )
			{
				Point3 curPos = rowStart;
				for ( int col = 0; col < res; ++col )
				{
					TerrainPatch newPatch = new TerrainPatch( );
					Point3 patchTopLeft = curPos;
					Point3 patchTopRight = curPos + xInc;
					Point3 patchBottomLeft = curPos + yInc;
					Point3 patchBottomRight = topRight + yInc;

					newPatch.SetBounds( patchTopLeft, patchTopRight, patchBottomLeft, patchBottomRight );
				//	newPatch.LodLevel = rnd.Next( TerrainPatchBuilder.MaxLodLevels - 2, TerrainPatchBuilder.MaxLodLevels );
				//	newPatch.LodLevel = rnd.Next( TerrainPatchBuilder.MaxLodLevels - 3, TerrainPatchBuilder.MaxLodLevels );
				//	newPatch.LodLevel = TerrainPatchBuilder.MaxLodLevels - 1;
					newPatch.LodLevel = testLodLevel;
					patches[ col, row ] = newPatch;
					m_Patches.Add( newPatch );

					curPos += xInc;
				}
				rowStart += yInc;
			}

			int finalCol = res - 1;
			int finalRow = res - 1;
			for ( int row = 0; row < res; ++row )
			{
				for ( int col = 0; col < res; ++col )
				{
					TerrainPatch curPatch = patches[ col, row ];
					curPatch.LeftPatch = col == 0 ? null : patches[ col - 1, row ];
					curPatch.TopPatch = row == 0 ? null : patches[ col, row - 1 ];
					curPatch.RightPatch = col == finalCol ? null : patches[ col + 1, row ];
					curPatch.BottomPatch = row == finalRow ? null : patches[ col, row + 1 ];

					curPatch.PreBuild( m_Builder );
				}
			}

			return patches;
		}

		private static void LinkSides( TerrainPatch[ , ] side, TerrainPatch[ , ] leftSide, TerrainPatch[ , ] topSide, TerrainPatch[ , ] rightSide, TerrainPatch[ , ] bottomSide, int res )
		{
			for ( int col = 0; col < res; ++col )
			{
				TerrainPatch.LinkTopAndBottomPatches( topSide[ col, res - 1 ], side[ col, 0 ] );
				TerrainPatch.LinkTopAndBottomPatches( side[ col, res - 1 ], bottomSide[ col, 0 ] );

				TerrainPatch.LinkLeftAndRightPatches( leftSide[ res - 1, col ], side[ 0, col ] );
				TerrainPatch.LinkLeftAndRightPatches( side[ 0, col ], rightSide[ res - 1, col ] );
			}
		}

		public PlanetTerrainRenderer( )
		{
			float hDim = 1;
			Point3[] cubePoints = new Point3[]
				{
					new Point3( -hDim, -hDim, -hDim ),
					new Point3( +hDim, -hDim, -hDim ),
					new Point3( +hDim, +hDim, -hDim ),
					new Point3( -hDim, +hDim, -hDim ),
					
					new Point3( -hDim, -hDim, +hDim ),
					new Point3( +hDim, -hDim, +hDim ),
					new Point3( +hDim, +hDim, +hDim ),
					new Point3( -hDim, +hDim, +hDim ),
				};

			int[] cubeSides = new int[]
				{
					0, 1, 3,	//	-z
					7, 6, 4,	//	+z
					4, 5, 0,	//	+y
					6, 7, 2,	//	-y
					0, 3, 4,	//	-x
					5, 6, 1		//	+x
				};
			int[] sideConnections = new int[]
				{
				//	Left	Top		Right	Bottom
					1,		0,		0,		0,		//	Side 0 (-z)
				//	0,		0,		0,		0,		//	Side 1 (+z)
					0,		0,		0,		0,		//	Side 2 (+y)
				//	0,		0,		0,		0,		//	Side 3 (-y)
				//	0,		0,		0,		0,		//	Side 4 (-x)
				//	0,		0,		0,		0,		//	Side 5 (+x)
				};

			int res = 3;
			TerrainPatch[][,] sidePatches = new TerrainPatch[6][,];
			for ( int side = 0; side < cubeSides.Length / 3; ++side )
			{
				Point3 topLeft = cubePoints[ cubeSides[ side * 3 ] ];
				Point3 topRight = cubePoints[ cubeSides[ side * 3 + 1 ] ];
				Point3 bottomLeft = cubePoints[ cubeSides[ side * 3 + 2 ] ];
				sidePatches[ side ] = MakePatchSide( res, topLeft, topRight, bottomLeft, side == 0 ? 3 : 2 );
			}

		//	for ( int side = 0; side < cubeSides.Length / 3; ++side )
			for ( int side = 0; side < 1; ++side )
			{
				TerrainPatch[ , ] leftPatches = sidePatches[ sideConnections[ side * 4 ] ];
				TerrainPatch[ , ] topPatches = sidePatches[ sideConnections[ side * 4 + 1 ] ];
				TerrainPatch[ , ] rightPatches = sidePatches[ sideConnections[ side * 4 + 2 ] ];
				TerrainPatch[ , ] bottomPatches = sidePatches[ sideConnections[ side * 4 + 3 ] ];
				LinkSides( sidePatches[ side ], leftPatches, topPatches, rightPatches, bottomPatches, res );
			}

			foreach ( TerrainPatch patch in m_Patches )
			{
				patch.Build( m_Builder );
			}
			m_PatchRenderState = Graphics.Factory.CreateRenderState( );
			m_PatchRenderState.DepthTest = false;
			m_PatchRenderState.CullBackFaces = false;
			m_PatchRenderState.FaceRenderMode = PolygonRenderMode.Lines;
		}

		private IRenderState m_PatchRenderState;

		public void Render( IRenderContext context )
		{
			Graphics.Renderer.PushRenderState( m_PatchRenderState );
			m_Builder.BeginPatchRendering( );

			foreach ( TerrainPatch patch in m_Patches )
			{
				patch.Render( );
			}
			m_Builder.EndPatchRendering( );
			Graphics.Renderer.PopRenderState( );
		}

		private readonly List<TerrainPatch> m_Patches = new List<TerrainPatch>( );
		private TerrainPatchBuilder m_Builder = new TerrainPatchBuilder( );
	}
}
