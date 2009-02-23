
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Interface for a planet's pack based terrain texture model
	/// </summary>
	public interface IPlanetTerrainPackTextureModel : IPlanetEnvironmentModel
	{
		/// <summary>
		/// Gets/sets the terrain types texture
		/// </summary>
		ITexture2d TerrainTypesTexture
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the terrain pack texture
		/// </summary>
		ITexture2d TerrainPackTexture
		{
			get; set;
		}
	}
}
