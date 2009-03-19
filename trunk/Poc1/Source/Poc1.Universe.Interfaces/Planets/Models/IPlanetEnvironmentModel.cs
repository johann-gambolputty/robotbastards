
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
		/// Gets/sets the planet model composite that contains this model
		/// </summary>
		IPlanetModel PlanetModel
		{
			get; set;
		}

		/// <summary>
		/// Visits this model
		/// </summary>
		/// <param name="visitor">Visitor to call back to</param>
		T InvokeVisit<T>( IPlanetEnvironmentModelVisitor<T> visitor );
	}
}
