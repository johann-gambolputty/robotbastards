using Poc1.Universe.Classes.Rendering;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Poc1.Universe.Planets.Models;
using Poc1.Universe.Planets.Spherical.Models;
using Poc1.Universe.Planets.Spherical.Renderers;
using Rb.Core.Threading;

namespace Poc1.Universe.Planets.Spherical
{
	/// <summary>
	/// Creates a sphere planet
	/// </summary>
	public class SpherePlanet : Planet, ISpherePlanet
	{
		#region Test planet creation

		/// <summary>
		/// Creates a default spherical planet
		/// </summary>
		public static ISpherePlanet DefaultPlanet( IWorkItemQueue workQueue, Units.Metres planetRadius )
		{
			workQueue = workQueue ?? ExtendedThreadPool.Instance;

			ISpherePlanet planet = new SpherePlanet( );
			planet.Radius = planetRadius;

			//	Models
			planet.OceanModel			= new PlanetOceanModel( );
			planet.TerrainModel			= new SpherePlanetProcTerrainModel( );
			planet.AtmosphereModel		= new SpherePlanetAtmosphereModel( );
			planet.RingModel			= new SpherePlanetRingModel( planetRadius * 1.75, new Units.Metres( 50000 ) );
			planet.CloudModel			= new SpherePlanetCloudModel( workQueue );

			//	Renderers
			planet.MarbleRenderer		= new SpherePlanetMarbleRenderer( new SpherePlanetMarbleTextureBuilder( ) );
			planet.OceanRenderer		= new SpherePlanetOceanRenderer( );
			planet.TerrainRenderer		= new SpherePlanetTerrainPatchRenderer( );
			planet.AtmosphereRenderer	= new SpherePlanetAtmosphereRenderer( );
			planet.RingRenderer			= new SpherePlanetRingRenderer( );
			planet.CloudRenderer		= new SpherePlanetCloudRenderer( );

			return planet;
		}

		/// <summary>
		/// Creates a default spherical gas giant
		/// </summary>
		public static ISpherePlanet DefaultGasGiant( Units.Metres planetRadius )
		{
			ISpherePlanet planet = new SpherePlanet( );
			planet.Radius = planetRadius;

			//	Renderers
			planet.MarbleRenderer = new SphereGasGiantMarbleRenderer( );
			planet.RingModel = new SpherePlanetRingModel( planetRadius * 1.75, planetRadius * 0.1 );

			return planet;
		}

		#endregion

		#region ISpherePlanet Members

		/// <summary>
		/// Gets/sets the radius of the planet. If the radius is changed, PlanetChanged is invoked
		/// </summary>
		public Units.Metres Radius
		{
			get { return m_Radius; }
			set
			{
				if ( m_Radius != value )
				{
					m_Radius = value;
					OnPlanetChanged( );
				}
			}
		}

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