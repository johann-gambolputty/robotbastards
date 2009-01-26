using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Poc1.Universe.Planets.Renderers;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Sphere planet renderer
	/// </summary>
	public class SpherePlanetRenderer : PlanetRenderer, ISpherePlanetRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="planet">Associated planet</param>
		public SpherePlanetRenderer( ISpherePlanet planet ) :
			base( planet )
		{	
		}

		#region ISpherePlanetRenderer Members

		#region Renderers

		/// <summary>
		/// Gets the atmosphere renderer
		/// </summary>
		public ISpherePlanetAtmosphereRenderer SphereAtmosphereRenderer
		{
			get { return ( ISpherePlanetAtmosphereRenderer )AtmosphereRenderer; }
		}

		/// <summary>
		/// Gets the cloud renderer
		/// </summary>
		public ISpherePlanetCloudRenderer SphereCloudRenderer
		{
			get { return ( ISpherePlanetCloudRenderer )CloudRenderer; }
		}

		/// <summary>
		/// Gets the terrain renderer
		/// </summary>
		public ISpherePlanetTerrainRenderer SphereTerrainRenderer
		{
			get { return ( ISpherePlanetTerrainRenderer )TerrainRenderer; }
		}

		#endregion

		#endregion
	}
}
