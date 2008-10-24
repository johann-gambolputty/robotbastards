using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Planets.Models;

namespace Poc1.Universe.Planets.Spherical.Models
{
	/// <summary>
	/// Sphere planet ring model implementation
	/// </summary>
	public class SpherePlanetRingModel : PlanetRingModel, ISpherePlanetRingModel
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetRingModel( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public SpherePlanetRingModel( Units.Metres innerRadius, Units.Metres width )
		{
			m_InnerRadius = innerRadius;
			Width = width;
		}

		#region ISpherePlanetRingModel Members

		/// <summary>
		/// Gets/sets the inner radius of the rings
		/// </summary>
		public Units.Metres InnerRadius
		{
			get { return m_InnerRadius; }
			set { m_InnerRadius = value; }
		}

		#endregion

		#region Private Members

		private Units.Metres m_InnerRadius = new Units.Metres( 250000 );

		#endregion
	}
}
