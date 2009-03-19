using System;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Poc1.Universe.Planets.Models;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Core.Threading;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Spherical planet marble renderer
	/// </summary>
	public class SpherePlanetMarbleRenderer : AbstractSpherePlanetEnvironmentRenderer, IPlanetMarbleRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SpherePlanetMarbleRenderer( ISpherePlanetMarbleTextureBuilder textureBuilder )
		{
			m_TextureBuilder = textureBuilder;
			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/marbleSpherePlanet.cgfx" );
			m_Technique = new TechniqueSelector( effect, "DefaultTechnique" );
		}

		#region IRenderable Members
		
		/// <summary>
		/// Renders the planet
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <remarks>
		/// Expects that the planet's entity transform has been applied, using astro-render units, and that
		/// the scene is being rendered using the astro-render camera.
		/// </remarks>
		public override void Render( IRenderContext context )
		{
			if ( Planet == null )
			{
				return;
			}
			using ( GameProfiles.Game.Rendering.PlanetRendering.FlatPlanetRendering.Enter( ) )
			{
				if ( m_Geometry == null )
				{
					BuildGeometry( );
				}
				UpdateMarbleTexture( );
				if ( Planet.PlanetRenderer.CloudRenderer != null )
				{
					Planet.PlanetRenderer.CloudRenderer.SetupCloudEffectParameters( m_Technique.Effect );
				}
				if ( m_MarbleTexture != null )
				{
					m_Technique.Effect.Parameters[ "MarbleTexture" ].Set( m_MarbleTexture );
				}
				IPlanetTerrainPackTextureModel textureModel = ( IPlanetTerrainPackTextureModel )SpherePlanet.PlanetModel.TerrainModel;
				m_Technique.Effect.Parameters[ "TerrainPackTexture" ].Set( textureModel.TerrainPackTexture );
				m_Technique.Effect.Parameters[ "TerrainTypeTexture" ].Set( textureModel.TerrainTypesTexture );
				m_Technique.Apply( context, m_Geometry );
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Assigns/unassigns this renderer to/from a planet
		/// </summary>
		protected override void AssignToPlanet( IPlanetRenderer renderer, bool remove )
		{
			renderer.MarbleRenderer = remove ? null : this;

			m_MarbleTexture = null;
			m_MarbleTextureDirty = true;
			m_MarbleTextureBuilding = false;
			m_Geometry = null;

			if ( !remove )
			{
			}
		}

		#endregion
		#region Private Members

		private ITexture					m_MarbleTexture;
		private bool						m_MarbleTextureDirty;
		private bool						m_MarbleTextureBuilding;
		private IRenderable					m_Geometry;
		private readonly TechniqueSelector	m_Technique;
		private readonly ISpherePlanetMarbleTextureBuilder m_TextureBuilder;

		/// <summary>
		/// Event, invoked when the planet changed
		/// </summary>
		private void OnPlanetChanged( object sender, EventArgs args )
		{
			//	Discard the current marble geometry. It will be re-created on the next Render() call
			IDisposable disposableGeometry = m_Geometry as IDisposable;
			if ( disposableGeometry != null )
			{
				disposableGeometry.Dispose( );
			}
			m_Geometry = null;
		}

		/// <summary>
		/// Event, invoked when the terrain model changes
		/// </summary>
		private void OnTerrainModelChanged( object sender, EventArgs args )
		{
			PlanetTerrainModelChangedEventArgs modelChangedArgs = args as PlanetTerrainModelChangedEventArgs;
			if ( ( modelChangedArgs == null ) || ( modelChangedArgs.GeometryChanged ) )
			{
				DisposableHelper.Dispose( m_MarbleTexture );
				m_MarbleTexture = null;
				m_MarbleTextureDirty = true;
			}
		}

		/// <summary>
		/// Builds marble geometry
		/// </summary>
		private void BuildGeometry( )
		{
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, ( float )SpherePlanet.PlanetModel.Radius.ToAstroRenderUnits, 40, 40 );
			m_Geometry = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Called when the texture builder has finished building the marble texture
		/// </summary>
		private void OnMarbleTextureBuilt( ITexture marbleTexture )
		{
			m_MarbleTexture = marbleTexture;
			m_MarbleTextureBuilding = false;
		}

		/// <summary>
		/// Updates the marble texture, if necessary
		/// </summary>
		private void UpdateMarbleTexture( )
		{
			if ( ( ( m_MarbleTexture == null ) || m_MarbleTextureDirty ) && !m_MarbleTextureBuilding )
			{
				if ( SpherePlanet.PlanetModel.SphereTerrainModel.ReadyToUse )
				{
					m_MarbleTextureDirty = false;
					m_MarbleTextureBuilding = true;
					m_TextureBuilder.QueueBuild( ExtendedThreadPool.Instance, SpherePlanet, OnMarbleTextureBuilt );
				}
			}
		}

		#endregion
	}
}
