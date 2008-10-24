
namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Model for a planet's rings
	/// </summary>
	public interface IPlanetRingModel : IPlanetEnvironmentModel
	{
		/// <summary>
		/// Gets/sets the width of the rings
		/// </summary>
		Units.Metres Width
		{
			get; set;
		}
	}
}
