using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using IPlanet = Poc1.Universe.Interfaces.Planets.IPlanet;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Planet ocean model implementation
	/// </summary>
	public class PlanetOceanModel : IPlanetOceanModel
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
				m_SeaLevel = value;
				if ( ModelChanged != null )
				{
					ModelChanged( this, null );
				}
			}
		}

		#endregion

		#region IPlanetEnvironmentModel Members

		/// <summary>
		/// Event, invoked when the model changes
		/// </summary>
		public event System.EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the associated planet
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = value; }
		}

		#endregion

		#region Private Members

		private IPlanet m_Planet;
		private Units.Metres m_SeaLevel = new Units.Metres( 0 );

		#endregion
	}
}
