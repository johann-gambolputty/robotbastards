using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using Rb.Core.Components;
using Rb.Tools.LevelEditor.Core.Selection;
using ComponentModel_TypeConverter=System.ComponentModel.TypeConverter;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
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

			if ( EditorState.Instance.CurrentSelection != null )
			{
				EditorState.Instance.CurrentSelection.ObjectSelected += ObjectSelected;
				EditorState.Instance.CurrentSelection.ObjectSelected += ObjectDeselected;
			}
			EditorState.Instance.SceneOpened += OnSceneOpened;
			EditorState.Instance.SceneClosed += OnSceneClosed;

			SceneSerializer.Instance.PreSerialize += OnPreSceneSerialized;
			SceneSerializer.Instance.PostSerialize += OnPostSceneSerialized;
		}

		private bool m_RefreshValues;

		private void OnPreSceneSerialized( object sender, EventArgs args )
		{
			foreach ( object obj in EditorState.Instance.CurrentSelection.Selection )
			{
				IObjectEditor editor = obj as IObjectEditor;
				if ( editor != null )
				{
					editor.ObjectChanged -= OnObjectChanged;
				}
			}
		}

		private void OnPostSceneSerialized( object sender, EventArgs args )
		{
			foreach ( object obj in EditorState.Instance.CurrentSelection.Selection )
			{
				IObjectEditor editor = obj as IObjectEditor;
				if ( editor != null )
				{
					editor.ObjectChanged += OnObjectChanged;
				}
			}
		}

		/// <summary>
		/// Called when a new scene is opened
		/// </summary>
		/// <param name="state">Opened scene's edit state</param>
		private void OnSceneOpened( SceneEditState state )
		{
			state.SelectedObjects.ObjectSelected += ObjectSelected;
			state.SelectedObjects.ObjectDeselected += ObjectDeselected;
		}

		/// <summary>
		/// Called when a scene is about to be closed
		/// </summary>
		/// <param name="state">Closing scene's edit state</param>
		private void OnSceneClosed( SceneEditState state )
		{
			state.SelectedObjects.ObjectSelected -= ObjectSelected;
			state.SelectedObjects.ObjectDeselected -= ObjectDeselected;
		}

		/// <summary>
		/// Called when an object is selected. Rebuilds the property grid
		/// </summary>
		private void ObjectSelected( object obj )
		{
			//	We want to be notified if the object changes, so the property grid can be updated
			if ( obj is IObjectEditor )
			{
				( ( IObjectEditor )obj ).ObjectChanged += OnObjectChanged;
			}
			BuildPropertyGrid( );
		}

		/// <summary>
		/// Called when an object is deselected. Rebuilds the property grid
		/// </summary>
		private void ObjectDeselected( object obj )
		{
			//	Remove change notification
			if ( obj is IObjectEditor )
			{
				( ( IObjectEditor )obj ).ObjectChanged -= OnObjectChanged;
			}
			BuildPropertyGrid( );
		}

		/// <summary>
		/// Called every 1/10th of a second to update the property grid if one of the selected objects has changed
		/// </summary>
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
		/// Called when a selected objects properties change
		/// </summary>
		private void OnObjectChanged( object sender, EventArgs args )
		{
			m_RefreshValues = true;
			m_Timer.Start( );
		}

		/// <summary>
		/// Builds the property grid from the current object selection
		/// </summary>
		private void BuildPropertyGrid( )
		{
			object[] selectedObjects = EditorState.Instance.CurrentSelection.ToArray( );

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

		/// <summary>
		/// Creates a property bag for a given object
		/// </summary>
		private static PropertyBag CreatePropertyBag( object obj )
		{
			if ( obj == null )
			{
				return null;
			}

			//if ( obj is IObjectEditor )
			//{
			//    return CreatePropertyBag( obj );
			//}
			ISelectionModifier modifier = obj as ISelectionModifier;
			if ( modifier != null )
			{
				obj = modifier.SelectedObject;
			}

			ExPropertyBag bag = new ExPropertyBag( obj );
			return bag;
		}


		/// <summary>
		/// Extended property bag
		/// </summary>
		[TypeConverter( typeof( ExpandableObjectConverter ) )]
		private class ExPropertyBag : PropertyBag
		{
			/// <summary>
			/// Sets up this bag
			/// </summary>
			/// <param name="obj">The object that this bag represents</param>
			public ExPropertyBag( object obj )
			{
				m_Name = obj.ToString( );

				GetValue += ExPropertySpec.GetValue;
				SetValue += ExPropertySpec.SetValue;
				BuildBag( obj );
			}

			/// <summary>
			/// Returns the name of this bag
			/// </summary>
			public override string ToString( )
			{
				return m_Name;
			}

			private readonly string m_Name;

			/// <summary>
			/// Builds this bag
			/// </summary>
			/// <param name="obj">The object that this bag represents</param>
			private void BuildBag( object obj )
			{			
				string category = ( obj.GetType( ).Name.Substring( obj.GetType( ).Name.LastIndexOf( '.' ) + 1 ) );
				foreach ( PropertyInfo property in obj.GetType( ).GetProperties( ) )
				{
					if ( property.CanRead && property.GetIndexParameters( ).Length == 0 )
					{
						ExPropertySpec propertySpec = new ExPropertySpec( obj, property, category );

						//propertySpec.OwnerChanged += delegate( object sender, EventArgs args )
						//    {
						//        property.SetValue( obj, sender, null );
						//    };

						Properties.Add( propertySpec );
					}
				}

				IParent parent = obj as IParent;
				if ( parent == null )
				{
					return;
				}

				foreach ( object childObj in parent.Children )
				{
					BuildBag( childObj );
				}
			}
		}
		
		/// <summary>
		/// Extended property specifier
		/// </summary>
		private class ExPropertySpec : PropertySpec
		{
			/// <summary>
			/// Sets the value of an extended property
			/// </summary>
			public static void SetValue( object sender, PropertySpecEventArgs args )
			{
				ExPropertySpec exProperty = ( ( ExPropertySpec )args.Property );
				exProperty.Value = args.Value;
			}

			/// <summary>
			/// Gets the value of an extended property
			/// </summary>
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

			/// <summary>
			/// Builds this extended property
			/// </summary>
			/// <param name="property">Object property being represented</param>
			/// <param name="owner">Object containing the property</param>
			/// <param name="category">Category that the property should be displayed in</param>
			public ExPropertySpec( object owner, PropertyInfo property, string category ) :
				base( property.Name, property.PropertyType, category )
			{
				m_Owner = owner;
				m_Property = property;

				DetermineAttributes( property );
			}

			/// <summary>
			/// Updates the underlying property bag, if this property needs one
			/// </summary>
			public void UpdateBag( )
			{
				if ( ObjectUITypeEditor.HandlesType( m_Property.PropertyType ) && ( Value != null ) )
				{
					m_Bag = new ExPropertyBag( Value );
				}
			}

			/// <summary>
			/// Gets/sets the property bag for this property
			/// </summary>
			public PropertyBag PropertyBag
			{
				get
				{
					UpdateBag( );
					return m_Bag;
				}
			}

			/// <summary>
			/// Gets/sets the values of the property
			/// </summary>
			public object Value
			{
				get { return m_Property.GetValue( m_Owner, null ); }
				set
				{
					m_Property.SetValue( m_Owner, value, null );
					UpdateBag( );
				}
			}

			private readonly object m_Owner;
			private readonly PropertyInfo m_Property;
			private PropertyBag m_Bag;
			
			/// <summary>
			/// Returns true if a given type should use the null converter
			/// </summary>
			private static bool UseNullConverter( Type type )
			{
				TypeConverter converter = TypeDescriptor.GetConverter( type );
				return ( converter is ReferenceConverter ) || ( converter.GetType( ) == typeof( TypeConverter ) );
			}
			
			/// <summary>
			/// Determines the attributes that this property should have
			/// </summary>
			private void DetermineAttributes( PropertyInfo property )
			{
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

				srcAttributes = property.GetCustomAttributes( typeof( EditorAttribute ), false );
				if ( srcAttributes.Length > 0 )
				{
					attributes.Add( ( Attribute )srcAttributes[ 0 ] );
				}
				else if ( ObjectUITypeEditor.HandlesType( property.PropertyType ) )
				{
					//	Set the editor attribute to ObjectUITypeEditor, if the property's type doesn't have a nice
					//	default editor of its own
					attributes.Add( new EditorAttribute( typeof( ObjectUITypeEditor ), typeof( UITypeEditor ) ) );
				}

				Attributes = attributes.ToArray( );
			}

		}

		/// <summary>
		/// Null object converter
		/// </summary>
		public class NullConverter : ExpandableObjectConverter
		{
			public override bool GetCreateInstanceSupported( ITypeDescriptorContext context )
			{
				Type type = context.PropertyDescriptor.PropertyType;

				return type.IsValueType;
			}

			public override object CreateInstance( ITypeDescriptorContext context, IDictionary propertyValues )
			{
				Type type = context.PropertyDescriptor.PropertyType;
				object obj = Activator.CreateInstance( type );
				foreach ( DictionaryEntry entry in propertyValues )
				{
					PropertyInfo property = type.GetProperty( ( string )entry.Key );
					property.SetValue( obj, entry.Value, null );
				}

				return obj;
			}

			public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType)
			{
				return false;
			}

			public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType)
			{
				return false;
			}
		}
	}
}