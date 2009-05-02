using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;

namespace Poc1.Core.Classes.Astronomical.Planets.Models
{
	/// <summary>
	/// Planet ocean model
	/// </summary>
	public class PlanetOceanModel : AbstractPlanetEnvironmentModel, IPlanetOceanModel
	{
		#region IPlanetOceanModel Members

		/// <summary>
		/// Gets/sets the sea level. If changed, the ModelChanged event is invoked.
		/// </summary>
		public Units.Metres SeaLevel
		{
			get { return m_SeaLevel; }
			set
			{
				if ( m_SeaLevel != value )
				{
					m_SeaLevel = value;
					OnModelChanged( );
				}
			}
		}

		#endregion

		#region Private Members

		private Units.Metres m_SeaLevel = new Units.Metres( 1000 );

		#endregion
	}
}
