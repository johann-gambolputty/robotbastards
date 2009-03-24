using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Rb.Core.Maths;

namespace Poc1.Core.Classes.Astronomical.Planets.Models
{
	/// <summary>
	/// Planet cloud model
	/// </summary>
	public class PlanetSimpleCloudModel : AbstractPlanetEnvironmentModel, IPlanetSimpleCloudModel
	{
		/// <summary>
		/// Gets/sets the height of the cloud layer  over sea level in metres
		/// </summary>
		public Units.Metres CloudLayerHeight
		{
			get { return m_CloudLayerHeight; }
			set
			{
				if ( m_CloudLayerHeight != value )
				{
					m_CloudLayerHeight = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the cloud coverage range. 0 is clear skies, 1 is complete cover
		/// </summary>
		public Range<float> CloudCoverRange
		{
			get { return m_CloudCoverRange; }
			set
			{
				if ( m_CloudCoverRange != value )
				{
					m_CloudCoverRange = value;
					OnModelChanged( );
				}
			}
		}

		#region Private Members

		private Range<float> m_CloudCoverRange;
		private Units.Metres m_CloudLayerHeight = new Units.Metres( 7000 );

		#endregion
	}

}
