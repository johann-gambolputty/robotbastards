using Poc1.Universe.Interfaces;
using Rb.Core.Maths;

namespace Poc1.Universe.Planets.Models.Templates
{
	/// <summary>
	/// Base template for planetary rings
	/// </summary>
	public abstract class PlanetRingModelTemplate : PlanetEnvironmentModelTemplate
	{
		/// <summary>
		/// Gets/sets the range of width values that the planetary rings can take
		/// </summary>
		public Range<Units.Metres> RingWidth
		{
			get { return m_RingWidth; }
			set { m_RingWidth = value; }
		}

		#region Private Members

		private Range<Units.Metres> m_RingWidth;

		#endregion
	}
}
