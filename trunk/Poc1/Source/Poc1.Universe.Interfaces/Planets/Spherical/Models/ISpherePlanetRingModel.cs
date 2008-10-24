using Poc1.Universe.Interfaces.Planets.Models;

namespace Poc1.Universe.Interfaces.Planets.Spherical.Models
{
	/// <summary>
	/// Ring model for spherical planets
	/// </summary>
	public interface ISpherePlanetRingModel : IPlanetRingModel
	{
		/// <summary>
		/// Gets/sets the inner radius of the rings
		/// </summary>
		Units.Metres InnerRadius
		{
			get; set;
		}
	}
}
