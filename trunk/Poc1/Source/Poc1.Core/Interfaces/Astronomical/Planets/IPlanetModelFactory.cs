
namespace Poc1.Core.Interfaces.Astronomical.Planets
{
	/// <summary>
	/// Factory for <see cref="IPlanetModel"/>
	/// </summary>
	public interface IPlanetModelFactory
	{
		/// <summary>
		/// Creates an IPlanetModel for a planet
		/// </summary>
		IPlanetModel Create( IPlanet planet );
	}

}
