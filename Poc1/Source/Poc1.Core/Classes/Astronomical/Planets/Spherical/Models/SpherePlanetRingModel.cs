using Poc1.Core.Classes.Astronomical.Planets.Generic.Models;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Models
{

	/// <summary>
	/// Sphere planet ring model implementation
	/// </summary>
	public class SpherePlanetRingModel : GenericPlanetRingModel<ISpherePlanet, ISpherePlanetModel>, ISpherePlanetRingModel
	{
		/// <summary>
		/// Gets/sets the distance in metres from the centre of the planet to the inner edge of the rings
		/// </summary>
		public Units.Metres InnerRadius
		{
			get { return m_InnerRadius; }
			set
			{
				if ( m_InnerRadius != value )
				{
					m_InnerRadius = value;
					OnModelChanged( );
				}
			}
		}

		#region Private Members

		private Units.Metres m_InnerRadius;

		#endregion
	}

}
