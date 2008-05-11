using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{
	class SphereTerrainQuadPatches : IRenderable
	{

		public SphereTerrainQuadPatches( IPlanet planet, TerrainTypeManager terrainTypes, IPlanetTerrain terrain )
		{
			Bitmap terrainTypesBitmap = terrainTypes.ToBitmap( );
			terrainTypesBitmap.Save( planet.Name + " TerrainTypes.bmp", ImageFormat.Bmp );
			m_TerrainTypesTexture = TextureUtils.FromBitmap( terrainTypesBitmap, false );

			m_Planet = planet;

			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanetTerrain.cgfx" );
			TechniqueSelector selector = new TechniqueSelector( effect, "DefaultTechnique" );

			TextureLoader.TextureLoadParameters loadParameters = new TextureLoader.TextureLoadParameters( );
			loadParameters.GenerateMipMaps = true;
			m_TerrainPackTexture = ( ITexture2d )AssetManager.Instance.Load( "Textures/Terrain Packs/TerrainPack0.jpg", loadParameters );

			m_NoiseTexture = ( ITexture2d )AssetManager.Instance.Load( "Textures/Terrain Packs/Noise.jpg" );

			m_Terrain = terrain;
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
			int res = 1;
			CubeSide[] sides = new CubeSide[]
				{
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 7 ], cubePoints[ 6 ], cubePoints[ 4 ] ),	//	+z
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 0 ], cubePoints[ 1 ], cubePoints[ 3 ] ),	//	-z
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 4 ], cubePoints[ 5 ], cubePoints[ 0 ] ),	//	+y
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 6 ], cubePoints[ 7 ], cubePoints[ 2 ] ),	//	-y
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 5 ], cubePoints[ 6 ], cubePoints[ 1 ] ),	//	+x
					new CubeSide( m_Patches, m_Vertices, res, cubePoints[ 0 ], cubePoints[ 3 ], cubePoints[ 4 ] ),	//	-x
				};

#if DEBUG
			DebugInfo.PropertyChanged +=
				delegate
				{
					selector.Select( DebugInfo.ShowPatchWireframe ? "WireFrameTechnique" : "DefaultTechnique" );
				};
#endif
		}

		/// <summary>
		/// Renders the planet's visible terrain patches
		/// </summary>
		public void Render( IRenderContext context )
		{
			UpdateLod( UniCamera.Current );

			Graphics.Renderer.PushTransform( TransformType.LocalToWorld );
			{
				IUniCamera curCam = UniCamera.Current;
				UniTransform transform = m_Planet.Transform;
				float x = ( float )( UniUnits.RenderUnits.FromUniUnits( transform.Position.X - curCam.Position.X ) );
				float y = ( float )( UniUnits.RenderUnits.FromUniUnits( transform.Position.Y - curCam.Position.Y ) );
				float z = ( float )( UniUnits.RenderUnits.FromUniUnits( transform.Position.Z - curCam.Position.Z ) );

				Graphics.Renderer.SetTransform( TransformType.LocalToWorld, new Point3( x, y, z ), transform.XAxis, transform.YAxis, transform.ZAxis );
			}

			m_PlanetTerrainTechnique.Effect.Parameters[ "TerrainPackTexture" ].Set( m_TerrainPackTexture );
			m_PlanetTerrainTechnique.Effect.Parameters[ "TerrainTypeTexture" ].Set( m_TerrainTypesTexture );
			m_PlanetTerrainTechnique.Effect.Parameters[ "NoiseTexture" ].Set( m_NoiseTexture );

			m_Vertices.VertexBuffer.Begin( );
			context.ApplyTechnique( m_PlanetTerrainTechnique, RenderPatches );
			m_Vertices.VertexBuffer.End( );

			if ( DebugInfo.ShowPatchInfo )
			{
				foreach ( TerrainQuadPatch patch in m_Patches )
				{
					patch.DebugRender( );
				}
			}

			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );

			DebugText.Write( "Camera Pos: {0}", UniCamera.Current.Position.ToRenderUnitString( ) );
			DebugText.Write( "Planet Pos: {0}", m_Planet.Transform.Position.ToRenderUnitString( ) );
		}

		#region CubeSide Private Class

		private class CubeSide
		{
			public CubeSide( ICollection<TerrainQuadPatch> allPatches, TerrainQuadPatchVertices vertices, int res, Point3 topLeft, Point3 topRight, Point3 bottomLeft )
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
						Color colour = ( col + row ) % 2 == 0 ? Color.Red : Color.Black;

						TerrainQuadPatch newPatch = new TerrainQuadPatch( vertices, colour, curPos, xAxis, yAxis );
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
				patch.UpdateLod( localPos );
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
				patch.Render( );
			}
		}

		#endregion
	}
}
