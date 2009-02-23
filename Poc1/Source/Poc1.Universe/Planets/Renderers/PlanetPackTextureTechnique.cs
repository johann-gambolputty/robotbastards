using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Renderers
{
	/// <summary>
	/// Applies terrain typed pack texturing
	/// </summary>
	public class PlanetPackTextureTechnique : AbstractTechnique
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public PlanetPackTextureTechnique( )
		{
			m_NoiseTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/TiledNoise.noise.jpg" );

			m_Effect = new EffectAssetHandle( "Effects/Planets/terrestrialPlanetTerrain.cgfx", true );
			m_Effect.OnReload += Effect_OnReload;
			m_Technique = new TechniqueSelector( m_Effect, "DefaultTechnique" );
		}

		#region IPlanetTechnique Members

		/// <summary>
		/// Applies this technique for rendering an object
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="planet">Planet (data source for any technique parameters)</param>
		/// <param name="renderable">Object to render</param>
		public void Apply( IRenderContext context, IPlanet planet, IRenderable renderable )
		{
			SetupTerrainEffect( m_Effect, planet );
			renderable.Render( context );
		}

		#endregion

		#region Private Members

		private readonly EffectAssetHandle m_Effect;
		private readonly ITexture m_NoiseTexture;
		private TechniqueSelector m_Technique;

		/// <summary>
		/// Sets up the terrain rendering effect
		/// </summary>
		protected virtual void SetupTerrainEffect( IEffect effect, IPlanet planet )
		{
			IPlanetTerrainPackTextureModel textureModel = ( IPlanetTerrainPackTextureModel )planet.PlanetModel.TerrainModel;
			ITexture2d packTexture = textureModel.TerrainPackTexture;
			ITexture2d typesTexture = textureModel.TerrainTypesTexture;

			effect.Parameters[ "PlanetMaximumTerrainHeight" ].Set( planet.PlanetModel.TerrainModel.MaximumHeight.ToRenderUnits );
			effect.Parameters[ "TerrainPackTexture" ].Set( packTexture );
			effect.Parameters[ "TerrainTypeTexture" ].Set( typesTexture );
			effect.Parameters[ "NoiseTexture" ].Set( m_NoiseTexture );
		}

		/// <summary>
		/// Called when the terrain effect is reloaded
		/// </summary>
		private void Effect_OnReload( ISource source )
		{
			m_Technique = new TechniqueSelector( m_Effect.Asset, m_Technique.Name );
		}

		#endregion
	}
}
