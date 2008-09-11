using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Atmosphere renderer for spherical planets
	/// </summary>
	public class SpherePlanetAtmosphereRenderer : ISpherePlanetAtmosphereRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetAtmosphereRenderer( )
		{
			//	Load in atmosphere effect
			m_Effect = new EffectAssetHandle( "Effects/Planets/atmosphereShell.cgfx", true );
			m_Effect.OnReload += Effect_OnReload;
			m_Techniques = new TechniqueSelector( m_Effect, "DefaultTechnique" );
		}

		#region IRenderable Members

		/// <summary>
		/// Gets/sets the planet this renderer belongs to
		/// </summary>
		public ISpherePlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = value; }
		}

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
			if ( m_Planet == null )
			{
				return;
			}
			IUniCamera camera = UniCamera.Current;
			Point3 localPos = UniUnits.RenderUnits.MakeRelativePoint( m_Planet.Transform.Position, camera.Position );
			float planetRadius = m_Planet.Radius.RenderUnits;
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

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the atmosphere
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( m_Planet == null )
			{
				return;
			}
		}

		#endregion

		#region Private Members

		private ISpherePlanet m_Planet;
		private IRenderable m_AtmosphereGeometry;
		private TechniqueSelector m_Techniques;
		private ITexture3d m_ScatteringTexture;
		private ITexture2d m_OpticalDepthTexture;
		private EffectAssetHandle m_Effect;

		/// <summary>
		/// Gets the atmosphere geometry object
		/// </summary>
		private IRenderable AtmosphereGeometry
		{
			get
			{
				if ( m_AtmosphereGeometry == null )
				{
					float renderRadius = 0;
					Graphics.Draw.StartCache( );
					Graphics.Draw.Sphere( null, Point3.Origin, renderRadius, 40, 40 );
					m_AtmosphereGeometry = Graphics.Draw.StopCache( );
				}
				return m_AtmosphereGeometry;
			}
		}

		/// <summary>
		/// Called when the atmosphere effect is reloaded
		/// </summary>
		private void Effect_OnReload( Rb.Assets.Interfaces.ISource obj )
		{
			m_Techniques = new TechniqueSelector( m_Effect.Asset, m_Techniques.Name );
		}

		#endregion
	}
}
