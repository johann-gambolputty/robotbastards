
using Poc1.Universe.Interfaces.Planets.Renderers;

namespace Poc1.Universe.Interfaces.Planets.Spherical.Renderers
{
	/// <summary>
	/// Sphere planet renderer
	/// </summary>
	public interface ISpherePlanetRenderer : IPlanetRenderer
	{
		#region Renderers

		/// <summary>
		/// Gets the atmosphere renderer
		/// </summary>
		ISpherePlanetAtmosphereRenderer SphereAtmosphereRenderer
		{
			get;
		}

		/// <summary>
		/// Gets the cloud renderer
		/// </summary>
		ISpherePlanetCloudRenderer SphereCloudRenderer
		{
			get;
		}

		/// <summary>
		/// Gets the terrain renderer
		/// </summary>
		ISpherePlanetTerrainRenderer SphereTerrainRenderer
		{
			get;
		}

		#endregion
	}
}
