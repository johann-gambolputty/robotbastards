
using Rb.Core.Maths;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Base terrain template interface
	/// </summary>
	public interface IPlanetTerrainTemplate : IPlanetEnvironmentModelTemplate
	{
		/// <summary>
		/// Gets/sets the maximum height range for terrain
		/// </summary>
		Range<Units.Metres> MaximumHeightRange
		{
			get; set;
		}
	}
}
