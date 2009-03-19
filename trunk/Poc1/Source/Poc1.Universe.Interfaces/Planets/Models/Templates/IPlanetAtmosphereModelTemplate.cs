using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	/// <summary>
	/// Atmosphere model template
	/// </summary>
	public interface IPlanetAtmosphereModelTemplate : IPlanetEnvironmentModelTemplate
	{
		/// <summary>
		/// Gets/sets the atmospheric thickness range
		/// </summary>
		/// <remarks>
		/// Thickness is the height above the planet surface that the atmosphere terminates
		/// </remarks>
		Range<Units.Metres> Thickness
		{
			get; set;
		}
	}
}
