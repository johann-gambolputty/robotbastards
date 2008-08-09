using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Spherical planet atmosphere renderer
	/// </summary>
	public interface ISphereAtmosphereRenderer : IAtmosphereRenderer
	{

		/// <summary>
		/// Sets the lookup textures required by the atmosphere renderer
		/// </summary>
		/// <param name="scatteringTexture">Lookup table for in- and out-scattering coefficients</param>
		/// <param name="opticalDepthTexture">Lookup table for optical depth</param>
		void SetLookupTextures( ITexture3d scatteringTexture, ITexture2d opticalDepthTexture );

	}
}
