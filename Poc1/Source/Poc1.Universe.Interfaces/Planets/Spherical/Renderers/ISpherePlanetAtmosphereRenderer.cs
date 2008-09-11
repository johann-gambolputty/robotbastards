using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Spherical.Renderers
{
	/// <summary>
	/// Atmosphere rendering interface for spherical planets
	/// </summary>
	public interface ISpherePlanetAtmosphereRenderer : IPlanetAtmosphereRenderer
	{
		/// <summary>
		/// Gets/sets the planet whose atmosphere is rendered by this object
		/// </summary>
		ISpherePlanet Planet
		{
			get; set;
		}

		/// <summary>
		/// Sets the lookup textures required by the atmosphere renderer
		/// </summary>
		/// <param name="scatteringTexture">Lookup table for in- and out-scattering coefficients</param>
		/// <param name="opticalDepthTexture">Lookup table for optical depth</param>
		void SetLookupTextures( ITexture3d scatteringTexture, ITexture2d opticalDepthTexture );
	}
}
