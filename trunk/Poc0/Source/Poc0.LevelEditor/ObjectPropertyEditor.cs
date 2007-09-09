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
		/// <summary>
		/// Default constructor
		/// </summary>
		public ObjectPropertyEditor( )
		{
			InitializeComponent( );

			EditModeContext.Instance.Selection.ObjectSelected += OnSelectionChanged;
			EditModeContext.Instance.Selection.ObjectDeselected += OnSelectionChanged;
		}

		/// <summary>
		/// Called when the current selection changes
		/// </summary>
		/// <param name="obj">Added/removed object</param>
		private void OnSelectionChanged( object obj )
		{
			object[] selectedObjects = EditModeContext.Instance.Selection.ToArray( );

			if ( selectedObjects.Length == 0 )
			{
				objectPropertyGrid.SelectedObject = null;
				return;
			}

			PropertyBag[] bags = new PropertyBag[ selectedObjects.Length ];

			for ( int index = 0; index < selectedObjects.Length; ++index )
			{
				bags[ index ] = CreatePropertyBag( selectedObjects[ index ] );
			}

			objectPropertyGrid.SelectedObjects = bags;
		}

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

			FillPropertyBag( bag, obj );

			return bag;
		}

		private static void FillPropertyBag( PropertyBag bag, object obj )
		{
			string category = ( obj.GetType( ).Name.Substring( obj.GetType( ).Name.LastIndexOf( '.' ) + 1 ) );
			foreach ( PropertyInfo property in obj.GetType( ).GetProperties( ) )
			{
				bag.Properties.Add( new PropertySpec( property.Name, property.PropertyType, category ) );
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