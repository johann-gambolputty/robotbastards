using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Rb.Core.Utils;
using System.ComponentModel;

namespace Poc1.Bob.Controls.Biomes
{
	public partial class BiomeListViewControl : UserControl, IBiomeListView
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public BiomeListViewControl( )
		{
			InitializeComponent( );

			BiomeListModel listModel = new BiomeListModel( );
			listModel.Models.Add( new BiomeModel( "arctic" ) );
			listModel.Models.Add( new BiomeModel( "temperate" ) );
			listModel.Models.Add( new BiomeModel( "desert" ) );

			ListModel = listModel;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="listModel">Model displayed in this control</param>
		public BiomeListViewControl( BiomeListModel listModel )
		{
			InitializeComponent( );
			ListModel = listModel;
		}

		/// <summary>
		/// Gets/sets the list model displayed in this control
		/// </summary>
		public BiomeListModel ListModel
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
			biomeListView.Items.Clear( );
			if ( m_ListModel == null )
			{
				return;
			}

			foreach ( BiomeModel model in m_ListModel.Models )
			{
				biomeListView.Items.Add( BiomeModelToListViewItem( model ) );
			}

			m_ListModel.Models.ListChanged += OnListChanged;
		}

		/// <summary>
		/// Creates a ListViewItem to represent a biome model
		/// </summary>
		private static ListViewItem BiomeModelToListViewItem( BiomeModel model )
		{
			ListViewItem item = new ListViewItem( model.Name );
			item.SubItems.Add( model.Name );
			item.Tag = model;
			return item;
		}

		/// <summary>
		/// Handles changes to the list model
		/// </summary>
		private void OnListChanged( object sender, ListChangedEventArgs args )
		{
			switch ( args.ListChangedType )
			{
				case ListChangedType.ItemAdded :
					BiomeModel model = ListModel.Models[ args.NewIndex ];
					biomeListView.Items.Insert( args.NewIndex, BiomeModelToListViewItem( model ) );
					break;
				case ListChangedType.ItemDeleted :
					biomeListView.Items.RemoveAt( args.NewIndex );
					break;
			}
		}

		#region IBiomeListView Members

		/// <summary>
		/// Event raised when the user requests that a new biome be added to the biome list
		/// </summary>
		public event ActionDelegates.Action AddNewBiome;

		/// <summary>
		/// Event raised when the user requests the removal of a biome
		/// </summary>
		public event ActionDelegates.Action<BiomeModel> RemoveBiome;

		/// <summary>
		/// Event raised when the user selects a biome
		/// </summary>
		public event ActionDelegates.Action<BiomeModel> BiomeSelected;

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
			if ( biomeListView.SelectedItems.Count == 0 )
			{
				return;
			}
			List<ListViewItem> itemsToRemove = EnumerableAdapter<ListViewItem>.ToList( biomeListView.SelectedItems );
			foreach ( ListViewItem item in itemsToRemove )
			{
				if ( RemoveBiome != null )
				{
					RemoveBiome( ( BiomeModel )item.Tag );
				}
				biomeListView.Items.Remove( item );
			}
		}

		private void biomeListView_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( biomeListView.SelectedItems.Count == 0 )
			{
				return;
			}
			if ( BiomeSelected != null )
			{
				BiomeModel selectedBiome = ( BiomeModel )biomeListView.SelectedItems[ 0 ].Tag;
				BiomeSelected( selectedBiome );
			}
		}

		#endregion

	}
}
