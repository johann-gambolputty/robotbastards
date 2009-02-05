using Poc1.Universe.Classes.Rendering;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Poc1.Universe.Planets.Models;
using Poc1.Universe.Planets.Spherical.Models;
using Poc1.Universe.Planets.Spherical.Renderers;
using Rb.Core.Threading;
using Rb.Core.Utils;

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

			//	Models
			ISpherePlanetModel model = planet.PlanetModel;
			model.Radius				= planetRadius;
			model.OceanModel			= new PlanetOceanModel( );
			model.TerrainModel			= new SpherePlanetProcTerrainModel( );
			model.AtmosphereModel		= new SpherePlanetAtmosphereModel( );
			model.RingModel				= new SpherePlanetRingModel( planetRadius * 1.75, new Units.Metres( 50000 ) );
			model.CloudModel			= new SpherePlanetCloudModel( workQueue );

			//	Renderers
			ISpherePlanetRenderer renderer = planet.PlanetRenderer;
			renderer.MarbleRenderer		= new SpherePlanetMarbleRenderer( new SpherePlanetMarbleTextureBuilder( ) );
			renderer.OceanRenderer		= new SpherePlanetOceanRenderer( );
			renderer.TerrainRenderer	= new SpherePlanetTerrainPatchRenderer( );
			renderer.AtmosphereRenderer = new SpherePlanetAtmosphereRenderer( );
			renderer.RingRenderer		= new SpherePlanetRingRenderer( );
			renderer.CloudRenderer		= new SpherePlanetCloudRenderer( );

			return planet;
		}

		/// <summary>
		/// Creates a default spherical gas giant
		/// </summary>
		public static ISpherePlanet DefaultGasGiant( Units.Metres planetRadius )
		{
			ISpherePlanet planet = new SpherePlanet( );

			//	Models
			ISpherePlanetModel model = planet.PlanetModel;
			model.Radius = planetRadius;
			model.RingModel = new SpherePlanetRingModel( planetRadius * 1.75, planetRadius * 0.1 );

			//	Renderers
			ISpherePlanetRenderer renderer = planet.PlanetRenderer;
			renderer.MarbleRenderer = new SphereGasGiantMarbleRenderer( );

			return planet;
		}

		#endregion

		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanet( )
		{
			PlanetModel = new SpherePlanetModel( );
			PlanetRenderer = new SpherePlanetRenderer( );
		}

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
		/// Gets the current sphere planet model
		/// </summary>
		public new ISpherePlanetModel PlanetModel
		{
			get { return ( ISpherePlanetModel )base.PlanetModel; }
			set { base.PlanetModel = value; }
		}

		/// <summary>
		/// Gets the current sphere planet renderer
		/// </summary>
		public new ISpherePlanetRenderer PlanetRenderer
		{
			get { return ( ISpherePlanetRenderer )base.PlanetRenderer; }
			set { base.PlanetRenderer = value; }
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Validates a planet model passed to the <see cref="PlanetModel"/> setter
		/// </summary>
		protected override void ValidatePlanetModel( IPlanetModel model )
		{
			Arguments.CheckedNonNullCast<ISpherePlanetModel>( model, "model" );
		}

		/// <summary>
		/// Validates a planet renderer passed to the <see cref="PlanetRenderer"/> setter
		/// </summary>
		protected override void ValidatePlanetRenderer( IPlanetRenderer renderer )
		{
			Arguments.CheckedNonNullCast<ISpherePlanetRenderer>( renderer, "renderer" );
		}

		#endregion

		#region Private Members

		private Units.Metres m_Radius = new Units.Metres( 100000 );

		#endregion
	}
}
