using System;
using System.Drawing;
using Rb.Common.Controls.Categories;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Rb.Common.Controls.Components
{
	/// <summary>
	/// A category for component types
	/// </summary>
	public class ComponentTypeCategory : Category
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public ComponentTypeCategory( Type baseType, string name, string description )
			:
			this( null, baseType, name, description )
		{
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="src">Source category</param>
		/// <exception cref="ArgumentNullException">Thrown if src is null</exception>
		public ComponentTypeCategory( ComponentTypeCategory src )
			:
			this( null, src )
		{
		}

		/// <summary>
		/// Copy constructor, with an alternate parent category
		/// </summary>
		/// <param name="parentCategory">Parent category</param>
		/// <param name="src">Source category</param>
		/// <exception cref="ArgumentNullException">Thrown if src is null</exception>
		public ComponentTypeCategory( ComponentTypeCategory parentCategory, ComponentTypeCategory src )
			:
			base( parentCategory, src.Name, src.Description, src.Icon )
		{
			Arguments.CheckNotNull( src, "src" );
			m_BaseType = src.BaseType;
			foreach ( ComponentTypeCategory srcSubCategory in src.SubCategories )
			{
				new ComponentTypeCategory( this, srcSubCategory );
			}
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public ComponentTypeCategory( ComponentTypeCategory parentCategory, Type baseType, string name, string description )
			:
			base( parentCategory, name, description, s_DefaultIcon )
		{
			Arguments.CheckNotNull( baseType, "baseType" );
			m_BaseType = baseType;
		}

		/// <summary>
		/// Gets the type associated with this category
		/// </summary>
		public Type BaseType
		{
			get { return m_BaseType; }
		}

		/// <summary>
		/// Adds a component type as a category item to this category, and any type-compatible sub-categories
		/// </summary>
		public void AddComponentType( ComponentType type, string name, string description )
		{
			AddComponentType( type, name, description, s_DefaultItemIcon );
		}

		/// <summary>
		/// Adds a component type as a category item to this category, and any type-compatible sub-categories
		/// </summary>
		public void AddComponentType( ComponentType type, string name, string description, Icon icon )
		{
			Arguments.CheckNotNull( type, "type" );
			Arguments.CheckNotNullOrEmpty( name, "name" );
			Arguments.CheckNotNull( description, "description" );
			Arguments.CheckNotNull( icon, "icon" );
			if ( m_BaseType.IsAssignableFrom( type.Type ) )
			{
				CategoryItem item = new CategoryItem( name, description, icon );
				item.Tag = type;
				AddItem( item );
			}
			foreach ( Category subCategory in SubCategories )
			{
				ComponentTypeCategory componentSubCategory = subCategory as ComponentTypeCategory;
				if ( componentSubCategory != null )
				{
					componentSubCategory.AddComponentType( type, name, description, icon );
				}
			}
		}

		#region Private Members

		private readonly Type m_BaseType;
		private static readonly Icon s_DefaultIcon = SystemIcons.Asterisk;
		private static readonly Icon s_DefaultItemIcon = SystemIcons.Application;

		#endregion
	}


	/// <summary>
	/// A category for a component type
	/// </summary>
	public class ComponentTypeCategory<T> : ComponentTypeCategory
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public ComponentTypeCategory( string name, string description )
			:
			base( null, typeof( T ), name, description )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public ComponentTypeCategory( ComponentTypeCategory parentCategory, string name, string description )
			:
			base( parentCategory, typeof( T ), name, description )
		{
		}
	}

}
