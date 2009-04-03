using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;

namespace Poc1.Core.Classes.Astronomical.Planets.Models
{
	/// <summary>
	/// Base class for planetary atmospheres
	/// </summary>
	public class PlanetAtmosphereModel : AbstractPlanetEnvironmentModel, IPlanetAtmosphereModel
	{
		/// <summary>
		/// Gets/sets the thickness of the atmosphere
		/// </summary>
		public Units.Metres Thickness
		{
			get { return m_Thickness; }
			set
			{
				if ( m_Thickness != value )
				{
					m_Thickness = value;
					OnModelChanged( );
				}
			}
		}

		#region Private Members

		private Units.Metres m_Thickness = new Units.Metres( 10000 );

		#endregion
	}
}
