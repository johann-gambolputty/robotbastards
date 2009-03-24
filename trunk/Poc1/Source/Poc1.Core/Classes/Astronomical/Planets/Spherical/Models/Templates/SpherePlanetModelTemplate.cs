using Poc1.Core.Classes.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;
using Rb.Core.Maths;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Models.Templates
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
