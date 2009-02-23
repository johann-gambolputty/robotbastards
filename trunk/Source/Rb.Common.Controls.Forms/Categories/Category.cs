using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Rb.Common.Controls.Forms.Categories;
using Rb.Core.Utils;

namespace Rb.Common.Controls.Forms.Categories
{

	/// <summary>
	/// Item category
	/// </summary>
	public class Category : CategoryItem
	{
		/// <summary>
		/// Event raised when a sub-category is added
		/// </summary>
		public event Action<Category> SubCategoryAdded;

		/// <summary>
		/// Event raised when a sub-category is removed
		/// </summary>
		public event Action<Category> SubCategoryRemoved;

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Category name</param>
		/// <param name="description">Category description</param>
		/// <param name="icon">Category icon</param>
		/// <exception cref="ArgumentNullException">Thrown if any argument is null</exception>
		/// <exception cref="ArgumentException">Thrown if name is an empty string</exception>
		public Category( string name, string description, Icon icon )
			: base( name, description, icon )
		{
		}

		/// <summary>
		/// Setup constructor for a sub category
		/// </summary>
		/// <param name="parentCategory">Parent category</param>
		/// <param name="name">Category name</param>
		/// <param name="description">Category description</param>
		/// <param name="icon">Category icon</param>
		/// <exception cref="ArgumentNullException">Thrown if name, description or icon is null</exception>
		/// <exception cref="ArgumentException">Thrown if name is an empty string</exception>
		public Category( Category parentCategory, string name, string description, Icon icon )
			: base( name, description, icon )
		{
			if ( parentCategory != null )
			{
				parentCategory.AddSubCategory( this );
			}
		}

		/// <summary>
		/// Adds a sub-category to this category
		/// </summary>
		/// <param name="category">Category to add</param>
		public void AddSubCategory( Category category )
		{
			Arguments.CheckNotNull( category, "category" );
			category.ChangeParentCategory( this );
		}

		/// <summary>
		/// Removes a sub-category from this category
		/// </summary>
		/// <param name="category">Category to remove</param>
		public void RemoveSubCategory( Category category )
		{
			Arguments.CheckNotNull( category, "category" );
			if ( category.ParentCategory != this )
			{
				throw new ArgumentException( string.Format( "Category \"{0}\" was not a sub-category of \"{1}\", and cannot be removed", category.Name, Name ), "category" );
			}
			category.ChangeParentCategory( null );
		}

		/// <summary>
		/// Adds an item to this category
		/// </summary>
		/// <param name="item">Item to add</param>
		public void AddItem( CategoryItem item )
		{
			Arguments.CheckNotNull( item, "item" );
			m_Items.Add( item );
			item.ChangeParentCategory( this );
		}

		/// <summary>
		/// Removes an item from this category
		/// </summary>
		/// <param name="item">Item to remove</param>
		public void RemoveItem( CategoryItem item )
		{
			Arguments.CheckNotNull( item, "item" );
			m_Items.Remove( item );
			item.ChangeParentCategory( null );
		}

		/// <summary>
		/// Removes all items from the category
		/// </summary>
		public void ClearItems( )
		{
			while ( m_Items.Count > 0 )
			{
				m_Items[ 0 ].ChangeParentCategory( null );
				m_Items.RemoveAt( 0 );
			}
		}

		/// <summary>
		/// Gets the children of this category
		/// </summary>
		public ReadOnlyCollection<Category> SubCategories
		{
			get { return m_SubCategories.AsReadOnly( ); }
		}

		/// <summary>
		/// Gets the list of items in this category
		/// </summary>
		public ReadOnlyCollection<CategoryItem> Items
		{
			get { return m_Items.AsReadOnly( ); }
		}

		#region Internal Members

		/// <summary>
		/// Changes the current parent category
		/// </summary>
		internal override bool ChangeParentCategory( Category parentCategory )
		{
			if ( ParentCategory == parentCategory )
			{
				return false;
			}
			if ( ParentCategory != null )
			{
				ParentCategory.m_SubCategories.Remove( this );
				if ( ParentCategory.SubCategoryRemoved != null )
				{
					ParentCategory.SubCategoryRemoved( this );
				}
			}
			base.ChangeParentCategory( parentCategory );
			if ( ParentCategory != null )
			{
				ParentCategory.m_SubCategories.Add( this );
				if ( ParentCategory.SubCategoryAdded != null )
				{
					ParentCategory.SubCategoryAdded( this );
				}
			}
			return true;
		}

		#endregion

		#region Private Members

		private readonly List<Category> m_SubCategories = new List<Category>( );
		private readonly List<CategoryItem> m_Items = new List<CategoryItem>( );

		#endregion
	}
}
