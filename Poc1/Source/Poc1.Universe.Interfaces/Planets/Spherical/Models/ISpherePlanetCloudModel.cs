using Poc1.Universe.Interfaces.Planets.Models;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Spherical.Models
{
	/// <summary>
	/// Cloud model interface for spherical planets
	/// </summary>
	public interface ISpherePlanetCloudModel : IPlanetCloudModel
	{
		/// <summary>
		/// Gets the cloud texture
		/// </summary>
		ICubeMapTexture CloudTexture
		{
			get;
		}
	}
}
