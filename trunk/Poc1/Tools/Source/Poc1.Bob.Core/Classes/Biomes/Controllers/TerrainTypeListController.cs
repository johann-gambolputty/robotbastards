using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Tools.TerrainTextures.Core;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Biomes.Controllers
{
	/// <summary>
	/// Controller for a terrain type list view (<see cref="ITerrainTypeListView"/>)
	/// </summary>
	public class TerrainTypeListController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">Terrain type list view</param>
		/// <param name="model">Terrain type model. Can be null</param>
		/// <exception cref="System.ArgumentNullException">Thrown if view is null</exception>
		public TerrainTypeListController( TerrainTypeList model, ITerrainTypeListView view )
		{
			Arguments.CheckNotNull( view, "view" );

			m_View = view;
			view.AddTerrainType += OnAddTerrainType;
			view.RemoveTerrainType += OnRemoveTerrainType;

			Model = model;
		}

		/// <summary>
		/// Gets/sets the model used by the controller
		/// </summary>
		public TerrainTypeList Model
		{
			get { return m_Model; }
			set
			{
				m_Model = value;
				if ( m_Model == null )
				{
					return;
				}
				m_View.TerrainTypes = m_Model;
			}
		}

		#region Private Members

		private ITerrainTypeListView m_View;
		private TerrainTypeList m_Model;

		/// <summary>
		/// Adds a new terrain type to the list
		/// </summary>
		private void OnAddTerrainType( TerrainType terrainType )
		{
			Arguments.CheckNotNull( terrainType, "terrainType" );
			if ( Model != null )
			{
				Model.Add( terrainType );
			}
		}

		/// <summary>
		/// Removes a terrain type to the list
		/// </summary>
		private void OnRemoveTerrainType( TerrainType terrainType )
		{
			Arguments.CheckNotNull( terrainType, "terrainType" );
			if ( Model != null )
			{
				Model.Remove( terrainType );
			}
		}

		#endregion
	}
}
