
using Rb.Core.Maths;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Ocean model template interface
	/// </summary>
	public interface IPlanetOceanTemplate : IPlanetEnvironmentModelTemplate
	{
		/// <summary>
		/// Gets/sets the range of sea levels
		/// </summary>
		Range<Units.Metres> SeaLevel
		{
			get; set;
		}
	}
}
