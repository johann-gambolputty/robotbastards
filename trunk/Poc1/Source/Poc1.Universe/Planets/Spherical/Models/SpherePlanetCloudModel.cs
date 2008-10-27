
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Planets.Models;

namespace Poc1.Universe.Planets.Spherical.Models
{
	/// <summary>
	/// Model for sphere planet's cloud cover
	/// </summary>
	public class SpherePlanetCloudModel : PlanetCloudModel, ISpherePlanetCloudModel
	{
		/// <summary>
		/// Gets/sets the resolution of the cloud maps
		/// </summary>
		public int Resolution
		{
			get { return m_Resolution; }
			set { m_Resolution = value; }
		}

		#region Private Members

		private int m_Resolution;

		#endregion
	}
}
