using Poc1.Tools.TerrainTextures.Core;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Interfaces.Biomes.Views
{
	/// <summary>
	/// View of a terrain type
	/// </summary>
	public interface ITerrainTypeView
	{
		/// <summary>
		/// Event, raised when the user requests that the terrain type be removed
		/// </summary>
		event ActionDelegates.Action<ITerrainTypeView> RemoveTerrainType;

		/// <summary>
		/// Gets/sets the terrain type displayed by this view
		/// </summary>
		TerrainType TerrainType
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the enabled flag of the view
		/// </summary>
		bool Enabled
		{
			get; set;
		}
	}
}
