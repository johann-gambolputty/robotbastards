
using Poc1.Tools.TerrainTextures.Core;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Interfaces.Biomes.Views
{
	/// <summary>
	/// Terrain type list view interface
	/// </summary>
	public interface ITerrainTypeListView
	{
		/// <summary>
		/// Event raised when the user requests a new terrain type be added to the list
		/// </summary>
		event ActionDelegates.Action<TerrainType> AddTerrainType;

		/// <summary>
		/// Event raised when the user requests a terrain type be removed from the list
		/// </summary>
		event ActionDelegates.Action<TerrainType> RemoveTerrainType;

		/// <summary>
		/// Gets/sets the terrain types displayed by this view
		/// </summary>
		TerrainTypeList TerrainTypes
		{
			get; set;
		}
	}
}
