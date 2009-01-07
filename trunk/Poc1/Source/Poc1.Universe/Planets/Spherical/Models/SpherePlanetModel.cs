using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Planets.Models;

namespace Poc1.Universe.Planets.Spherical.Models
{
	/// <summary>
	/// Sphere planet model
	/// </summary>
	public class SpherePlanetModel : PlanetModel, ISpherePlanetModel
	{
		/// <summary>
		/// Default constructor (radius of planet defaults to zero)
		/// </summary>
		public SpherePlanetModel( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public SpherePlanetModel( Units.Metres radius )
		{
			m_Radius = radius;
		}

		#region ISpherePlanetModel Members

		/// <summary>
		/// Gets/sets the radius of this model. Set raises the <see cref="PlanetModel.ModelChanged"/> event
		/// </summary>
		public Units.Metres Radius
		{
			get { return m_Radius; }
			set
			{
				if ( m_Radius != value )
				{
					m_Radius = value;
					OnModelChanged( );
				}
			}
		}

		#endregion

		#region Private Members

		private Units.Metres m_Radius; 

		#endregion
	}
}
