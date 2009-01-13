using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Tools.TerrainTextures.Core;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Biomes.Controllers
{
	/// <summary>
	/// Terrain texture controller
	/// </summary>
	public class BiomeTerrainTextureController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="context">Selected biome context</param>
		/// <param name="view">Biome terrain texture view</param>
		/// <exception cref="System.ArgumentNullException">Thrown if context or view is null</exception>
		public BiomeTerrainTextureController( SelectedBiomeContext context, IBiomeTerrainTextureView view )
		{
			Arguments.CheckNotNull( context, "context" );
			Arguments.CheckNotNull( view, "view" );

			context.BiomeSelected += OnBiomeSelected;

			//	Create a new child controller for controlling the terrain type list view
			TerrainTypeList terrainTypes = null;
			if ( context.SelectedBiome != null )
			{
				terrainTypes = context.SelectedBiome.TerrainTypes;
			}
			m_TerrainTypeListController = new TerrainTypeListController( terrainTypes, view.TerrainTypesView );
		}

		#region Private Members

		private readonly TerrainTypeListController m_TerrainTypeListController;

		/// <summary>
		/// Called when a biome is selected
		/// </summary>
		private void OnBiomeSelected( BiomeModel model )
		{
			m_TerrainTypeListController.Model = model == null ? null : model.TerrainTypes;
		}
		
		#endregion
	}
}
