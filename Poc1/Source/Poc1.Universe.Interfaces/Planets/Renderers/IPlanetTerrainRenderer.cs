
namespace Poc1.Universe.Interfaces.Planets.Renderers
{
	/// <summary>
	/// Planet terrain renderer
	/// </summary>
	public interface IPlanetTerrainRenderer : IPlanetEnvironmentRenderer
	{
		/// <summary>
		/// Refreshes the terrain renderer
		/// </summary>
		void Refresh( );
	}
}
