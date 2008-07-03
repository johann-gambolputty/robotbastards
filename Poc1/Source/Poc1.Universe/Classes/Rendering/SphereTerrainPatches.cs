using System;
using System.Collections.Generic;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Manages creation and rendering of terrain patches for spherical planets
	/// </summary>
	public class SphereTerrainPatches : IRenderable
	{
		#region Construction

		/// <summary>
		/// Setup constructor
		/// </summary>
		public SphereTerrainPatches( IPlanet planet, IPlanetTerrainModel terrain, ICubeMapTexture planetTexture )
		{
			m_Planet = planet;
			m_PlanetTexture = planetTexture;

			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanetTerrain.cgfx" );
			TechniqueSelector selector = new TechniqueSelector( effect, "DefaultTechnique" );
		//	TechniqueSelector selector = new TechniqueSelector( effect, "WireFrameTechnique" );

			m_Terrain = terrain;
			m_PlanetTerrainTechnique = selector;

			float hDim = 3200;
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
			int res = 15;
			int defaultLodLevel = TerrainPatchGeometryManager.LowestDetailLodLevel;
			CubeSide[] sides = new CubeSide[ ]
				{
					new CubeSide( res, cubePoints[ 0 ], cubePoints[ 1 ], cubePoints[ 3 ], defaultLodLevel, true ),	//	-z
					new CubeSide( res, cubePoints[ 7 ], cubePoints[ 6 ], cubePoints[ 4 ], defaultLodLevel, true ),	//	+z
					new CubeSide( res, cubePoints[ 4 ], cubePoints[ 5 ], cubePoints[ 0 ], defaultLodLevel, true ),	//	+y
					new CubeSide( res, cubePoints[ 6 ], cubePoints[ 7 ], cubePoints[ 2 ], defaultLodLevel, true ),	//	-y
					new CubeSide( res, cubePoints[ 0 ], cubePoints[ 3 ], cubePoints[ 4 ], defaultLodLevel, true ),	//	-x
					new CubeSide( res, cubePoints[ 5 ], cubePoints[ 6 ], cubePoints[ 1 ], defaultLodLevel, true )	//	+x
				};

			foreach ( CubeSide side in sides )
			{
				side.PreBuild( m_GeometryManager );
				foreach ( TerrainPatch patch in side.TerrainPatches )
				{
					m_Patches.Add( patch );
				}
			}

			#region Bobbins

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

			#endregion

			foreach ( CubeSide side in sides )
			{
				side.Build( terrain );
			}
		}

		#endregion

		/// <summary>
		/// Renders the planet's visible terrain patches
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			UpdateLod( UniCamera.Current, Graphics.Renderer.ViewportHeight );

			Graphics.Renderer.PushTransform( TransformType.LocalToWorld );
			{
				IUniCamera curCam = UniCamera.Current;
				UniTransform transform = m_Planet.Transform;
				float x = ( float )( UniUnits.RenderUnits.FromUniUnits( transform.Position.X - curCam.Position.X ) );
				float y = ( float )( UniUnits.RenderUnits.FromUniUnits( transform.Position.Y - curCam.Position.Y ) );
				float z = ( float )( UniUnits.RenderUnits.FromUniUnits( transform.Position.Z - curCam.Position.Z ) );

				Graphics.Renderer.SetTransform( TransformType.LocalToWorld, new Point3( x, y, z ), transform.XAxis, transform.YAxis, transform.ZAxis );
			}

			m_PlanetTerrainTechnique.Effect.Parameters[ "TerrainSampler" ].Set( m_PlanetTexture );

			m_GeometryManager.BeginPatchRendering( );
			context.ApplyTechnique( m_PlanetTerrainTechnique, RenderPatches );
			m_GeometryManager.EndPatchRendering( );
			
			//foreach ( TerrainPatch patch in m_Patches )
			//{
			//    patch.DebugRender( );
			//}

			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );

			Graphics.Fonts.DebugFont.Write( 0, 15, System.Drawing.Color.White, "Camera Pos: {0}", UniCamera.Current.Position.ToRenderUnitString( ) );
			Graphics.Fonts.DebugFont.Write( 0, 30, System.Drawing.Color.White, "Planet Pos: {0}", m_Planet.Transform.Position.ToRenderUnitString( ) );
		}

		#region Private Members

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
			public void PreBuild( ITerrainPatchGeometryManager builder )
			{
				foreach ( TerrainPatch patch in TerrainPatches )
				{
					patch.PreBuild( builder );
				}
			}
			
			/// <summary>
			/// Builds all terrain patches in this side
			/// </summary>
			public void Build( IPlanetTerrainModel terrain )
			{
				foreach ( TerrainPatch patch in TerrainPatches )
				{
					patch.Build( terrain );
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

			//public void Link( CubeSide left, CubeSide top, CubeSide right, CubeSide bottom )
			//{
			//    int res = m_Resolution;
			//    for ( int col = 0; col < res; ++col )
			//    {
			//        TerrainPatch.LinkTopAndBottomPatches( top[ col, res - 1 ], this[ col, 0 ] );
			//        TerrainPatch.LinkTopAndBottomPatches( this[ col, res - 1 ], bottom[ res - ( col + 1 ), res - 1 ] );

			//        TerrainPatch.LinkLeftAndRightPatches( left[ res - 1, col ], this[ 0, col ] );
			//        TerrainPatch.LinkLeftAndRightPatches( this[ 0, col ], right[ res - 1, col ] );
			//    }
			//}

			#region Private Members

			private readonly TerrainPatch[ , ] m_Patches;
			private readonly int m_Resolution;

			#endregion
		}

		#endregion

		private readonly IPlanet m_Planet;
		private readonly ICubeMapTexture m_PlanetTexture;
		private readonly IPlanetTerrainModel m_Terrain;
		private readonly ITechnique m_PlanetTerrainTechnique;
		private readonly List<TerrainPatch> m_Patches = new List<TerrainPatch>( );
		private readonly ITerrainPatchGeometryManager m_GeometryManager = new TerrainPatchGeometryManager( );

		private static double AccurateDistance( Point3 pt0, Point3 pt1 )
		{
			double x = pt1.X - pt0.X;
			double y = pt1.Y - pt0.Y;
			double z = pt1.Z - pt0.Z;

			return Math.Sqrt( x * x + y * y + z * z );
		}

		private void UpdatePatchLod( TerrainPatch patch, Point3 localPos, IProjectionCamera camera, float viewportHeight, List< TerrainPatch > changedPatches )
		{
			double dist = AccurateDistance( patch.Centre, localPos );
			patch.UpdateLod( camera, viewportHeight, ( float )dist, m_GeometryManager, changedPatches );
		}

		private void UpdateLod( IUniCamera camera, float viewportHeight )
		{
			Point3 localPos = UniUnits.RenderUnits.MakeRelativePoint( m_Planet.Transform.Position, camera.Position );

			//	TODO: AP: LOD determination can happen at a slower rate than patch rendering (maybe even in a separate thread)
			List<TerrainPatch> changedPatches = new List<TerrainPatch>( );
			foreach ( TerrainPatch patch in m_Patches )
			{
				UpdatePatchLod( patch, localPos, camera, viewportHeight, changedPatches );
			}
			foreach ( TerrainPatch patch in changedPatches )
			{
				patch.PreBuild( m_GeometryManager );
			}
			foreach ( TerrainPatch patch in changedPatches )
			{
				patch.Build( m_Terrain );
			}
		}

		/// <summary>
		/// Renders all the visible patches
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void RenderPatches( IRenderContext context )
		{
			foreach ( TerrainPatch patch in m_Patches )
			{
				patch.Render( );
			}
		}

		#endregion
	}
}
