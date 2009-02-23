namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Models a planet's oceans
	/// </summary>
	public interface IPlanetOceanModel :  IPlanetEnvironmentModel
	{
		/// <summary>
		/// Gets/sets the sea level. If changed, the ModelChanged event is invoked.
		/// </summary>
		Units.Metres SeaLevel
		{
			get; set;
		}
	}
}
