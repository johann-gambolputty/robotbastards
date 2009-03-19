
namespace Poc1.Core.Interfaces.Astronomical.Planets.Spherical
{
	/// <summary>
	/// Spherical planet interface
	/// </summary>
	public interface ISpherePlanet : IPlanet
	{
		/// <summary>
		/// Gets the planet model
		/// </summary>
		new ISpherePlanetModel Model
		{
			get;
		}

		/// <summary>
		/// Gets the planet renderer
		/// </summary>
		new ISpherePlanetRenderer Renderer
		{
			get;
		}
	}
}
