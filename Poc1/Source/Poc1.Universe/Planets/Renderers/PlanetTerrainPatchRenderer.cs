
using System;
using System.Collections.Generic;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Interfaces.Planets.Renderers.Patches;
using Poc1.Universe.Planets.Spherical.Renderers.Patches;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Renderers
{
	/// <summary>
	/// Abstract base class for rendering planetary terrain
	/// </summary>
	public class PlanetTerrainPatchRenderer : IPlanetTerrainRenderer
	{
		/// <summary>
		/// Setup constructor. Loads in shared assets used for rendering
		/// </summary>
		public PlanetTerrainPatchRenderer( string terrainEffectPath )
		{
			Arguments.CheckNotNullOrEmpty( terrainEffectPath, "terrainEffectPath" );
			m_NoiseTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/TiledNoise.noise.jpg" );

			m_Effect = new EffectAssetHandle( "Effects/Planets/terrestrialPlanetTerrain.cgfx", true );
			m_Effect.OnReload += Effect_OnReload;
			m_Technique = new TechniqueSelector( m_Effect, "DefaultTechnique" );
			//	m_Technique = new TechniqueSelector( m_Effect, "WireFrameTechnique" );	
		}

		#region IPlanetTerrainRenderer Members

		/// <summary>
		/// Refreshes the terrain
		/// </summary>
		public virtual void Refresh( )
		{
			m_RootPatches.Clear( );
			m_Vertices.Clear( );
			GC.Collect( );
		}

		#endregion

		#region IPlanetEnvironmentRenderer Members

		/// <summary>
		/// Gets/sets the associated planet
		/// </summary>
		public virtual IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = value; }
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the terrain
		/// </summary>
		/// <param name="context">Terrain rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( m_Planet == null )
			{
				return;
			}

			if ( DebugInfo.ShowPendingTerrainBuildItemCount )
			{
				DebugText.Write( "Pending terrain build items: " + TerrainPatchBuilder.PendingBuildItems );
			}

			GameProfiles.Game.Rendering.PlanetRendering.TerrainRendering.Begin( );

			UpdatePatches( UniCamera.Current );

			IPlanetAtmosphereRenderer atmosphereRenderer = Planet.AtmosphereRenderer;
			if ( atmosphereRenderer != null )
			{
				atmosphereRenderer.SetupAtmosphereEffectParameters( m_Technique.Effect, true, false );
			}

			SetupTerrainEffect( m_Technique.Effect );

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
				foreach ( TerrainPatch patch in m_RootPatches )
				{
					leafCount += patch.CountNodesWithVertexData( );
				}

				DebugText.Write( "Terrain leaf nodes: {0}", leafCount );
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Gets the terrain patch vertex buffer manager
		/// </summary>
		protected TerrainPatchVertices Vertices
		{
			get { return m_Vertices; }
		}

		/// <summary>
		/// Gets the list of root patches
		/// </summary>
		protected List<TerrainPatch> RootPatches
		{
			get { return m_RootPatches; }
		}

		/// <summary>
		/// Sets up the terrain rendering effect
		/// </summary>
		protected virtual void SetupTerrainEffect( IEffect effect )
		{
			ITexture2d packTexture = Planet.TerrainModel.TerrainPackTexture;
			ITexture2d typesTexture = Planet.TerrainModel.TerrainTypesTexture;

			effect.Parameters[ "PlanetMaximumTerrainHeight" ].Set( m_Planet.TerrainModel.MaximumHeight.ToRenderUnits );
			effect.Parameters[ "TerrainPackTexture" ].Set( packTexture );
			effect.Parameters[ "TerrainTypeTexture" ].Set( typesTexture );
			effect.Parameters[ "NoiseTexture" ].Set( m_NoiseTexture );
		}

		#endregion

		#region Private Members

		private readonly TerrainPatchVertices	m_Vertices = new TerrainPatchVertices( );
		private readonly ITexture2d				m_NoiseTexture;
		private readonly EffectAssetHandle		m_Effect;
		private IPlanet							m_Planet;
		private TechniqueSelector				m_Technique;
		private readonly List<TerrainPatch>		m_RootPatches = new List<TerrainPatch>( );

		/// <summary>
		/// Updates patches prior to rendering
		/// </summary>
		/// <param name="camera">Camera that LOD is calculated relative to</param>
		private void UpdatePatches( IUniCamera camera )
		{
			Point3 localPos = Units.RenderUnits.MakeRelativePoint( m_Planet.Transform.Position, camera.Position );

			ITerrainPatchGenerator generator = ( ITerrainPatchGenerator )m_Planet.TerrainModel;
			foreach ( TerrainPatch patch in m_RootPatches )
			{
				patch.UpdateLod( localPos, generator, camera );
			}
			foreach ( TerrainPatch patch in m_RootPatches )
			{
				patch.Update( camera, generator );
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
		/// Renders root patches
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void RenderPatches( IRenderContext context )
		{
			foreach ( TerrainPatch rootPatch in m_RootPatches )
			{
				rootPatch.Render( context );
			}
		}

		#endregion

	}
}
