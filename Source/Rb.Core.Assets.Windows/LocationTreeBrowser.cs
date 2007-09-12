using System;
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

			((Bitmap)upButton.Image).MakeTransparent(Color.Magenta);
			((Bitmap)backButton.Image).MakeTransparent(Color.Magenta);
			currentFolderView.SmallImageList = tree.Images;
		}

		#region ILocationBrowser Members

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
			}
			foreach ( LocationTreeItem item in items )
			{
				ListViewItem listItem = currentFolderView.Items.Add( item.Name, item.Image );
				listItem.Tag = item;
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
				header.Tag = property;
			}

			ShowCurrentFolder( m_Locations.DefaultFolder );
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
				if ( SelectionChosen != null )
				{
					SelectionChosen( this, null );
				}
			}
		}
	}
}
