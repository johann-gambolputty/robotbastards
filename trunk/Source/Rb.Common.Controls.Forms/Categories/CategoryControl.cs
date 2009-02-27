using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Rb.Common.Controls.Categories;
using Rb.Core.Utils;

namespace Rb.Common.Controls.Forms.Categories
{
	public partial class CategoryControl : UserControl
	{
		public CategoryControl( )
		{
			InitializeComponent( );

			categoryView.ImageList = m_Images;
		}

		/// <summary>
		/// Gets the currently selected category item
		/// </summary>
		public CategoryItem SelectedItem
		{
			get
			{
				if ( categoryView.SelectedNode == null )
				{
					return null;
				}
				return categoryView.SelectedNode.Tag as CategoryItem;
			}
		}

		/// <summary>
		/// Gets/sets the default icon used when adding categories without specific icons
		/// </summary>
		public Icon DefaultCategoryIcon
		{
			get { return m_DefaultCategoryIcon; }
			set
			{
				Arguments.CheckNotNull( value, "value" );
				m_DefaultCategoryIcon = value;
			}
		}

		/// <summary>
		/// Adds a category to the control
		/// </summary>
		/// <param name="category">Category to add</param>
		/// <returns>Returns category</returns>
		public Category AddCategory( Category category )
		{
			Arguments.CheckNotNull( category, "category" );
			if ( category.ParentCategory != null )
			{
				throw new ArgumentException( "Cannot add non-root category" );
			}
			categoryView.Nodes.Add( CreateNodeForCategory( category ) );
			m_RootCategories.Add( category );
			return category;
		}

		/// <summary>
		/// Clears the control
		/// </summary>
		public void Clear( )
		{
			m_NodeMap.Clear( );
			m_Images.Images.Clear( );
			m_RootCategories.Clear( );
			categoryView.Nodes.Clear( );
		}

		#region Private Members

		private Icon m_DefaultCategoryIcon = SystemIcons.Exclamation;
		private ImageList m_Images = new ImageList( );
		private readonly List<Category> m_RootCategories = new List<Category>( );
		private readonly Dictionary<CategoryItem, TreeNode> m_NodeMap = new Dictionary<CategoryItem, TreeNode>( );


		/// <summary>
		/// Adds an image to the image list
		/// </summary>
		private string AddImage( string key, Icon image )
		{
			if ( !m_Images.Images.ContainsKey( key ) )
			{
				m_Images.Images.Add( key, image );
			}
			return key;
		}

		/// <summary>
		/// Creates a tree node for a category item
		/// </summary>
		private TreeNode CreateNodeForCategoryItem( CategoryItem category )
		{
			Icon icon = category.Icon ?? DefaultCategoryIcon;
			TreeNode node = new TreeNode( category.Name );
			node.Tag = category;
			node.SelectedImageKey = node.ImageKey = AddImage( icon.Handle.ToString( ), icon );

			m_NodeMap[ category ] = node;

			return node;
		}

		/// <summary>
		/// Creates a tree node for a category
		/// </summary>
		private TreeNode CreateNodeForCategory( Category category )
		{
			TreeNode node = CreateNodeForCategoryItem( category );
			foreach ( Category subCategory in category.SubCategories )
			{
				node.Nodes.Add( CreateNodeForCategory( subCategory ) );
			}
			foreach ( CategoryItem item in category.Items )
			{
				node.Nodes.Add( CreateNodeForCategoryItem( item ) );
			}
			node.ExpandAll( );

			return node;
		}

		/// <summary>
		/// Filters items from a category
		/// </summary>
		private TreeNode FilterItem( CategoryItem item, Regex filter )
		{
			if ( filter.IsMatch( item.Name ) )
			{
				TreeNode addNode = m_NodeMap[ item ];
				if ( addNode == null )
				{
					addNode = CreateNodeForCategoryItem( item );
				}
				return addNode;
			}
			TreeNode node = m_NodeMap[ item ];
			if ( node != null )
			{
				node.Remove( );
				m_NodeMap[ item ] = null;
			}
			return null;
		}

		/// <summary>
		/// Filters a category
		/// </summary>
		private TreeNode FilterCategory( Category category, Regex filter )
		{
			TreeNode curNode = null;

			//	Run through all the subcategories of the current category, and recursively filter them
			foreach ( Category subCategory in category.SubCategories )
			{
				TreeNode subNode = FilterCategory( subCategory, filter );
				if ( subNode != null )
				{
					//	subNode is not null, which means that the sub-category contained unfiltered items
					//	Get or create the current category's node, and add the sub-category node to it
					if ( curNode == null )
					{
						curNode = m_NodeMap[ category ] ?? CreateNodeForCategoryItem( category );
					}
					if ( subNode.Parent != curNode )
					{
						curNode.Nodes.Add( subNode );
					}
				}
			}

			//	Run through all the items in the current category, and filter them
			foreach ( CategoryItem item in category.Items )
			{
				TreeNode itemNode = FilterItem( item, filter );
				if ( itemNode != null )
				{
					if ( curNode == null )
					{
						curNode = m_NodeMap[ category ] ?? CreateNodeForCategoryItem( category );
					}
					if ( itemNode.Parent != curNode )
					{
						curNode.Nodes.Add( itemNode );
					}
				}
			}

			if ( curNode == null )
			{
				//	Current node was never created, meaning that all items and sub-categories it
				//	contained were filtered. Make sure it is removed
				curNode = m_NodeMap[ category ];
				if ( curNode != null )
				{
					curNode.Remove( );
					m_NodeMap[ category ] = null;
				}
				return null;
			}
			curNode.Expand( );
			return curNode;
		}

		#region Event Handlers

		private void filterTextBox_TextChanged( object sender, EventArgs e )
		{
			Regex regex = StringUtils.CreateWildcardRegex( "*" + filterTextBox.Text + "*" );
			foreach ( Category category in m_RootCategories )
			{
				TreeNode node = FilterCategory( category, regex );
				if ( node != null && !categoryView.Nodes.Contains( node ) )
				{
					categoryView.Nodes.Add( node );
					node.Expand( );
				}
			}
		}
		
		#endregion

		#endregion

	}
}
