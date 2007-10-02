using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Core.Components;
using ComponentModel_TypeConverter=System.ComponentModel.TypeConverter;

namespace Poc0.LevelEditor
{
	public partial class ObjectPropertyEditor : UserControl
	{
		private readonly Timer m_Timer;

		/// <summary>
		/// Default constructor
		/// </summary>
		public ObjectPropertyEditor( )
		{
			InitializeComponent( );

			m_Timer = new Timer( );
			m_Timer.Interval = 100;
			m_Timer.Enabled = true;
			m_Timer.Tick += RefreshPropertyGridTick;

			EditModeContext.Instance.Selection.ObjectSelected += ObjectSelected;
			EditModeContext.Instance.Selection.ObjectDeselected += ObjectDeselected;
		}

		private void ObjectSelected( object obj )
		{
			if ( obj is ObjectEditState )
			{
				( ( ObjectEditState )obj ).ObjectChanged += OnObjectChanged;
			}
			BuildPropertyGrid( );
		}

		private void ObjectDeselected( object obj )
		{
			if ( obj is ObjectEditState )
			{
				( ( ObjectEditState )obj ).ObjectChanged -= OnObjectChanged;
			}
			BuildPropertyGrid( );
		}

		private void RefreshPropertyGridTick( object sender, EventArgs args )
		{
			if ( m_RefreshValues )
			{
				objectPropertyGrid.Refresh( );
				m_Timer.Stop( );
				m_RefreshValues = false;
			}
		}


		/// <summary>
		/// Builds the property grid from the current object selection
		/// </summary>
		private void BuildPropertyGrid( )
		{
			object[] selectedObjects = EditModeContext.Instance.Selection.ToArray( );

			if ( selectedObjects.Length == 0 )
			{
				objectPropertyGrid.SelectedObject = null;
				return;
			}

			PropertyBag[] bags = new ExPropertyBag[ selectedObjects.Length ];
			for ( int index = 0; index < selectedObjects.Length; ++index )
			{
				bags[ index ] = CreatePropertyBag( selectedObjects[ index ] );
			}

			objectPropertyGrid.SelectedObjects = bags;
		}

		private void OnObjectChanged( )
		{
			m_RefreshValues = true;
			m_Timer.Start( );
		}

		private bool m_RefreshValues;

		private static PropertyBag CreatePropertyBag( object obj )
		{
			if ( obj == null )
			{
				return null;
			}

			if ( obj is ObjectEditState )
			{
				return CreatePropertyBag( ( ( ObjectEditState )obj ).Instance );
			}

			if ( obj is ObjectTemplate )
			{
				return CreatePropertyBag( ( ObjectTemplate )obj );
			}

			PropertyBag bag = new ExPropertyBag( obj.GetType( ).Name );
			bag.GetValue += ExPropertySpec.GetValue;
			bag.SetValue += ExPropertySpec.SetValue;

			FillPropertyBag( bag, obj );

			return bag;
		}

		public class NullConverter : ExpandableObjectConverter
		{
			public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
			{
				return false;
			}

			public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
			{
				return false;
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		private class ExPropertyBag : PropertyBag
		{
			public ExPropertyBag( string name )
			{
				m_Name = name;
			}

			public override string ToString( )
			{
				return m_Name;
			}

			private readonly string m_Name;
		}

		private class ExPropertySpec : PropertySpec
		{
			public static void SetValue( object sender, PropertySpecEventArgs args )
			{
				ExPropertySpec exProperty = ( ( ExPropertySpec )args.Property );
				exProperty.Value = args.Value;
				exProperty.PropertyBag = null;
			}
			
			public static void GetValue( object sender, PropertySpecEventArgs args )
			{
				ExPropertySpec exProperty = ( ExPropertySpec )args.Property;
				if ( exProperty.PropertyBag != null )
				{
					args.Value = exProperty.PropertyBag;
				}
				else
				{
					args.Value = exProperty.Value;
				}
			}

			private static bool UseNullConverter( Type type )
			{
				TypeConverter converter = TypeDescriptor.GetConverter( type );
				return ( converter is ReferenceConverter ) || ( converter.GetType( ) == typeof( TypeConverter ) );
			}

			public ExPropertySpec( PropertyInfo property, object obj, string category ) :
				base( property.Name, property.PropertyType, category )
			{
				m_Object = obj;
				m_Property = property;

				List< Attribute > attributes = new List< Attribute >( );

				//	ReferenceConverter is rubbish, because it doesn't expand object instances
				if ( UseNullConverter( property.PropertyType ) )
				{
					ConverterTypeName = typeof( NullConverter ).FullName;
				}

				//	Add read-only attributes
				object[] srcAttributes = property.GetCustomAttributes( typeof( ReadOnlyAttribute ), false );
				if ( srcAttributes.Length > 0 )
				{
					attributes.Add( ( Attribute )srcAttributes[ 0 ] );
				}
				else if ( !property.CanWrite )
				{
					//attributes.Add( new ReadOnlyAttribute( true ) );
				}

				//	Add browsable attributes
				srcAttributes = property.GetCustomAttributes( typeof( BrowsableAttribute ), false );
				if ( srcAttributes.Length > 0 )
				{
					attributes.Add( ( Attribute )srcAttributes[ 0 ] );
				}

				if ( ObjectUITypeEditor.HandlesType( property.PropertyType ) )
				{
					//	Set the editor attribute to ObjectUITypeEditor, if the property's type doesn't have a nice
					//	default editor of its own
					attributes.Add( new EditorAttribute( typeof( ObjectUITypeEditor ), typeof( UITypeEditor ) ) );
				}

				Attributes = attributes.ToArray( );
			}

			/// <summary>
			/// Gets a property bag for this 
			/// </summary>
			/// <returns></returns>
			public PropertyBag PropertyBag
			{
				get
				{
					if ( m_Bag != null )
					{
						return m_Bag;
					}

					if ( !ObjectUITypeEditor.HandlesType( m_Property.PropertyType ) )
					{
						return null;
					}

					m_Bag = CreatePropertyBag( Value );
					return m_Bag;
				}
				set
				{
					m_Bag = value;
				}
			}

			public object Value
			{
				get { return m_Property.GetValue( m_Object, null ); }
				set { m_Property.SetValue( m_Object, value, null ); }
			}

			private PropertyBag m_Bag;
			private readonly object m_Object;
			private readonly PropertyInfo m_Property;
		}

		private static void FillPropertyBag( PropertyBag bag, object obj )
		{
			string category = ( obj.GetType( ).Name.Substring( obj.GetType( ).Name.LastIndexOf( '.' ) + 1 ) );
			foreach ( PropertyInfo property in obj.GetType( ).GetProperties( ) )
			{
				if ( property.CanRead && property.GetIndexParameters( ).Length == 0 )
				{
					bag.Properties.Add( new ExPropertySpec( property, obj, category ) );
				}
			}

			IParent parent = obj as IParent;
			if ( parent == null )
			{
				return;
			}

			foreach ( object childObj in parent.Children )
			{
				FillPropertyBag( bag, childObj );
			}
			
		}

		private static PropertyBag CreatePropertyBag( ObjectTemplate template )
		{
			PropertyBag bag = template.CreatePropertyBag( true );
			foreach ( PropertySpec property in bag.Properties )
			{
				if ( property.TypeName == typeof( ObjectTemplate ).FullName )
				{
					property.EditorTypeName = typeof( ObjectTemplateUITypeEditor ).FullName;
				}
			}
			return bag;
		}

		private class ObjectTemplateUITypeEditor : UITypeEditor
		{			
		}
	}
}