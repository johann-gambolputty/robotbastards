using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical
{
	/// <summary>
	/// Spherical planet model
	/// </summary>
	public class SpherePlanetModel : AbstractPlanetModel, ISpherePlanetModel
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="spherePlanet">Spherical planet</param>
		public SpherePlanetModel( ISpherePlanet spherePlanet ) :
			base( spherePlanet )
		{
		}

		#region ISpherePlanetModel Members

		/// <summary>
		/// Gets the sphere planet that this model is attached to
		/// </summary>
		public new ISpherePlanet Planet
		{
			get { return ( ISpherePlanet )base.Planet; }
		}

		/// <summary>
		/// Gets the radius, in metres, of the planet
		/// </summary>
		public Units.Metres Radius
		{
			get { return m_Radius; }
			set
			{
				if ( m_Radius == value )
				{
					return;
				}
				RadiusChangedEventArgs eventArgs = new RadiusChangedEventArgs( m_Radius, value );
				m_Radius = value;
				OnModelChanged( eventArgs );
			}
		}

		#endregion

		#region Private Members

		private Units.Metres m_Radius;

		#endregion
	}
}
