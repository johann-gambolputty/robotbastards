using Rb.Core.Maths;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Planet ring model template
	/// </summary>
	public interface IPlanetRingTemplate
	{
		/// <summary>
		/// Gets/sets the range of width values that the planetary rings can take
		/// </summary>
		Range<Units.Metres> RingWidth
		{
			get; set;
		}
	}
}