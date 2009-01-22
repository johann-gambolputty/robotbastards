using System.Windows.Forms;
using Poc1.Bob.Core.Classes.Biomes.Models;
using System.ComponentModel;
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
		/// Unbinds this list view from the current model
		/// </summary>
		private void UnbindModel( )
		{
			if ( m_ListModel == null )
			{
				return;
			}
		}

		/// <summary>
		/// Binds this list view to the current model
		/// </summary>
		private void BindModel( )
		{
			biomeListBox.Items.Clear( );
			if ( m_ListModel == null )
			{
				return;
			}

			foreach ( BiomeModel model in m_ListModel.Models )
			{
				biomeListBox.Items.Add( model );
			}

			m_ListModel.Models.ListChanged += OnListChanged;
		}

		/// <summary>
		/// Handles changes to the list model
		/// </summary>
		private void OnListChanged( object sender, ListChangedEventArgs args )
		{
			switch ( args.ListChangedType )
			{
				case ListChangedType.ItemAdded:
					BiomeModel model = BiomeList.Models[ args.NewIndex ];
					biomeListBox.Items.Insert( args.NewIndex, model );
					break;
				case ListChangedType.ItemDeleted:
					biomeListBox.Items.RemoveAt( args.NewIndex );
					break;
			}
		}

		#region IBiomeListView Members

		/// <summary>
		/// Event raised when the user requests that a new biome be added to the biome list
		/// </summary>
		public event ActionDelegates.Action AddNewBiome;

		/// <summary>
		/// Event raised when the user requests that a existing biome be added to the biome list
		/// </summary>
		public event ActionDelegates.Action<BiomeModel> AddExistingBiome;

		/// <summary>
		/// Event raised when the user requests the removal of a biome
		/// </summary>
		public event ActionDelegates.Action<BiomeModel> RemoveBiome;

		/// <summary>
		/// Event raised when the user selects a biome
		/// </summary>
		public event ActionDelegates.Action<BiomeModel> BiomeSelected;

		/// <summary>
		/// Gets/sets the list of biomes to manage
		/// </summary>
		public BiomeListModel BiomeList
		{
			get { return m_ListModel; }
			set
			{
				if ( m_ListModel != value )
				{
					UnbindModel( );
					m_ListModel = value;
					BindModel( );
				}
			}
		}

		/// <summary>
		/// Gets the currently selected biome
		/// </summary>
		public BiomeModel SelectedBiome
		{
			get
			{
				return biomeListBox.SelectedItems.Count > 0 ? ( BiomeModel )biomeListBox.SelectedItem : null;
			}
			set
			{
				if ( value == null )
				{
					biomeListBox.SelectedItems.Clear( );
					return;
				}
				foreach ( BiomeModel model in biomeListBox.Items )
				{
					if ( model == value )
					{
						biomeListBox.SelectedItem = model;
						break;
					}
				}
				throw new ArgumentException( string.Format( "Distribution \"{0}\" did not exist in the list displayed by this control", value.Name ), "value" );
			}
		}

		#endregion

		#region Private Members

		private BiomeListModel m_ListModel;

		private void addButton_Click( object sender, EventArgs e )
		{
			if ( AddNewBiome != null )
			{
				AddNewBiome( );
			}
		}

		private void removeButton_Click( object sender, EventArgs e )
		{
			if ( biomeListBox.SelectedItems.Count == 0 )
			{
				return;
			}

			List<BiomeModel> itemsToRemove = EnumerableAdapter<BiomeModel>.ToList( biomeListBox.SelectedItems );
			foreach ( BiomeModel model in itemsToRemove )
			{
				if ( RemoveBiome != null )
				{
					RemoveBiome( model );
				}
				biomeListBox.Items.Remove( model );
			}
		}

		private void biomeListView_SelectedIndexChanged( object sender, EventArgs e )
		{
			BiomeModel biome = SelectedBiome;
			if ( BiomeSelected != null )
			{
				BiomeSelected( biome );
			}
		}

		#endregion

		private void biomeListBox_ItemCheck( object sender, ItemCheckEventArgs e )
		{
			BiomeModel model = ( BiomeModel )biomeListBox.Items[ e.Index ];
			if ( e.NewValue == CheckState.Checked )
			{
				if ( AddExistingBiome != null )
				{
					AddExistingBiome( model );
				}
			}
			else
			{
				if ( RemoveBiome != null )
				{
					RemoveBiome( model );
				}
			}
		}

	}
}
