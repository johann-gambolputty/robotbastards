using Poc1.Tools.Waves;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Renders the sea for a sphere planet
	/// </summary>
	public class SpherePlanetOceanRenderer : IPlanetOceanRenderer
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
			Graphics.Draw.Sphere( null, Point3.Origin, 10.0f, 40, 40 );
			m_OceanGeometry = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Gets/sets the planet this renderer belongs to
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = ( ISpherePlanet )value; }
		}

		/// <summary>
		/// Renders the ocean
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( m_Planet == null || m_Planet.PlanetModel.OceanModel == null )
			{
				return;
			}

			GameProfiles.Game.Rendering.PlanetRendering.OceanRendering.Begin( );

			float seaLevel = ( m_Planet.SpherePlanetModel.Radius + m_Planet.SpherePlanetModel.OceanModel.SeaLevel ).ToRenderUnits;
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

		#region Private Members

		private ISpherePlanet m_Planet;
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
