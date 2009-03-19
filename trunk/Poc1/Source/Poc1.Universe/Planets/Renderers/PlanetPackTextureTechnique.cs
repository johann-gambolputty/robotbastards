using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Utils;
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
		/// <param name="planet">Planet to render</param>
		/// <param name="effectPath">Path to the terrain effect</param>
		public PlanetPackTextureTechnique( IPlanet planet, string effectPath )
		{
			Arguments.CheckNotNull( planet, "planet" );
			Arguments.CheckNotNullOrEmpty( effectPath, "effectPath" );
			m_Planet = planet;
			m_NoiseTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/TiledNoise.noise.jpg" );
			m_Effect = new EffectAssetHandle( effectPath, true );
			m_Effect.OnReload += Effect_OnReload;
			m_Technique = new TechniqueSelector( m_Effect, "DefaultTechnique" );
		}

		/// <summary>
		/// Renders the specified object
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="renderable">Object to render</param>
		public override void Apply( IRenderContext context, IRenderable renderable )
		{
			SetupTerrainEffect( m_Effect, m_Planet );
			renderable.Render( context );
		}

		/// <summary>
		/// Throws a NotSupportedException (can only render IPlanetEnvironmentRenderer objects)
		/// </summary>
		public override void Apply( IRenderContext context, RenderDelegate render )
		{
			SetupTerrainEffect( m_Effect, m_Planet );
			render( context );
		}

		#region Private Members

		private readonly IPlanet m_Planet;
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

			IPlanetAtmosphereRenderer atmosphereRenderer = planet.PlanetRenderer.AtmosphereRenderer;
			if ( atmosphereRenderer != null )
			{
				atmosphereRenderer.SetupAtmosphereEffectParameters( effect, true, false );
			}
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
