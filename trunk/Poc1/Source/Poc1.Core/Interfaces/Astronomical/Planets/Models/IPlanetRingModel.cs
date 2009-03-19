
namespace Poc1.Core.Interfaces.Astronomical.Planets.Models
{
	/// <summary>
	/// Planet ring model interface
	/// </summary>
	public interface IPlanetRingModel : IPlanetEnvironmentModel
	{
		/// <summary>
		/// Gets/sets the width of the rings in metres
		/// </summary>
		Units.Metres Width
		{
			get; set;
		}
	}
}
