using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Spherical.Models.Templates;
using Poc1.Universe.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Universe.Planets.Spherical.Models.Templates
{
	/// <summary>
	/// Template for spherical planets
	/// </summary>
	public class SpherePlanetModelTemplate : PlanetModelTemplate, ISpherePlanetModelTemplate
	{
		/// <summary>
		/// Get/sets the range of valid radii for a spherical planet instance
		/// </summary>
		public Range<Units.Metres> Radius
		{
			get { return m_Radius; }
			set { m_Radius = value; }
		}

		#region Private Members

		private Range<Units.Metres> m_Radius = new Range<Units.Metres>( new Units.Metres( 5000 ), new Units.Metres( 10000000 ) );

		#endregion
	}
}
