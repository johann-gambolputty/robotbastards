using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Rb.NiceControls;

namespace Rb.Core.Assets.Windows
{
	public partial class LocationTreeBrowser : UserControl, ILocationBrowser
	{
		/// <summary>
		/// Browser setup
		/// </summary>
		/// <param name="tree">Location tree provider</param>
		public LocationTreeBrowser( ILocationTree tree )
		{
			InitializeComponent( );
			m_Locations = tree;

			( ( Bitmap )upButton.Image ).MakeTransparent( Color.Magenta );
			( ( Bitmap )backButton.Image ).MakeTransparent( Color.Magenta );
			currentFolderView.SmallImageList = tree.Images;
		}

		#region ILocationBrowser Members

		/// <summary>
		/// Filter string
		/// </summary>
		/// <remarks>
		/// Equivalent to OpenFileDialog filter string - "Description|Wildcard|Description|Wildcard...", e.g.
		/// "Text Files|*.txt|All Files|*.*"
		/// </remarks>
		public string Filter
		{
			set
			{
				string[] filterParts = value.Split( new char[] { '|' } );
				
				typeComboBox.Items.Clear( );

				if ( filterParts.Length == 1 )
				{
					typeComboBox.Items.Add( new LocationFilter( "All Files (*.*)", "*.*" ) );
				}
				else
				{
					for ( int partIndex = 0; partIndex < filterParts.Length; partIndex += 2 )
					{
						string description = filterParts[ partIndex ];
						string wildcard = filterParts[ partIndex + 1 ];

						typeComboBox.Items.Add( new LocationFilter( description, wildcard ) );
					}
				}
				typeComboBox.SelectedIndex = 0;
			}
		}

		/// <summary>
		/// Gets the sources selected in the browser
		/// </summary>
		public ISource[] Sources
		{
			get
			{
				ISource[] sources = new ISource[ currentFolderView.SelectedItems.Count ];
				for ( int sourceIndex = 0; sourceIndex < currentFolderView.SelectedItems.Count; ++sourceIndex )
				{
					sources[ sourceIndex ] = ( ( LocationTreeNode )currentFolderView.SelectedItems[ sourceIndex ].Tag ).Source;
				}
				return sources;
			}
		}

		/// <summary>
		/// Event, invoked when the user has made his selection of assets (e.g. by double clicking on a location, or
		///  pressing return)
		/// </summary>
		public event EventHandler SelectionChosen;

		#endregion
		
		private readonly List< LocationTreeFolder > m_FolderStack = new List< LocationTreeFolder >( );
		private readonly ILocationTree m_Locations;
		private LocationTreeFolder m_CurrentFolder;
		private LocationFilter m_Filter;

		private void AddPathToRoot( ComboBox combo, LocationTreeFolder folder, int distance )
		{
			int addAt = combo.Items.Count;

			combo.Items.Insert( addAt, NewFolderComboItem( distance--, folder ) );

			combo.SelectedIndexChanged -= foldersComboBox_SelectedIndexChanged;
			combo.SelectedIndex = addAt;
			combo.SelectedIndexChanged += foldersComboBox_SelectedIndexChanged;

			if ( folder.Parent != null )
			{
				for ( folder = folder.Parent; folder != null; folder = folder.Parent )
				{
					combo.Items.Insert( addAt, NewFolderComboItem( distance--, folder ) );
				}
			}
		}

		private static void AddListItemProperties( ListViewItem item, LocationTreeNode node, LocationProperty[] properties )
		{
			for ( int propertyIndex = 1; propertyIndex < properties.Length; ++propertyIndex )
			{
				LocationProperty property = properties[ propertyIndex ];

				if ( node.HasProperty( property ) )
				{
					item.SubItems.Add( new ListViewItem.ListViewSubItem( item, node[ property ].ToString( ) ) );
				}
				else
				{
					item.SubItems.Add( new ListViewItem.ListViewSubItem( item, "" ) );
				}
			}
		}

		private void ShowCurrentFolder( LocationTreeFolder currentFolder )
		{
			IEnumerable< LocationTreeFolder > folders;
			IEnumerable< LocationTreeItem > items;
			try
			{
				folders = currentFolder.Folders;
				items = currentFolder.Items;
			}
			catch ( Exception ex )
			{
				MessageBox.Show( ex.Message );
				return;
			}

			currentFolderView.Items.Clear( );
			foreach ( LocationTreeFolder folder in folders )
			{
				ListViewItem listItem = currentFolderView.Items.Add( folder.Name, folder.Image );
				listItem.Tag = folder;
				
				AddListItemProperties( listItem, folder, m_Locations.Properties );
			}
			foreach ( LocationTreeItem item in items )
			{
				if ( ( m_Filter == null ) || ( m_Filter.Match( item ) ) )
				{
					ListViewItem listItem = currentFolderView.Items.Add( item.Name, item.Image );
					listItem.Tag = item;

					AddListItemProperties( listItem, item, m_Locations.Properties );
				}
			}

			foldersComboBox.Items.Clear( );
			foreach ( LocationTreeFolder rootFolder in m_Locations.Roots )
			{
				int distance = rootFolder.GetDistanceTo( currentFolder );
				if ( distance >= 0 )
				{
					AddPathToRoot( foldersComboBox, currentFolder, distance );
				}
				else
				{
					foldersComboBox.Items.Add( NewFolderComboItem( 0, rootFolder ) );	
				}
			}
			m_CurrentFolder = currentFolder;

			upButton.Enabled = ( m_CurrentFolder.Parent != null );
		}

		private object NewFolderComboItem( int depth, LocationTreeNode folder )
		{
			Image image = m_Locations.Images.Images[ folder.Image ];
			Image selImage = m_Locations.Images.Images[ folder.SelectedImage ];

			NiceComboBox.Item newItem = new NiceComboBox.Item( depth, folder.Name, FontStyle.Regular, image, selImage, folder );

			return newItem;
		}

		public void EnterFolder( LocationTreeFolder folder )
		{
			if ( m_CurrentFolder != folder )
			{
				m_FolderStack.Add( m_CurrentFolder );
				ShowCurrentFolder( folder );

				backButton.Enabled = true;
			}
		}

		private void LocationTreeBrowser_Load( object sender, EventArgs e )
		{
			foreach ( LocationProperty property in m_Locations.Properties )
			{
				ColumnHeader header = currentFolderView.Columns.Add( property.Name, property.DefaultSize, HorizontalAlignment.Left );
				header.Tag = new ItemComparer( property );
			}

			m_CurrentFolder = m_Locations.DefaultFolder;
			Filter = ""; // This forces a refresh of the current view
		}

		private void backButton_Click( object sender, EventArgs e )
		{
			LocationTreeFolder folder = m_FolderStack[ m_FolderStack.Count - 1 ];
			m_FolderStack.Remove( folder );
			ShowCurrentFolder( folder );

			backButton.Enabled = m_FolderStack.Count > 0;
		}

		private void upButton_Click( object sender, EventArgs e )
		{
			EnterFolder( m_CurrentFolder.Parent );
		}

		private void foldersComboBox_SelectedIndexChanged( object sender, EventArgs e )
		{
			LocationTreeFolder folder = ( LocationTreeFolder )( ( NiceComboBox.Item )foldersComboBox.SelectedItem ).Tag;
			EnterFolder( folder );
		}

		private void currentFolderView_DoubleClick( object sender, EventArgs e )
		{
			LocationTreeNode node = ( LocationTreeNode )currentFolderView.SelectedItems[ 0 ].Tag;
			if ( node is LocationTreeFolder )
			{
				EnterFolder( ( LocationTreeFolder )node );
			}
			else
			{
				OnSelectionChosen( );
			}
		}

		private void OnSelectionChosen( )
		{
			if ( SelectionChosen != null )
			{
				SelectionChosen( this, null );
			}
		}
		
		private class LocationFilter
		{
			public LocationFilter( string description, string wildcard )
			{
				m_Description = description;
				m_Extension = wildcard.Substring( 2 );
				m_AllFiles = m_Extension[ 0 ] == '*';
			}

			public override string ToString( )
			{
				return m_Description;
			}

			public bool Match( LocationTreeNode item )
			{
				if ( m_AllFiles )
				{
					return true;
				}

				return item.Name.EndsWith( m_Extension, StringComparison.CurrentCultureIgnoreCase );
			}

			private readonly string m_Extension;
			private readonly string m_Description;
			private readonly bool m_AllFiles;
		}


		private class ItemComparer : IComparer
		{
			public ItemComparer( LocationProperty property )
			{
				m_Property = property;
			}

			public void ToggleSortOrder( )
			{
				m_SortUp = !m_SortUp;
			}

			private readonly LocationProperty m_Property;
			private bool m_SortUp;

			#region IComparer Members

			public int Compare( object x, object y )
			{
				LocationTreeNode nodeX = ( LocationTreeNode )( ( ListViewItem )x ).Tag;
				LocationTreeNode nodeY = ( LocationTreeNode )( ( ListViewItem )y ).Tag;

				int result = m_Property.Compare( nodeX, nodeY );
				return m_SortUp ? result : -result;
			}

			#endregion
		}

		private void currentFolderView_ColumnClick( object sender, ColumnClickEventArgs e )
		{
			ItemComparer comparer = ( ItemComparer )currentFolderView.Columns[ e.Column ].Tag;

			currentFolderView.ListViewItemSorter = comparer;
			currentFolderView.Sort( );
			currentFolderView.ListViewItemSorter = null;

			comparer.ToggleSortOrder( );
		}

		private void selectedAssetTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if ( e.KeyCode != Keys.Return )
			{
				return;
			}
			
			string selPath = selectedAssetTextBox.Text;
			if ( !string.IsNullOrEmpty( selPath ) )
			{
				string path = m_Locations.GetFullPath( m_CurrentFolder.Path, selPath );
				if ( m_Locations.IsFolder( path ) )
				{
					EnterFolder( m_Locations.OpenFolder( path ) );
				}
				else if ( m_Locations.IsItem( path ) )
				{
					OnSelectionChosen( );
				}

				return;
			}

			if ( currentFolderView.SelectedItems.Count > 0 )
			{
				OnSelectionChosen( );
			}

		}

		private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_Filter = ( LocationFilter )typeComboBox.SelectedItem;
			ShowCurrentFolder( m_CurrentFolder );
		}
	}
}
