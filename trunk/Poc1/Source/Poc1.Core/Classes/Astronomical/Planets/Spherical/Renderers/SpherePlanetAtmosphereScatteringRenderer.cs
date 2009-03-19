using System;
using Poc1.Core.Interfaces.Rendering;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{

	/// <summary>
	/// Sphere planet atmospheric scattering renderer
	/// </summary>
	public class SpherePlanetAtmosphereScatteringRenderer : SpherePlanetEnvironmentRenderer
	{
		/// <summary>
		/// Renders the atmosphere
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IUniRenderContext context )
		{
			throw new Exception( "The method or operation is not implemented." );
		}
	}

}
