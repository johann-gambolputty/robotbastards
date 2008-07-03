using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Atmosphere renderer for spherical planets
	/// </summary>
	public class SphereAtmosphereRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SphereAtmosphereRenderer( SpherePlanet planet )
		{
			float radius = ( float )UniUnits.AstroRenderUnits.FromUniUnits( planet.Radius + planet.SphereAtmosphere.Radius );

			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, radius, 40, 40 );
			m_Atmosphere = Graphics.Draw.StopCache( );

			//	Load in atmosphere effect
			EffectAssetHandle atmosphereEffect = new EffectAssetHandle( "Effects/Planets/atmosphere.cgfx", true );
			atmosphereEffect.OnReload += atmosphereEffect_OnReload;
			m_Techniques = new TechniqueSelector( atmosphereEffect, "DefaultTechnique" );
		}

		/// <summary>
		/// Renders the atmosphere
		/// </summary>
		public void Render( IRenderContext context )
		{
			context.ApplyTechnique( m_Techniques, m_Atmosphere );
		}

		#region Private Members

		private readonly IRenderable m_Atmosphere;
		private readonly TechniqueSelector m_Techniques;

		private void atmosphereEffect_OnReload( Rb.Assets.Interfaces.ISource obj )
		{
			//	Reselect the current technique
			m_Techniques.Select( m_Techniques.Technique.Name );
		}

		#endregion
	}
}
