using System;
using System.Drawing;
using Rb.Core.Utils;

namespace Rb.Common.Controls.Forms.Categories
{
	/// <summary>
	/// An item in a category
	/// </summary>
	public class CategoryItem
	{

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Category name</param>
		/// <param name="description">Category description</param>
		/// <param name="icon">Category icon</param>
		/// <exception cref="ArgumentNullException">Thrown if name or description are null</exception>
		/// <exception cref="ArgumentException">Thrown if name is an empty string</exception>
		public CategoryItem( string name, string description, Icon icon )
		{
			Arguments.CheckNotNullOrEmpty( name, "name" );
			Arguments.CheckNotNull( description, "description" );
			Name = name;
			Description = description;
			Icon = icon;
		}

		/// <summary>
		/// Gets/sets the name of this category
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set
			{
				Arguments.CheckNotNullOrEmpty( value, "value" );
				m_Name = value;
			}
		}

		/// <summary>
		/// Gets/sets the description of this category
		/// </summary>
		public string Description
		{
			get { return m_Description; }
			set
			{
				Arguments.CheckNotNull( value, "value" );
				m_Description = value;
			}
		}

		/// <summary>
		/// Gets/sets this category's icon
		/// </summary>
		public Icon Icon
		{
			get { return m_Icon; }
			set { m_Icon = value; }
		}

		/// <summary>
		/// Gets the category which this is a sub-category of. Returns null if this is a root category
		/// </summary>
		public Category ParentCategory
		{
			get { return m_ParentCategory; }
		}

		/// <summary>
		/// Gets/sets the tag value associated with this category
		/// </summary>
		public object Tag
		{
			get { return m_Tag; }
			set { m_Tag = value; }
		}

		/// <summary>
		/// Returns the name of this category item
		/// </summary>
		public override string ToString( )
		{
			return Name;
		}

		#region Protected Members

		#region Internal Members

		/// <summary>
		/// Changes the current parent category
		/// </summary>
		internal virtual bool ChangeParentCategory( Category parentCategory )
		{
			m_ParentCategory = parentCategory;
			return true;
		}

		#endregion


		#endregion

		#region Private Members

		private object m_Tag;
		private string m_Name;
		private string m_Description;
		private Icon m_Icon;
		private Category m_ParentCategory;

		#endregion
	}
}
