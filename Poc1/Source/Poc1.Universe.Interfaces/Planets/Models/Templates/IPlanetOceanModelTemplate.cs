
using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	/// <summary>
	/// Ocean model template interface
	/// </summary>
	public interface IPlanetOceanModelTemplate : IPlanetEnvironmentModelTemplate
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
