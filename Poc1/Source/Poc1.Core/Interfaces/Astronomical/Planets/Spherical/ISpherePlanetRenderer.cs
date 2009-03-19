
namespace Poc1.Core.Interfaces.Astronomical.Planets.Spherical
{
	/// <summary>
	/// Spherical planet renderer
	/// </summary>
	public interface ISpherePlanetRenderer : IPlanetRenderer
	{
		/// <summary>
		/// Gets the spherical planet that this renderer is attached to
		/// </summary>
		new ISpherePlanet Planet
		{
			get;
		}

		/// <summary>
		/// Shortcut to Planet.PlanetModel
		/// </summary>
		new ISpherePlanetModel PlanetModel
		{
			get;
		}
	}
}
