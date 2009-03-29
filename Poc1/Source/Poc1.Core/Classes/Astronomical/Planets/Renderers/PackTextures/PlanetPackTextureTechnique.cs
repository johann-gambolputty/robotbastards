using System;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers.PackTextures;
using Poc1.Core.Interfaces.Rendering;
using Poc1.Core.Interfaces.Rendering.Cameras;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Renderers.PackTextures
{
	/// <summary>
	/// Applies terrain typed pack texturing
	/// </summary>
	public class PlanetPackTextureTechnique : AbstractTechnique
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="textureProvider">Planet texture provider</param>
		/// <param name="effectPath">Path to the terrain effect</param>
		public PlanetPackTextureTechnique( IPackTextureProvider textureProvider, string effectPath )
		{
			Arguments.CheckNotNull( textureProvider, "textureProvider" );
			Arguments.CheckNotNullOrEmpty( effectPath, "effectPath" );

			m_TextureProvider = textureProvider;

			m_NoiseTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/TiledNoise.noise.jpg" );
			m_Effect = new EffectAssetHandle( effectPath, true );
			m_Effect.OnReload += Effect_OnReload;
			m_Technique = new TechniqueSelector( m_Effect, "DefaultTechnique" );
		}

		/// <summary>
		/// Sets the planet used by this technique
		/// </summary>
		public IPlanet Planet
		{
			set
			{
				Arguments.CheckNotNull( value, "value" );
				m_Planet = value;
			}
		}

		/// <summary>
		/// Renders the specified object
		/// </summary>
		/// <param name="context">Rendering context. Must be an <see cref="IUniRenderContext"/></param>
		/// <param name="renderable">Object to render</param>
		public override void Apply( IRenderContext context, IRenderable renderable )
		{
			if ( m_Planet == null )
			{
				throw new InvalidOperationException( "Planet must be set before applying this technique" );
			}
			IUniRenderContext uniRenderContext = ( IUniRenderContext )context;
			SetupTerrainEffect( uniRenderContext.Camera, m_Effect, m_Planet, m_TextureProvider );
			renderable.Render( context );
		}

		/// <summary>
		/// Renders the specified object
		/// </summary>
		/// <param name="context">Rendering context. Must be an <see cref="IUniRenderContext"/></param>
		/// <param name="render">Rendering delegate</param>
		public override void Apply( IRenderContext context, RenderDelegate render )
		{
			if ( m_Planet == null )
			{
				throw new InvalidOperationException( "Planet must be set before applying this technique" );
			}
			IUniRenderContext uniRenderContext = ( IUniRenderContext )context;
			SetupTerrainEffect( uniRenderContext.Camera, m_Effect, m_Planet, m_TextureProvider );
			render( context );
		}

		#region Private Members

		private readonly IPackTextureProvider m_TextureProvider;
		private readonly EffectAssetHandle m_Effect;
		private readonly ITexture m_NoiseTexture;
		private TechniqueSelector m_Technique;
		private IPlanet m_Planet;

		/// <summary>
		/// Sets up the terrain rendering effect
		/// </summary>
		protected virtual void SetupTerrainEffect( IUniCamera camera, IEffect effect, IPlanet planet, IPackTextureProvider textureProvider )
		{
			ITexture packTexture = textureProvider.PackTexture;
			ITexture typesTexture = textureProvider.LookupTexture;

			IPlanetTerrainModel terrainModel = planet.Model.GetModel<IPlanetTerrainModel>( );
			if ( terrainModel == null )
			{
				throw new InvalidOperationException( "Expected planet to contain an IPlanetTerrainModel" );
			}

			effect.Parameters[ "PlanetMaximumTerrainHeight" ].Set( terrainModel.MaximumHeight.ToRenderUnits );
			effect.Parameters[ "TerrainPackTexture" ].Set( packTexture );
			effect.Parameters[ "TerrainTypeTexture" ].Set( typesTexture );
			effect.Parameters[ "NoiseTexture" ].Set( m_NoiseTexture );

			IPlanetAtmosphereRenderer atmosphereRenderer = planet.Model.GetModel<IPlanetAtmosphereRenderer>( );
			if ( atmosphereRenderer != null )
			{
				atmosphereRenderer.SetupObjectEffect( camera, effect, false );
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
