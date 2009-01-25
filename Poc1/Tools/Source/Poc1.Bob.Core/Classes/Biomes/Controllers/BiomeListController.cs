using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces;
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
		/// <param name="viewFactory">View factory</param>
		/// <param name="context">Selected biome context</param>
		/// <param name="completeBiomeList">List of all biomes that can be selected</param>
		/// <param name="currentBiomeList">Current biome list</param>
		/// <param name="view">View to watch</param>
		/// <exception cref="System.ArgumentNullException">Thrown if workspace, model or view are null</exception>
		public BiomeListController( IViewFactory viewFactory, SelectedBiomeContext context, BiomeListModel completeBiomeList, BiomeListModel currentBiomeList, IBiomeListView view )
		{
			Arguments.CheckNotNull( viewFactory, "viewFactory" );
			Arguments.CheckNotNull( context, "context");
			Arguments.CheckNotNull( completeBiomeList, "completeBiomeList" );
			Arguments.CheckNotNull( currentBiomeList, "currentBiomeList" );
			Arguments.CheckNotNull( view, "view" );

			m_ViewFactory = viewFactory;
			m_Context = context;
			m_AllBiomes = completeBiomeList;
			m_CurrentBiomes = currentBiomeList;

			//	Run through all the available biomes
			foreach ( BiomeModel model in m_AllBiomes.Models )
			{
				//	Add the current biome to the view. Set it as selected if it is in the current biome list
				view.AddBiome( model, m_CurrentBiomes.Models.IndexOf( model ) != -1 );
			}

			view.OnCreateBiome += OnCreateBiome;
			view.OnAddBiome += OnAddBiome;
			view.OnRemoveBiome += OnRemoveBiome;
			view.BiomeSelected += OnBiomeSelected;
			view.OnDeleteBiome += OnDeleteBiome;
			m_View = view;
		}

		#region Private Members

		private readonly IViewFactory m_ViewFactory;
		private readonly IBiomeListView m_View;
		private readonly BiomeListModel m_AllBiomes;
		private readonly BiomeListModel m_CurrentBiomes;
		private readonly SelectedBiomeContext m_Context;

		/// <summary>
		/// Called when a biome is selected from the view
		/// </summary>
		private void OnBiomeSelected( BiomeModel biome )
		{
			if ( biome != null )
			{
				m_Context.SelectedBiome = biome;
			}
		}

		/// <summary>
		/// Adds a new default biome to the list model
		/// </summary>
		private void OnCreateBiome( )
		{
			INewBiomeView newBiomeView = m_ViewFactory.CreateNewBiomeView( );
			if ( !newBiomeView.ShowView( ) )
			{
				return;
			}

			BiomeModel model = new BiomeModel( newBiomeView.BiomeName );
			m_AllBiomes.Models.Add( model );
			m_View.AddBiome( model, true );
		}

		/// <summary>
		/// Adds an existing biome to the list model
		/// </summary>
		private void OnAddBiome( BiomeModel model )
		{
			Arguments.CheckNotNull( model, "model" );
			if ( m_CurrentBiomes.Models.Contains( model ) )
			{
				return;
			}
			m_CurrentBiomes.Models.Add( model );
			m_View.SelectBiome( model, true );
		}

		/// <summary>
		/// Removes a biome from the list model
		/// </summary>
		private void OnRemoveBiome( BiomeModel model )
		{
			Arguments.CheckNotNull( model, "model" );
			if ( !m_CurrentBiomes.Models.Contains( model ) )
			{
				return;
			}
			m_CurrentBiomes.Models.Remove( model );
			m_View.SelectBiome( model, false );
		}

		/// <summary>
		/// Deletes a biome from the list model
		/// </summary>
		private void OnDeleteBiome( BiomeModel model )
		{
			Arguments.CheckNotNull( model, "model" );
			m_AllBiomes.Models.Remove( model );

			if ( m_CurrentBiomes.Models.Contains( model ) )
			{
				m_CurrentBiomes.Models.Remove( model );
				m_View.RemoveBiome( model );
			}
			if ( m_Context.SelectedBiome == model )
			{
				m_Context.SelectedBiome = null;
			}
		}

		#endregion
	}
}
