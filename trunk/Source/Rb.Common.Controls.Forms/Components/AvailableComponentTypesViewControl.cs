using System.Windows.Forms;
using Rb.Common.Controls.Components;
using Rb.Core.Components;

namespace Rb.Common.Controls.Forms.Components
{

	public partial class AvailableComponentTypesViewControl : UserControl
	{
		public AvailableComponentTypesViewControl( )
		{
			InitializeComponent( );
		}

		/// <summary>
		/// Gets/sets the component type categories used to organize template types added to this view
		/// </summary>
		public ComponentTypeCategory[] Categories
		{
			get { return m_Categories; }
			set
			{
				if ( m_Categories == value )
				{
					return;
				}
				if ( value == null )
				{
					m_Categories = null;
				}
				else
				{
					m_Categories = new ComponentTypeCategory[ value.Length ];
					for ( int catIndex = 0; catIndex < value.Length; ++catIndex )
					{
						m_Categories[ catIndex ] = new ComponentTypeCategory( value[ catIndex ] );
					}
				}
				RefreshCategories( );
			}
		}

		/// <summary>
		/// Gets the seleced template type. Returns null if nothing is selected
		/// </summary>
		public ComponentType SelectedType
		{
			get
			{
				if ( templateTypeCategoryControl.SelectedItem == null )
				{
					return null;
				}
				return templateTypeCategoryControl.SelectedItem.Tag as ComponentType;
			}
		}

		/// <summary>
		/// Gets/sets the template types displayed by this view
		/// </summary>
		public ComponentType[] Types
		{
			get { return m_Types; }
			set
			{
				if ( m_Types != value )
				{
					m_Types = value;
					RefreshCategories( );
				}
			}
		}

		#region Private Members

		private ComponentTypeCategory[] m_Categories;
		private ComponentType[] m_Types;

		/// <summary>
		/// Links the current category list to the control
		/// </summary>
		private void RefreshCategories( )
		{
			templateTypeCategoryControl.Clear( );
			if ( m_Categories == null )
			{
				return;
			}
			foreach ( ComponentTypeCategory category in m_Categories )
			{
				category.ClearItems( );
			}
			if ( m_Types != null )
			{
				foreach ( ComponentType templateType in m_Types )
				{
					foreach ( ComponentTypeCategory category in m_Categories )
					{
						AddComponentType( category, templateType );
					}
				}
			}
			foreach ( ComponentTypeCategory category in m_Categories )
			{
				templateTypeCategoryControl.AddCategory( category );
			}
		}

		/// <summary>
		/// Links a template types to the control
		/// </summary>
		private void AddComponentType( ComponentTypeCategory category, ComponentType type )
		{
			if ( category.BaseType.IsAssignableFrom( type.Type ) )
			{
				category.AddComponentType( type, type.Type.Name, "blah" );
			}
			foreach ( ComponentTypeCategory subCategory in category.SubCategories )
			{
				AddComponentType( subCategory, type );
			}
		}

		#region Event Handlers

		#endregion

		#endregion
	}
}
