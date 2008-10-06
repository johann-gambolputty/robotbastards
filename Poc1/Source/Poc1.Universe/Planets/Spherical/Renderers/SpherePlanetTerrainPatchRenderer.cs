using System;
using System.Collections.Generic;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Renderers.Patches;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Poc1.Universe.Planets.Spherical.Renderers.Patches;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using IPlanet=Poc1.Universe.Interfaces.Planets.IPlanet;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Terrain renderer for spherical planets
	/// </summary>
	public class SpherePlanetTerrainPatchRenderer : ISpherePlanetTerrainRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetTerrainPatchRenderer( )
		{
			m_NoiseTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/TiledNoise.noise.jpg" );

			m_Effect = new EffectAssetHandle( "Effects/Planets/terrestrialPlanetTerrain.cgfx", true );
			m_Effect.OnReload += Effect_OnReload;
			m_Technique = new TechniqueSelector( m_Effect, "DefaultTechnique" );
		//	m_Technique = new TechniqueSelector( m_Effect, "WireFrameTechnique" );	
		}

		#region Public Members

		/// <summary>
		/// Gets/sets the owner planet
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set
			{
				m_Planet = ( ISpherePlanet )value;
				Refresh( );
			}
		}

		#endregion

		#region IPlanetTerrainRenderer Members

		/// <summary>
		/// Refreshes the renderer (empties terrain patch quad trees)
		/// </summary>
		public void Refresh( )
		{
			m_Patches.Clear( );
			GC.Collect( );
			if ( m_Planet == null )
			{
				return;
			}
			m_Vertices.Clear( );
			float uvRes = ( float )( m_Planet.Radius.ToMetres / 1500.0 );
			CreateCubePatches( 20, 1, uvRes );
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the terrain
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( m_Planet == null )
			{
				return;
			}

			GameProfiles.Game.Rendering.PlanetRendering.TerrainRendering.Begin( );

			UpdatePatches( UniCamera.Current );

			ITexture2d packTexture = m_Planet.TerrainModel.TerrainPackTexture;
			ITexture2d typesTexture = m_Planet.TerrainModel.TerrainTypesTexture;
			ISpherePlanetAtmosphereRenderer atmosphereRenderer = m_Planet.SphereAtmosphereRenderer;
			if ( atmosphereRenderer != null )
			{
				atmosphereRenderer.SetupAtmosphereEffectParameters( m_Technique.Effect, true );
			}

			m_Technique.Effect.Parameters[ "PlanetRadius" ].Set( m_Planet.Radius.ToRenderUnits );
			m_Technique.Effect.Parameters[ "PlanetMaximumTerrainHeight" ].Set( m_Planet.TerrainModel.MaximumHeight.ToRenderUnits );
			m_Technique.Effect.Parameters[ "TerrainPackTexture" ].Set( packTexture );
			m_Technique.Effect.Parameters[ "TerrainTypeTexture" ].Set( typesTexture );
			m_Technique.Effect.Parameters[ "NoiseTexture" ].Set( m_NoiseTexture );

			m_Vertices.VertexBuffer.Begin( );
			context.ApplyTechnique( m_Technique, RenderPatches );
			m_Vertices.VertexBuffer.End( );

			GameProfiles.Game.Rendering.PlanetRendering.TerrainRendering.End( );

			//if ( DebugInfo.ShowPatchInfo )
			//{
			//    m_Patches[ 0 ].DebugRender( );
			//}

			if ( DebugInfo.ShowTerrainLeafNodeCount )
			{
			    int leafCount = 0;
			    foreach ( TerrainPatch patch in m_Patches )
			    {
			        leafCount += patch.CountNodesWithVertexData( );
			    }

			    DebugText.Write( "Terrain leaf nodes: {0}", leafCount );
			}
		}

		#endregion

		#region Private Members

		private readonly TerrainPatchVertices	m_Vertices = new TerrainPatchVertices( );
		private readonly ITexture2d				m_NoiseTexture;
		private readonly EffectAssetHandle		m_Effect;
		private ISpherePlanet					m_Planet;
		private TechniqueSelector				m_Technique;
		private readonly List<TerrainPatch>		m_Patches = new List<TerrainPatch>( );

		/// <summary>
		/// Adds patches that cover the side of a cube
		/// </summary>
		/// <param name="res">Patch resolution (res*res patches cover the cube side)</param>
		/// <param name="topLeft">The top left corner of the cube side</param>
		/// <param name="topRight">The top right corner of the cube side</param>
		/// <param name="bottomLeft">The bottom left corner of the cube side</param>
		/// <param name="uvRes">The UV resolution of the cube patch</param>
		private void AddCubeSidePatches( int res, Point3 topLeft, Point3 topRight, Point3 bottomLeft, float uvRes )
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
					TerrainPatch newPatch = new TerrainPatch( m_Vertices, curPos, xInc, yInc, uvRes );
					m_Patches.Add( newPatch );

					curPos += xInc;
				}
				rowStart += yInc;
			}
		}

		/// <summary>
		/// Called when the terrain effect is reloaded
		/// </summary>
		private void Effect_OnReload( ISource source )
		{
			m_Technique = new TechniqueSelector( m_Effect.Asset, m_Technique.Name );
		}

		/// <summary>
		/// Creates 8 corner points for a cube with a half-width of hDim
		/// </summary>
		private static Point3[] CreateCubeCorners( float hDim )
		{
			Point3[] corners = new Point3[]
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
			return corners;
		}

		/// <summary>
		/// Creates patches on each side of a cube
		/// </summary>
		private void CreateCubePatches( float hDim, int res, float uvRes )
		{
			Point3[] corners = CreateCubeCorners( hDim );

			AddCubeSidePatches( res, corners[ 7 ], corners[ 6 ], corners[ 4 ], uvRes );	//	+z
			AddCubeSidePatches( res, corners[ 0 ], corners[ 1 ], corners[ 3 ], uvRes );	//	-z
			AddCubeSidePatches( res, corners[ 4 ], corners[ 5 ], corners[ 0 ], uvRes );	//	+y
			AddCubeSidePatches( res, corners[ 6 ], corners[ 7 ], corners[ 2 ], uvRes );	//	-y
			AddCubeSidePatches( res, corners[ 5 ], corners[ 6 ], corners[ 1 ], uvRes );	//	+x
			AddCubeSidePatches( res, corners[ 0 ], corners[ 3 ], corners[ 4 ], uvRes );	//	-x
		}

		/// <summary>
		/// Renders all patches
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void RenderPatches( IRenderContext context )
		{
			if ( DebugInfo.ShowPendingTerrainBuildItemCount )
			{
				DebugText.Write( "Pending terrain build items: " + TerrainPatchBuilder.PendingBuildItems );
			}
			foreach ( TerrainPatch patch in m_Patches )
			{
				patch.Render( context );
			}	
		}

		/// <summary>
		/// Updates patches prior to rendering
		/// </summary>
		/// <param name="camera">Camera that LOD is calculated relative to</param>
		private void UpdatePatches( IUniCamera camera )
		{
			Point3 localPos = Units.RenderUnits.MakeRelativePoint( m_Planet.Transform.Position, camera.Position );

			ITerrainPatchGenerator generator = ( ITerrainPatchGenerator )m_Planet.TerrainModel;
			foreach ( TerrainPatch patch in m_Patches )
			{
				patch.UpdateLod( localPos, generator, camera );
			}
			foreach ( TerrainPatch patch in m_Patches )
			{
				patch.Update( camera, generator );
			}
		}

		#endregion
	}
}
