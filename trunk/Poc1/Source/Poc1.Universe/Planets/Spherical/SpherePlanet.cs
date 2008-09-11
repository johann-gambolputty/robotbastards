using System;
using Poc1.Universe.Classes.Rendering;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;

namespace Poc1.Universe.Planets.Spherical
{
	/// <summary>
	/// Creates a sphere planet
	/// </summary>
	public class SpherePlanet : Planet, ISpherePlanet
	{
		/// <summary>
		/// Creates a default spherical planet
		/// </summary>
		public static ISpherePlanet DefaultPlanet( )
		{
			ISpherePlanet planet = new SpherePlanet( );
			planet.OceanRenderer = new SpherePlanetOceanRenderer( );

			return planet;
		}

		#region ISpherePlanet Members

		/// <summary>
		/// Called when the planet changes. This does not occur when a model changes.
		/// </summary>
		public event EventHandler PlanetChanged;

		/// <summary>
		/// Gets/sets the radius of the planet. If the radius is changed, PlanetChanged is invoked
		/// </summary>
		public Units.Metres Radius
		{
			get { return m_Radius; }
			set
			{
				Radius = value;
				if ( PlanetChanged != null )
				{
					PlanetChanged( this, null );
				}
			}
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

		/// <summary>
		/// Gets the sphere atmosphere renderer
		/// </summary>
		public ISpherePlanetAtmosphereRenderer SphereAtmosphereRenderer
		{
			get { return ( ISpherePlanetAtmosphereRenderer )AtmosphereRenderer; }
		}

		/// <summary>
		/// Gets the sphere cloud renderer
		/// </summary>
		public ISpherePlanetCloudRenderer SphereCloudRenderer
		{
			get { return ( ISpherePlanetCloudRenderer )CloudRenderer; }
		}

		/// <summary>
		/// Gets the sphere terrain renderer
		/// </summary>
		public ISpherePlanetTerrainRenderer SphereTerrainRenderer
		{
			get { return ( ISpherePlanetTerrainRenderer )TerrainRenderer; }
		}

		#endregion


		#region Private Members

		private Units.Metres m_Radius = new Units.Metres( 100000 );

		#endregion

	}
}
