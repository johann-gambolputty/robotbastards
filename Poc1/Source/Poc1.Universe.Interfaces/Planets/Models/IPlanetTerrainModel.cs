
namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Planetary terrain model
	/// </summary>
	public interface IPlanetTerrainModel : IPlanetEnvironmentModel
	{
		/// <summary>
		/// Gets/sets the maximum height of terrain generated by this model
		/// </summary>
		/// <remarks>
		/// On set, the ModelChanged event is invoked.
		/// </remarks>
		Units.Metres MaximumHeight
		{
			get; set;
		}

		/// <summary>
		/// Returns true if the model is ready to use
		/// </summary>
		/// <remarks>
		/// Can return false if the model has not been set up yet
		/// </remarks>
		bool ReadyToUse
		{
			get;
		}
	}
}
