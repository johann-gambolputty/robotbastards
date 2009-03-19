
namespace Poc1.Core.Interfaces.Astronomical.Planets
{
	/// <summary>
	/// Factory for <see cref="IPlanetRenderer"/>
	/// </summary>
	public interface IPlanetRendererFactory
	{
		/// <summary>
		/// Creates an IPlanetRenderer for a planet
		/// </summary>
		IPlanetRenderer Create( IPlanet planet );
	}
}
