
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Base interface for spherical planet cloud rendering
	/// </summary>
	public interface ISpherePlanetCloudRenderer
	{
		/// <summary>
		/// Sets up the parameters for an effect that renders clouds
		/// </summary>
		void SetupCloudEffect( IEffect effect );
	}
}
