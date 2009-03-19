using Poc1.Tools.Waves;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Planets.Spherical.Renderers;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renders the sea for a sphere planet
	/// </summary>
	public class SpherePlanetOceanRenderer : AbstractSpherePlanetEnvironmentRenderer, IPlanetOceanRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetOceanRenderer( )
		{
			m_Effect = new EffectAssetHandle( "Effects/Planets/sphereOcean.cgfx", true );
			m_Effect.OnReload += Effect_OnReload;

			m_Technique = new TechniqueSelector( m_Effect, "DefaultTechnique" );

			using ( WaveAnimation animation = ( WaveAnimation )AssetManager.Instance.Load( "Ocean/SimpleWater.waves.bin" ) )
			{
				m_WaveAnimation = new AnimatedTexture2d( animation.ToTextures( true ), 5.0f );
			}

			//	Generate cached sphere for rendering the planet
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, 10.0f, 80, 80 );
			m_OceanGeometry = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Renders the ocean
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IRenderContext context )
		{
			if ( Planet == null || Planet.PlanetModel.OceanModel == null )
			{
				return;
			}

			GameProfiles.Game.Rendering.PlanetRendering.OceanRendering.Begin( );

			float seaLevel = ( SpherePlanet.PlanetModel.Radius + Planet.PlanetModel.OceanModel.SeaLevel ).ToRenderUnits;
			seaLevel /= 10.0f;
			Graphics.Renderer.PushTransform( TransformType.LocalToWorld );
			Graphics.Renderer.Scale( TransformType.LocalToWorld, seaLevel, seaLevel, seaLevel );

			m_WaveAnimation.UpdateAnimation( context.RenderTime );
			m_Technique.Effect.Parameters[ "OceanTexture0" ].Set( m_WaveAnimation.SourceTexture );
			m_Technique.Effect.Parameters[ "OceanTexture1" ].Set( m_WaveAnimation.DestinationTexture );
			m_Technique.Effect.Parameters[ "OceanTextureT" ].Set( m_WaveAnimation.LocalT );

			context.ApplyTechnique( m_Technique, m_OceanGeometry );

			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );

			GameProfiles.Game.Rendering.PlanetRendering.OceanRendering.End( );
		}

		#region Protected Members

		/// <summary>
		/// Assigns/unassigns this renderer to/from a planet
		/// </summary>
		protected override void AssignToPlanet( IPlanetRenderer renderer, bool remove )
		{
			renderer.OceanRenderer = remove ? null : this;
		}

		#endregion

		#region Private Members

		private readonly EffectAssetHandle m_Effect;
		private TechniqueSelector m_Technique;
		private readonly IRenderable m_OceanGeometry;

		private AnimatedTexture2d m_WaveAnimation;

		/// <summary>
		/// Handles reloading ocean renderer effect
		/// </summary>
		private void Effect_OnReload( ISource source )
		{
			m_Technique = new TechniqueSelector( m_Effect.Asset, m_Technique.Name );
		}

		#endregion
	}
}
