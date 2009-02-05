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
		/// Gets the sphere planet ring model
		/// </summary>
		public ISpherePlanetRingModel SphereRingModel
		{
			get { return ( ISpherePlanetRingModel )RingModel; }
		}

		/// <summary>
		/// Gets the sphere atmosphere model
		/// </summary>
		public ISpherePlanetAtmosphereModel SphereAtmosphereModel
		{
			get { return ( ISpherePlanetAtmosphereModel )AtmosphereModel; }
		}

		/// <summary>
		/// Gets the sphere cloud model
		/// </summary>
		public ISpherePlanetCloudModel SphereCloudModel
		{
			get { return ( ISpherePlanetCloudModel )CloudModel; }
		}

		/// <summary>
		/// Gets the sphere terrain model
		/// </summary>
		public ISpherePlanetTerrainModel SphereTerrainModel
		{
			get { return ( ISpherePlanetTerrainModel )TerrainModel; }
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
					OnModelChanged( new RadiusChangedEventArgs( ) );
				}
			}
		}

		#endregion

		#region Private Members

		private Units.Metres m_Radius; 

		#endregion
	}
}
