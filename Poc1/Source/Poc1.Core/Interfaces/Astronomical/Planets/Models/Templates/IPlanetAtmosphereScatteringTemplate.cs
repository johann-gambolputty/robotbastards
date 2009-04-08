
namespace Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Interface for scattering atmosphere templates
	/// </summary>
	public interface IPlanetAtmosphereScatteringTemplate : IPlanetAtmosphereTemplate
	{
		/// <summary>
		/// Gets/sets the names of the atmospheres used by this template
		/// </summary>
		/// <remarks>
		/// The atmosphere name maps to the texture files used for in-game rendering, and the 
		/// data files used for editor build tasks.
		/// </remarks>
		string[] AtmosphereNames
		{
			get; set;
		}
	}
}
