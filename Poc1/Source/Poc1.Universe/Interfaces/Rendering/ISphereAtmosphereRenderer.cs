using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Spherical planet atmosphere renderer
	/// </summary>
	public interface ISphereAtmosphereRenderer : IAtmosphereRenderer
	{
		/// <summary>
		/// Gets/sets the lookup texture used to render this atmosphere model
		/// </summary>
		ITexture3d LookupTexture
		{
			get; set;
		}

	}
}
