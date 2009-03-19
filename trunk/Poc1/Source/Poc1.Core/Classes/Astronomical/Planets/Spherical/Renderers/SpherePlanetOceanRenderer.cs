using System;
using Poc1.Core.Interfaces.Rendering;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Ocean renderer for spherical planets
	/// </summary>
	public class SpherePlanetOceanRenderer : SpherePlanetEnvironmentRenderer
	{
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

			throw new NotImplementedException( );
		}
	}
}
