using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	public interface IPlanetRingModelTemplate
	{
		/// <summary>
		/// Gets/sets the range of width values that the planetary rings can take
		/// </summary>
		Range<Units.Metres> RingWidth { get; set; }
	}
}