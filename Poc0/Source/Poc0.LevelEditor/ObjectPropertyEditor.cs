using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Core.Components;

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

			//object obj = ( ( ObjectEditState )selectedObjects[ 0 ] ).Instance;

			//objectPropertyGrid.SelectedObject = Rb.Core.Components.Parent.GetType< Poc0.Core.Entity >( obj );
			//return;

			PropertyBag[] bags = new PropertyBag[ selectedObjects.Length ];
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
			if ( obj is ObjectEditState )
			{
				return CreatePropertyBag( ( ( ObjectEditState )obj ).Instance );
			}

			if ( obj is ObjectTemplate )
			{
				return CreatePropertyBag( ( ObjectTemplate )obj );
			}

			PropertyBag bag = new PropertyBag( );
			bag.GetValue += ExPropertySpec.GetValue;
			bag.SetValue += ExPropertySpec.SetValue;

			FillPropertyBag( bag, obj );

			return bag;
		}

		private class ExPropertySpec : PropertySpec
		{
			public static void SetValue( object sender, PropertySpecEventArgs args )
			{
				( ( ExPropertySpec )args.Property ).Value = args.Value;
			}
			
			public static void GetValue( object sender, PropertySpecEventArgs args )
			{
				args.Value = ( ( ExPropertySpec )args.Property ).Value;
			}

			public ExPropertySpec( PropertyInfo property, object obj, string category ) :
				base( property.Name, property.PropertyType, category )
			{
				m_Object = obj;
				m_Property = property;

				//EditorTypeName = typeof( ObjectUITypeEditor ).FullName;

				//DefaultValue = new object( );

				List< Attribute > attributes = new List< Attribute >( );

				object[] srcAttributes = property.GetCustomAttributes( typeof( ReadOnlyAttribute ), false );
				if ( srcAttributes.Length > 0 )
				{
					attributes.Add( ( Attribute )srcAttributes[ 0 ] );
				}
				attributes.Add( new EditorAttribute( typeof( ObjectUITypeEditor ), typeof( UITypeEditor ) ) );

				Attributes = attributes.ToArray( );
			}

			public object Value
			{
				get { return m_Property.GetValue( m_Object, null ); }
				set { m_Property.SetValue( m_Object, value, null ); }
			}

			private readonly object m_Object;
			private readonly PropertyInfo m_Property;
		}

		private static void FillPropertyBag( PropertyBag bag, object obj )
		{
			string category = ( obj.GetType( ).Name.Substring( obj.GetType( ).Name.LastIndexOf( '.' ) + 1 ) );
			foreach ( PropertyInfo property in obj.GetType( ).GetProperties( ) )
			{
				bag.Properties.Add( new ExPropertySpec( property, obj, category ) );
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