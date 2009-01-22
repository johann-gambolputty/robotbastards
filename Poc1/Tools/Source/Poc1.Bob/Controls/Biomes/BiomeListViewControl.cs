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
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="listModel">Model displayed in this control</param>
		public BiomeListViewControl( BiomeListModel listModel )
		{
			InitializeComponent( );
			BiomeList = listModel;
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
					BiomeModel model = BiomeList.Models[ args.NewIndex ];
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
				return biomeListView.SelectedItems.Count > 0 ? ( BiomeModel )biomeListView.SelectedItems[ 0 ].Tag : null;
			}
			set
			{
				if ( value == null )
				{
					biomeListView.SelectedItems.Clear( );
					return;
				}
				foreach ( ListViewItem item in biomeListView.Items )
				{
					if ( item.Tag == value )
					{
						item.Selected = true;
						break;
					}
				}
				throw new ArgumentException( string.Format( "Biome \"{0}\" did not exist in the list displayed by this control", value.Name ), "value" );
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
			BiomeModel biome = SelectedBiome;
			if ( BiomeSelected != null )
			{
				BiomeSelected( biome );
			}
		}

		#endregion

	}
}
