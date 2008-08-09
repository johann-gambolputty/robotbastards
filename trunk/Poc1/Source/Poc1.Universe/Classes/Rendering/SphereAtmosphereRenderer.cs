using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Atmosphere renderer for spherical planets
	/// </summary>
	public class SphereAtmosphereRenderer : ISphereAtmosphereRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SphereAtmosphereRenderer( SpherePlanet planet )
		{
			m_Planet = planet;
			m_AtmosphereRadius = planet.SphereAtmosphere.Radius;

			float renderRadius = ( float )UniUnits.RenderUnits.FromUniUnits( planet.Radius + m_AtmosphereRadius );
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, renderRadius, 40, 40 );
			m_Atmosphere = Graphics.Draw.StopCache( );

			//	Load in atmosphere effect
			EffectAssetHandle atmosphereEffect = new EffectAssetHandle( "Effects/Planets/atmosphereShell.cgfx", true );
			atmosphereEffect.OnReload += atmosphereEffect_OnReload;
			m_Techniques = new TechniqueSelector( atmosphereEffect, "DefaultTechnique" );
		}

		#region ISphereAtmosphereRenderer Members

		/// <summary>
		/// Sets the lookup textures required by the atmosphere renderer
		/// </summary>
		/// <param name="scatteringTexture">Lookup table for in- and out-scattering coefficients</param>
		/// <param name="opticalDepthTexture">Lookup table for optical depth</param>
		public void SetLookupTextures( ITexture3d scatteringTexture, ITexture2d opticalDepthTexture )
		{
			m_ScatteringTexture = scatteringTexture;
			m_OpticalDepthTexture = opticalDepthTexture;
		}


		/// <summary>
		/// Sets up parameters for ground effects
		/// </summary>
		public void SetupAtmosphereEffectParameters( IEffect effect, bool objectRendering )
		{
			IUniCamera camera = UniCamera.Current;
			Point3 localPos = UniUnits.RenderUnits.MakeRelativePoint( m_Planet.Transform.Position, camera.Position );
			float planetRadius = ( float )UniUnits.RenderUnits.FromUniUnits( m_Planet.Radius );
			float atmosphereRadius = ( float )UniUnits.RenderUnits.FromUniUnits( m_AtmosphereRadius );
			float height = localPos.DistanceTo( Point3.Origin ) - planetRadius;
			float clampedHeight = Utils.Clamp( height, 0, atmosphereRadius );
			float normHeight = clampedHeight / atmosphereRadius;

			float clampedLength = planetRadius + clampedHeight;
			Vector3 groundVec = localPos.ToVector3( ).MakeLength( clampedLength );
			Point3 atmPos = Point3.Origin + groundVec;

			Vector3 viewDir = UniCamera.Current.Frame.ZAxis;
			effect.Parameters[ "AtmViewPosLength" ].Set( atmPos.DistanceTo( Point3.Origin ) );
			effect.Parameters[ "AtmHgCoeff" ].Set( m_Planet.Atmosphere.PhaseCoefficient );
			effect.Parameters[ "AtmPhaseWeight" ].Set( m_Planet.Atmosphere.PhaseWeight );
			effect.Parameters[ "AtmViewPos" ].Set( atmPos.X, atmPos.Y, atmPos.Z );
			effect.Parameters[ "AtmViewDir" ].Set( viewDir.X, viewDir.Y, viewDir.Z );
			effect.Parameters[ "AtmViewHeight" ].Set( normHeight );
			effect.Parameters[ "ScatteringTexture" ].Set( m_ScatteringTexture );
			effect.Parameters[ "AtmInnerRadius" ].Set( planetRadius );
			effect.Parameters[ "AtmThickness" ].Set( atmosphereRadius );

			if ( objectRendering )
			{
				effect.Parameters[ "OpticalDepthTexture" ].Set( m_OpticalDepthTexture );
			}
		}

		/// <summary>
		/// Gets the scattering coefficient lookup texture used to render this atmosphere model
		/// </summary>
		public ITexture3d ScatteringTexture
		{
			get { return m_ScatteringTexture; }
		}

		/// <summary>
		/// Gets the optical depth lookup texture used to render this atmosphere model
		/// </summary>
		public ITexture2d OpticalDepthTexture
		{
			get { return m_OpticalDepthTexture; }
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the atmosphere
		/// </summary>
		public void Render( IRenderContext context )
		{
			SetupAtmosphereEffectParameters( m_Techniques.Effect, false );
			context.ApplyTechnique( m_Techniques, m_Atmosphere );
		}

		#endregion

		#region Private Members

		private readonly SpherePlanet		m_Planet;
		private readonly long				m_AtmosphereRadius;
		private readonly IRenderable		m_Atmosphere;
		private readonly TechniqueSelector	m_Techniques;
		private ITexture3d					m_ScatteringTexture;
		private ITexture2d					m_OpticalDepthTexture;

		private void atmosphereEffect_OnReload( Rb.Assets.Interfaces.ISource obj )
		{
			//	Reselect the current technique
			m_Techniques.Select( m_Techniques.Technique.Name );
		}

		#endregion
	}
}
