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

namespace Poc1.Universe.Classes.Rendering
{
	class SphereTerrainQuadPatches : IRenderable
	{

		public SphereTerrainQuadPatches( IPlanet planet, IPlanetTerrain terrain, ICubeMapTexture planetTexture )
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
			int res = 1;
			int defaultLodLevel = TerrainPatchGeometryManager.LowestDetailLodLevel;
			CubeSide[] sides = new CubeSide[]
				{
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 0 ], cubePoints[ 1 ], cubePoints[ 3 ], defaultLodLevel, true ),	//	-z
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 7 ], cubePoints[ 6 ], cubePoints[ 4 ], defaultLodLevel, true ),	//	+z
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 4 ], cubePoints[ 5 ], cubePoints[ 0 ], defaultLodLevel, true ),	//	+y
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 6 ], cubePoints[ 7 ], cubePoints[ 2 ], defaultLodLevel, true ),	//	-y
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 0 ], cubePoints[ 3 ], cubePoints[ 4 ], defaultLodLevel, true ),	//	-x
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 5 ], cubePoints[ 6 ], cubePoints[ 1 ], defaultLodLevel, true )	//	+x
				};
		}

		/// <summary>
		/// Renders the planet's visible terrain patches
		/// </summary>
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


		#region CubeSide Private Class

		private class CubeSide
		{
			public CubeSide( ICollection<QuadPatch> allPatches, QuadPatchVertices vertices, int res, Point3 topLeft, Point3 topRight, Point3 bottomLeft, int defaultPatchLodLevel, bool defaultVisibility )
			{
				Vector3 xAxis = ( topRight - topLeft ).MakeNormal( );
				Vector3 yAxis = ( bottomLeft - topLeft ).MakeNormal( );
				Vector3 xInc = ( topRight - topLeft ) / res;
				Vector3 yInc = ( bottomLeft - topLeft ) / res;
				Point3 rowStart = topLeft;

				for ( int row = 0; row < res; ++row )
				{
					Point3 curPos = rowStart;
					for ( int col = 0; col < res; ++col )
					{
						System.Drawing.Color colour = ( col + row ) % 2 == 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

						QuadPatch newPatch = new QuadPatch( vertices, colour, curPos, xAxis, yAxis );
						allPatches.Add( newPatch );
						m_Patches.Add( newPatch );

						curPos += xInc;
					}
					rowStart += yInc;
				}
			}

			private List<QuadPatch> m_Patches = new List<QuadPatch>( );
		}

		#endregion

		#region Private Members

		private readonly IPlanet m_Planet;
		private readonly ICubeMapTexture m_PlanetTexture;
		private readonly QuadPatchVertices m_Vertices = new QuadPatchVertices( );
		private readonly IPlanetTerrain m_Terrain;
		private readonly ITechnique m_PlanetTerrainTechnique;
		private readonly List<QuadPatch> m_Patches = new List<QuadPatch>( );
		private readonly ITerrainPatchGeometryManager m_GeometryManager = new TerrainPatchGeometryManager( );

		private static double AccurateDistance( Point3 pt0, Point3 pt1 )
		{
			double x = pt1.X - pt0.X;
			double y = pt1.Y - pt0.Y;
			double z = pt1.Z - pt0.Z;

			return Math.Sqrt( x * x + y * y + z * z );
		}

		private void UpdateLod( IUniCamera camera, float viewportHeight )
		{
			Point3 localPos = UniUnits.RenderUnits.MakeRelativePoint( m_Planet.Transform.Position, camera.Position );

			//	TODO: AP: LOD determination can happen at a slower rate than patch rendering (maybe even in a separate thread)
			List<QuadPatch> changedPatches = new List<QuadPatch>( );
			foreach ( QuadPatch patch in m_Patches )
			{
				double dist = AccurateDistance( patch.PatchCentre, localPos );
				patch.UpdateLod( camera, viewportHeight, ( float )dist, changedPatches );
			}
			foreach ( QuadPatch patch in changedPatches )
			{
				patch.Update( camera, m_Terrain );
			}
		}

		/// <summary>
		/// Renders all the visible patches
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void RenderPatches( IRenderContext context )
		{
			foreach ( QuadPatch patch in m_Patches )
			{
				patch.Render( );
			}
		}

		#endregion
	}
}
