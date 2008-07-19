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
		/// Gets/sets the lookup texture used to render this atmosphere model
		/// </summary>
		public ITexture3d LookupTexture
		{
			get { return m_LookupTexture; }
			set { m_LookupTexture = value; }
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the atmosphere
		/// </summary>
		public void Render( IRenderContext context )
		{
			IUniCamera camera = UniCamera.Current;
			double height = UniCamera.Current.Position.DistanceTo( m_Planet.Transform.Position ) - m_Planet.Radius;
			double clampedHeight = Utils.Clamp( height, 0, m_AtmosphereRadius );
			float normHeight = ( float )( clampedHeight / m_AtmosphereRadius );

			Point3 localPos = UniUnits.RenderUnits.MakeRelativePoint( m_Planet.Transform.Position, camera.Position );
			Vector3 viewDir = UniCamera.Current.Frame.ZAxis;

			m_Techniques.Effect.Parameters[ "AtmViewPos" ].Set( localPos.X, localPos.Y, localPos.Z );
			m_Techniques.Effect.Parameters[ "AtmViewDir" ].Set( viewDir.X, viewDir.Y, viewDir.Z );
			m_Techniques.Effect.Parameters[ "AtmViewHeight" ].Set( normHeight );


			if ( m_LookupTexture != null )
			{
				m_Techniques.Effect.Parameters[ "AtmosphereLookupTexture" ].Set( m_LookupTexture );
			}
			context.ApplyTechnique( m_Techniques, m_Atmosphere );
		}

		#endregion

		#region Private Members

		private readonly SpherePlanet		m_Planet;
		private readonly long				m_AtmosphereRadius;
		private readonly IRenderable		m_Atmosphere;
		private readonly TechniqueSelector	m_Techniques;
		private ITexture3d					m_LookupTexture;

		private void atmosphereEffect_OnReload( Rb.Assets.Interfaces.ISource obj )
		{
			//	Reselect the current technique
			m_Techniques.Select( m_Techniques.Technique.Name );
		}

		#endregion
	}
}
