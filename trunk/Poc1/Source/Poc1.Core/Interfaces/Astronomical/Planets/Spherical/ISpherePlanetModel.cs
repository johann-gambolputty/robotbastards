
namespace Poc1.Core.Interfaces.Astronomical.Planets.Spherical
{

	/// <summary>
	/// Spherical planet model
	/// </summary>
	public interface ISpherePlanetModel : IPlanetModel
	{
		/// <summary>
		/// Gets the sphere planet that this model is attached to
		/// </summary>
		new ISpherePlanet Planet
		{
			get;
		}

		/// <summary>
		/// Gets the radius, in metres, of the planet
		/// </summary>
		Units.Metres Radius
		{
			get;
		}
	}
}
