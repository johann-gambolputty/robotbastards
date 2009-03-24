namespace Poc1.Core.Interfaces.Astronomical.Planets.Models
{
	/// <summary>
	/// Planet ocean model
	/// </summary>
	public interface IPlanetOceanModel : IPlanetEnvironmentModel
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
