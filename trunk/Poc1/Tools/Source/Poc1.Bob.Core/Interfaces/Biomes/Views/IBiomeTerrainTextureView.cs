
namespace Poc1.Bob.Core.Interfaces.Biomes.Views
{
	/// <summary>
	/// View interface for terrain texturing a biome
	/// </summary>
	public interface IBiomeTerrainTextureView
	{
		/// <summary>
		/// Gets the terrain types view
		/// </summary>
		ITerrainTypeListView TerrainTypesView
		{
			get;
		}

		/// <summary>
		/// Gets the terrain type distributions over altitude
		/// </summary>
		ITerrainTypeDistributionView AltitudeDistributionView
		{
			get;
		}

		/// <summary>
		/// Gets the terrain type distributions over slope
		/// </summary>
		ITerrainTypeDistributionView SlopeDistributionView
		{
			get;
		}
	}
}
