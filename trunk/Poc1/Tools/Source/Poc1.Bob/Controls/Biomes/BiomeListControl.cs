using System.Windows.Forms;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Rb.Core.Utils;
using System;
using System.Collections.Generic;
using Poc1.Bob.Core.Interfaces.Biomes.Views;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class BiomeListControl : UserControl, IBiomeListView
	{
		public BiomeListControl( )
		{
			InitializeComponent( );
		}

		/// <summary>
		/// Gets the currently selected biome model
		/// </summary>
		public BiomeModel SelectedBiome
		{
			get { return biomeListBox.SelectedItem as BiomeModel; }
		}

		#region IBiomeListView Members

		/// <summary>
		/// Event raised when the user requests that a new biome be created
		/// </summary>
		public event ActionDelegates.Action OnCreateBiome;

		/// <summary>
		/// Event raised when the user requests that the currently selected biome be added to the biome list
		/// </summary>
		public event ActionDelegates.Action<BiomeModel> OnAddBiome;

		/// <summary>
		/// Event raised when the user selects a biome
		/// </summary>
		public event ActionDelegates.Action<BiomeModel> BiomeSelected;

		/// <summary>
		/// Event raised when the user requests that the currently selected biome be removed from the biome list
		/// </summary>
		public event ActionDelegates.Action<BiomeModel> OnRemoveBiome;

		/// <summary>
		/// Event raised when the user requests that the currently selected biome be deleted
		/// </summary>
		public event ActionDelegates.Action<BiomeModel> OnDeleteBiome;

		/// <summary>
		/// Adds a biome to the view
		/// </summary>
		public void AddBiome( BiomeModel model, bool selected )
		{
			Arguments.CheckNotNull( model, "model" );
			int index = biomeListBox.Items.Add( model );
			biomeListBox.SetItemChecked( index, selected );
		}

		/// <summary>
		/// Removes a biome from the view
		/// </summary>
		public void RemoveBiome( BiomeModel model )
		{
			Arguments.CheckNotNull( model, "model" );
			biomeListBox.Items.Remove( model );
		}

		/// <summary>
		/// Selects/deselects a biome
		/// </summary>
		/// <param name="model">Biome to select</param>
		/// <param name="selected">Selection/deselection flag</param>
		public void SelectBiome( BiomeModel model, bool selected )
		{
			Arguments.CheckNotNull( model, "model" );
			int index = biomeListBox.Items.IndexOf( model );
			System.Diagnostics.Debug.Assert( index != -1 );

			biomeListBox.SetItemChecked( index, selected );
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Deletes the selected biomes
		/// </summary>
		private void DeleteSelected( )
		{
			if ( biomeListBox.SelectedItems.Count == 0 )
			{
				return;
			}

			List<BiomeModel> itemsToRemove = EnumerableAdapter<BiomeModel>.ToList( biomeListBox.SelectedItems );
			foreach ( BiomeModel model in itemsToRemove )
			{
				if ( OnDeleteBiome != null )
				{
					OnDeleteBiome( model );
				}
			}
		}

		#region Event Handlers

		private void createButton_Click( object sender, EventArgs e )
		{
			if ( OnCreateBiome != null )
			{
				OnCreateBiome( );
			}
		}

		private void deleteButton_Click( object sender, EventArgs e )
		{
			DeleteSelected( );
		}

		private void biomeListView_SelectedIndexChanged( object sender, EventArgs e )
		{
			BiomeModel biome = SelectedBiome;
			if ( BiomeSelected != null )
			{
				BiomeSelected( biome );
			}
		}

		private void biomeListBox_ItemCheck( object sender, ItemCheckEventArgs e )
		{
			BiomeModel model = ( BiomeModel )biomeListBox.Items[ e.Index ];
			if ( e.NewValue == CheckState.Checked )
			{
				if ( OnAddBiome != null )
				{
					OnAddBiome( model );
				}
			}
			else
			{
				if ( OnRemoveBiome != null )
				{
					OnRemoveBiome( model );
				}
			}
		}

		private void biomeListBox_KeyUp( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Delete )
			{
				DeleteSelected( );
			}
		}

		#endregion

		#endregion

	}
}
