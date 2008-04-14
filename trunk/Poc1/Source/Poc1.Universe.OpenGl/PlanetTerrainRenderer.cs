
using System;
using System.Collections.Generic;
using Poc1.Universe.Classes;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.OpenGl
{
	/// <summary>
	/// Only one planet at a time can be close enough to render using terrain patches. This class manages the rendering of the planet
	/// </summary>
	public class PlanetTerrainRenderer
	{
		#region CubeSide Private Class

		/// <summary>
		/// Side of a planetary cube
		/// </summary>
		private class CubeSide
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public CubeSide( int res, Point3 topLeft, Point3 topRight, Point3 bottomLeft, int defaultPatchLodLevel, bool defaultVisibility )
			{
				if ( ( res % 2 ) == 0 )
				{
					throw new ArgumentException( "Cube side resolution must be odd" );
				}

				m_Resolution = res;
				m_Patches = new TerrainPatch[ res, res ];
				
				Point3 rowStart = topLeft;
				Vector3 xInc = ( topRight - topLeft ) / res;
				Vector3 yInc = ( bottomLeft - topLeft ) / res;

				//	Create all the terrain patches and set their bounds
				for ( int row = 0; row < res; ++row )
				{
					Point3 curPos = rowStart;
					for ( int col = 0; col < res; ++col )
					{
						TerrainPatch newPatch = new TerrainPatch( );
						newPatch.LodLevel = defaultPatchLodLevel;
						newPatch.SetBounds( curPos, curPos + xInc, curPos + yInc );
						m_Patches[ col, row ] = newPatch;
						curPos += xInc;
					}
					rowStart += yInc;
				}

				//	Link patches (this does not link edge patches to other cube side patches - this is done
				//	by calling LinkToSide())
				int finalCol = res - 1;
				int finalRow = res - 1;
				for ( int row = 0; row < res; ++row )
				{
					for ( int col = 0; col < res; ++col )
					{
						TerrainPatch curPatch = m_Patches[ col, row ];
						curPatch.LeftPatch = col == 0 ? null : m_Patches[ col - 1, row ];
						curPatch.TopPatch = row == 0 ? null : m_Patches[ col, row - 1 ];
						curPatch.RightPatch = col == finalCol ? null : m_Patches[ col + 1, row ];
						curPatch.BottomPatch = row == finalRow ? null : m_Patches[ col, row + 1 ];
					}
				}

				Visible = defaultVisibility;
			}

			/// <summary>
			/// Pre-builds all terrain patches in this side
			/// </summary>
			public void PreBuild( TerrainPatchBuilder builder )
			{
				foreach ( TerrainPatch patch in TerrainPatches )
				{
					patch.PreBuild( builder );
				}
			}
			
			/// <summary>
			/// Builds all terrain patches in this side
			/// </summary>
			public void Build( TerrainPatchBuilder builder )
			{
				foreach ( TerrainPatch patch in TerrainPatches )
				{
					patch.Build( builder );
				}
			}

			/// <summary>
			/// Gets all the terrain patches in this side
			/// </summary>
			public IEnumerable<TerrainPatch> TerrainPatches
			{
				get
				{
					for ( int row = 0; row < m_Resolution; ++row )
					{
						for ( int col = 0; col < m_Resolution; ++col )
						{
							yield return m_Patches[ col, row ];
						}
					}
				}
			}

			/// <summary>
			/// Gets a patch
			/// </summary>
			public TerrainPatch this[ int col, int row ]
			{
				get { return m_Patches[ col, row ]; }
			}

			/// <summary>
			/// Gets/sets the visibility of this side
			/// </summary>
			public bool Visible
			{
				set
				{
					foreach ( TerrainPatch patch in TerrainPatches )
					{
						patch.Visible = value;
					}
				}
			}

			public void Link( CubeSide left, CubeSide top, CubeSide right, CubeSide bottom )
			{
				int res = m_Resolution;
				for ( int col = 0; col < res; ++col )
				{
				//	TerrainPatch.LinkTopAndBottomPatches( top[ col, res - 1 ], this[ col, 0 ] );
				//	TerrainPatch.LinkTopAndBottomPatches( this[ col, res - 1 ], bottom[ res - ( col + 1 ), res - 1 ] );

				//	TerrainPatch.LinkLeftAndRightPatches( left[ res - 1, col ], this[ 0, col ] );
				//	TerrainPatch.LinkLeftAndRightPatches( this[ 0, col ], right[ res - 1, col ] );
				}
			}

			#region Private Members

			private readonly TerrainPatch[ , ] m_Patches;
			private readonly int m_Resolution;

			#endregion
		}

		#endregion

		private readonly ITechnique m_PlanetTerrainTechnique; 

		public PlanetTerrainRenderer( )
		{
			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanetTerrain.cgfx" );
			TechniqueSelector selector = new TechniqueSelector( effect, "DefaultTechnique" );

			m_PlanetTerrainTechnique = selector;

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
			int res = 3;
			int defaultLodLevel = 0;
			CubeSide[] sides = new CubeSide[ ]
				{
					new CubeSide( res, cubePoints[ 0 ], cubePoints[ 1 ], cubePoints[ 3 ], defaultLodLevel, true ),	//	-z
					new CubeSide( res, cubePoints[ 7 ], cubePoints[ 6 ], cubePoints[ 4 ], defaultLodLevel, true ),	//	+z
					new CubeSide( res, cubePoints[ 4 ], cubePoints[ 5 ], cubePoints[ 0 ], defaultLodLevel, true ),	//	+y
					new CubeSide( res, cubePoints[ 6 ], cubePoints[ 7 ], cubePoints[ 2 ], defaultLodLevel, true ),	//	-y
					new CubeSide( res, cubePoints[ 0 ], cubePoints[ 3 ], cubePoints[ 4 ], defaultLodLevel, true ),	//	-x
					new CubeSide( res, cubePoints[ 5 ], cubePoints[ 6 ], cubePoints[ 1 ], defaultLodLevel, true )		//	+x
				};

			foreach ( CubeSide side in sides )
			{
				side.PreBuild( m_Builder );
				foreach ( TerrainPatch patch in side.TerrainPatches )
				{
					m_Patches.Add( patch );
				}
			}

			//	Arrange patches in a spiral, with the centre patch on side -y at position 0
			//int ringSize = 1;
			//CubeSide bottomSide = sides[ 3 ];
			//for ( int offsetFromCentre = 0; offsetFromCentre < res; ++offsetFromCentre )
			//{
			//    if ( offsetFromCentre == 0 )
			//    {
			//        continue;
			//    }
			//    int start = ( res / 2 ) - offsetFromCentre;

			//    ringSize += 2;
			//}

			//int[] cubeSides = new int[]
			//    {
			//        0, 1, 3,	//	-z
			//        7, 6, 4,	//	+z
			//        4, 5, 0,	//	+y
			//        6, 7, 2,	//	-y
			//        0, 3, 4,	//	-x
			//        5, 6, 1		//	+x
			//    };
			//int[] sideConnections = new int[]
			//    {
			//    //	Left	Top		Right	Bottom
			//        4,		2,		5,		3,		//	Side 0 (-z)
			//    //	0,		0,		0,		0,		//	Side 1 (+z)
			//    //	0,		0,		0,		0,		//	Side 2 (+y)
			//    //	0,		0,		0,		0,		//	Side 3 (-y)
			//    //	0,		0,		0,		0,		//	Side 4 (-x)
			//    //	0,		0,		0,		0,		//	Side 5 (+x)
			//    };

		//	for ( int side = 0; side < cubeSides.Length / 3; ++side )
			//for ( int side = 0; side < 1; ++side )
			//{
			//    CubeSide leftSide	= sides[ sideConnections[ side * 4 ] ];
			//    CubeSide topSide	= sides[ sideConnections[ side * 4 + 1 ] ];
			//    CubeSide rightSide	= sides[ sideConnections[ side * 4 + 2 ] ];
			//    CubeSide bottomSide	= sides[ sideConnections[ side * 4 + 3 ] ];

			//    sides[ side ].Link( leftSide, topSide, rightSide, bottomSide );
			//}

			foreach ( CubeSide side in sides )
			{
				side.Build( m_Builder );
			}

			m_PatchRenderState = Graphics.Factory.CreateRenderState( );
			m_PatchRenderState.DepthTest = true;
			m_PatchRenderState.DepthWrite = true;
			m_PatchRenderState.CullBackFaces = true;
			m_PatchRenderState.FaceRenderMode = PolygonRenderMode.Lines;
		}

		private void RenderPatches( IRenderContext context )
		{
			foreach ( TerrainPatch patch in m_Patches )
			{
				patch.Render( );
			}
		}

		public void Render( IRenderContext context, SpherePlanet planet, ITexture planetTerrainTexture )
		{
			Graphics.Renderer.PushTransform( TransformType.LocalToWorld );
			{
				IUniCamera curCam = UniCamera.Current;
				UniTransform transform = planet.Transform;
				double scale = 1.0 / 100000.0;
				float x = ( float )( UniUnits.ToMetres( transform.Position.X - curCam.Position.X ) * scale );
				float y = ( float )( UniUnits.ToMetres( transform.Position.Y - curCam.Position.Y ) * scale );
				float z = ( float )( UniUnits.ToMetres( transform.Position.Z - curCam.Position.Z ) * scale );

				Graphics.Renderer.SetTransform( TransformType.LocalToWorld, new Point3( x, y, z ), transform.XAxis, transform.YAxis, transform.ZAxis );

				float radius = ( float )( ( UniUnits.ToMetres( planet.Radius ) * scale ) / TerrainPatch.PlanetRadius );
				Graphics.Renderer.Scale( TransformType.LocalToWorld, radius, radius, radius );
			}

			m_PlanetTerrainTechnique.Effect.Parameters[ "TerrainSampler" ].Set( planetTerrainTexture );

			m_Builder.BeginPatchRendering( );
			context.ApplyTechnique( m_PlanetTerrainTechnique, RenderPatches );
			m_Builder.EndPatchRendering( );

		//	Graphics.Renderer.PushRenderState( m_PatchRenderState );
		//	Graphics.Renderer.PopRenderState( );
			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
		}

		private readonly IRenderState m_PatchRenderState;
		private readonly List<TerrainPatch> m_Patches = new List<TerrainPatch>( );
		private readonly TerrainPatchBuilder m_Builder = new TerrainPatchBuilder( );
	}
}
