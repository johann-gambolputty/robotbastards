
namespace Poc1.Core.Interfaces.Astronomical.Planets.Models
{
	/// <summary>
	/// Planet atmosphere model interface
	/// </summary>
	public interface IPlanetAtmosphereModel : IPlanetEnvironmentModel
	{
		/// <summary>
		/// Gets/sets the thickness of the atmosphere
		/// </summary>
		Units.Metres Thickness
		{
			get; set;
		}

	}
}
