using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Biomes.Controllers
{
	/// <summary>
	/// Controller part of the biome list MVC. Ties together the <see cref="BiomeListModel"/> and <see cref="IBiomeListView"/>
	/// </summary>
	public class BiomeListController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="context">Selected biome context</param>
		/// <param name="biomes">Biome list being controlled</param>
		/// <param name="view">View to watch</param>
		/// <exception cref="System.ArgumentNullException">Thrown if workspace, model or view are null</exception>
		public BiomeListController( SelectedBiomeContext context, BiomeListModel biomes, IBiomeListView view )
		{
			Arguments.CheckNotNull( context, "context");
			Arguments.CheckNotNull( biomes, "biomes" );
			Arguments.CheckNotNull( view, "view" );
			m_Context = context;
			m_Model = biomes;

			view.BiomeList = biomes;

			view.AddNewBiome += OnAddNewBiome;
			view.AddExistingBiome += OnAddExistingBiome;
			view.RemoveBiome += OnRemoveBiome;
			view.BiomeSelected += OnBiomeSelected;

			BiomeModel biome = biomes.Models.Count > 0 ? biomes.Models[ 0 ] : null;
			view.SelectedBiome = biome;
		}

		#region Private Members

		private readonly BiomeListModel m_Model;
		private readonly SelectedBiomeContext m_Context;

		/// <summary>
		/// Called when a biome is selected from the view
		/// </summary>
		private void OnBiomeSelected( BiomeModel biome )
		{
			Arguments.CheckNotNull( biome, "biome" );
			m_Context.SelectedBiome = biome;
		}

		/// <summary>
		/// Adds a new default biome to the list model
		/// </summary>
		private void OnAddNewBiome( )
		{
			BiomeModel model = new BiomeModel( "New Biome" );
			m_Model.Models.Add( model );
		}

		/// <summary>
		/// Adds an existing biome to the list model
		/// </summary>
		private void OnAddExistingBiome( BiomeModel model )
		{

		}

		/// <summary>
		/// Removes a biome from the list model
		/// </summary>
		private void OnRemoveBiome( BiomeModel model )
		{
			Arguments.CheckNotNull( model, "model" );

			//	TODO: AP: Move this functionality into the biome list model
			//	Restribute the latitude range
			m_Model.Models.Remove( model );
		}

		#endregion
	}
}
