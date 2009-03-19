
using System;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;
using Poc1.Core.Interfaces.Rendering;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Planetary ring renderer for spherical planets
	/// </summary>
	/// <seealso cref="ISpherePlanetRingModel"/>
	public class SpherePlanetRingRenderer : SpherePlanetEnvironmentRenderer
	{
		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IUniRenderContext context )
		{
			if ( context.CurrentPass != UniRenderPass.FarObjects )
			{
				return;
			}
			throw new NotImplementedException( );
		}
	}

}
