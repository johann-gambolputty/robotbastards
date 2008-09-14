
namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Base interface for planetary environments (atmosphere, clouds, ocean, terrain)
	/// </summary>
	public interface IPlanetEnvironmentModel
	{
		/// <summary>
		/// Event, invoked when the model changes
		/// </summary>
		event System.EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the planet associated with this model
		/// </summary>
		IPlanet Planet
		{
			get; set;
		}
	}
}
