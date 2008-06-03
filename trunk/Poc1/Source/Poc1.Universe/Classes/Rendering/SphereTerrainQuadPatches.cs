using System.Collections.Generic;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;

namespace Poc1.Universe.Classes.Rendering
{
	class SphereTerrainQuadPatches : IRenderable
	{

		public SphereTerrainQuadPatches( SpherePlanet planet, IPlanetTerrain terrain )
		{
			m_Planet = planet;

			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanetTerrain.cgfx" );
			TechniqueSelector selector = new TechniqueSelector( effect, "DefaultTechnique" );

			TextureLoader.TextureLoadParameters loadParameters = new TextureLoader.TextureLoadParameters( );
			loadParameters.GenerateMipMaps = true;
			m_TerrainPackTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/defaultSet0 Pack.jpg", loadParameters );
			m_TerrainTypesTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/defaultSet0 Distribution.bmp" );

			m_NoiseTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/Noise.jpg" );

			m_Terrain = terrain;
			m_PlanetTerrainTechnique = selector;

			float uvRes = ( float )( planet.Radius / 500000.0 );

			float hDim = 20;
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
			CubeSide[] sides = new CubeSide[]
				{
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 7 ], cubePoints[ 6 ], cubePoints[ 4 ], uvRes ),	//	+z
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 0 ], cubePoints[ 1 ], cubePoints[ 3 ], uvRes ),	//	-z
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 4 ], cubePoints[ 5 ], cubePoints[ 0 ], uvRes ),	//	+y
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 6 ], cubePoints[ 7 ], cubePoints[ 2 ], uvRes ),	//	-y
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 5 ], cubePoints[ 6 ], cubePoints[ 1 ], uvRes ),	//	+x
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 0 ], cubePoints[ 3 ], cubePoints[ 4 ], uvRes ),	//	-x
				};

#if DEBUG
			DebugInfo.PropertyChanged +=
				delegate
				{
					if ( DebugInfo.ShowPatchWireframe )
					{
						selector.Select( "WireFrameTechnique" );
					}
					else if ( DebugInfo.ShowTerrainSlopes )
					{
						selector.Select( "ShowSlopesTechnique" );
					}
					else
					{
						selector.Select( "DefaultTechnique" );
					}
				};
#endif
		}

		/// <summary>
		/// Renders the planet's visible terrain patches
		/// </summary>
		public void Render( IRenderContext context )
		{
			UpdateLod( UniCamera.Current );

			m_PlanetTerrainTechnique.Effect.Parameters[ "TerrainPackTexture" ].Set( m_TerrainPackTexture );
			m_PlanetTerrainTechnique.Effect.Parameters[ "TerrainTypeTexture" ].Set( m_TerrainTypesTexture );
			m_PlanetTerrainTechnique.Effect.Parameters[ "NoiseTexture" ].Set( m_NoiseTexture );

			m_Vertices.VertexBuffer.Begin( );
			context.ApplyTechnique( m_PlanetTerrainTechnique, RenderPatches );
			m_Vertices.VertexBuffer.End( );

			if ( DebugInfo.ShowPatchInfo )
			{
				m_Patches[ 0 ].DebugRender( );
				//foreach ( TerrainQuadPatch patch in m_Patches )
				//{
				//    patch.DebugRender( );
				//}
			}

			if ( DebugInfo.ShowTerrainLeafNodeCount )
			{
				int leafCount = 0;
				foreach ( TerrainQuadPatch patch in m_Patches )
				{
					leafCount += patch.CountNodesWithVertexData( );
				}

				DebugText.Write( "Terrain leaf nodes: {0}", leafCount );
			}
		}

		#region CubeSide Private Class

		private class CubeSide
		{
			public CubeSide( ICollection<TerrainQuadPatch> allPatches, TerrainQuadPatchVertices vertices, int res, Point3 topLeft, Point3 topRight, Point3 bottomLeft, float uvRes )
			{
				Vector3 xAxis = ( topRight - topLeft );
				Vector3 yAxis = ( bottomLeft - topLeft );
				Vector3 xInc = xAxis / res;
				Vector3 yInc = yAxis / res;
				Point3 rowStart = topLeft;

				for ( int row = 0; row < res; ++row )
				{
					Point3 curPos = rowStart;
					for ( int col = 0; col < res; ++col )
					{
						TerrainQuadPatch newPatch = new TerrainQuadPatch( vertices, curPos, xInc, yInc, uvRes );
						allPatches.Add( newPatch );
						m_Patches.Add( newPatch );

						curPos += xInc;
					}
					rowStart += yInc;
				}
			}

			private readonly List<TerrainQuadPatch> m_Patches = new List<TerrainQuadPatch>( );
		}

		#endregion

		#region Private Members

		private readonly ITexture2d					m_TerrainTypesTexture;
		private readonly ITexture2d					m_TerrainPackTexture;
		private readonly ITexture2d					m_NoiseTexture;
		private readonly IPlanet					m_Planet;
		private readonly TerrainQuadPatchVertices	m_Vertices = new TerrainQuadPatchVertices( );
		private readonly IPlanetTerrain				m_Terrain;
		private readonly TechniqueSelector			m_PlanetTerrainTechnique;
		private readonly List<TerrainQuadPatch>		m_Patches = new List<TerrainQuadPatch>( );


		private void UpdateLod( IUniCamera camera )
		{
			Point3 localPos = UniUnits.RenderUnits.MakeRelativePoint( m_Planet.Transform.Position, camera.Position );

			foreach ( TerrainQuadPatch patch in m_Patches )
			{
				patch.UpdateLod( localPos, m_Terrain, camera );
			}
			foreach ( TerrainQuadPatch patch in m_Patches )
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
			foreach ( TerrainQuadPatch patch in m_Patches )
			{
				patch.Render( context );
			}
		}

		#endregion
	}
}
