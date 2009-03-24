using Poc1.Core.Classes.Profiling;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Rendering;
using Poc1.Tools.Waves;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Ocean renderer for spherical planets
	/// </summary>
	public class SpherePlanetOceanRenderer : SpherePlanetEnvironmentRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetOceanRenderer( ) :
			this( "Effects/Planets/sphereOcean.cgfx", "Ocean/SimpleWater.waves.bin" )
		{
			
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetOceanRenderer( string effectPath, string waterAnimationPath )
		{
			Arguments.CheckNotNull( effectPath, "effectPath" );
			Arguments.CheckNotNull( waterAnimationPath, "waterAnimationPath" );

			m_Effect = new EffectAssetHandle( effectPath, true );
		//	m_Effect.OnReload += Effect_OnReload;

			m_Technique = new TechniqueSelector( m_Effect, "DefaultTechnique" );

			using ( WaveAnimation animation = ( WaveAnimation )AssetManager.Instance.Load( waterAnimationPath ) )
			{
				m_WaveAnimation = new AnimatedTexture2d( animation.ToTextures( true ), 5.0f );
			}

			//	Generate cached sphere for rendering the planet
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, 10.0f, 80, 80 );
			m_OceanGeometry = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IUniRenderContext context )
		{
			if ( context.CurrentPass != UniRenderPass.CloseObjects )
			{
				return;
			}

			IPlanetOceanModel ocean = GetModel<IPlanetOceanModel>( );
			if ( ocean == null )
			{
				return;
			}

			GameProfiles.Game.Rendering.PlanetRendering.OceanRendering.Begin( );

			float seaLevel = ( Planet.Model.Radius + ocean.SeaLevel ).ToRenderUnits;
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

		private readonly EffectAssetHandle	m_Effect;
		private TechniqueSelector			m_Technique;
		private readonly IRenderable		m_OceanGeometry;
		private AnimatedTexture2d			m_WaveAnimation;


		#endregion
	}
}
