using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

		private static void Flatten( object obj, IList objects )
		{
			objects.Add( obj );

			IParent parent = obj as IParent;
			if ( parent == null )
			{
				return;
			}

			foreach ( object childObj in parent.Children )
			{
				Flatten( childObj, objects );
			}
		}

		private static object[] Flatten(object obj)
		{
			ArrayList objects = new ArrayList( );

			Flatten( obj, objects );

			return objects.ToArray( );
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
				PropertyBag bag = new PropertyBag( );
				bags[ index ] = bag;

				object[] objects = Flatten( selectedObjects[ index ] );

				foreach ( object curObj in objects )
				{
					string category = curObj.GetType( ).Name;
					category = category.Substring( category.LastIndexOf( '.' ) + 1 );
					foreach ( PropertyInfo property in curObj.GetType( ).GetProperties( ) )
					{
						object currentValue = property.GetValue( curObj, null );
						bag.Properties.Add( new PropertySpec( property.Name, property.PropertyType, category, "", currentValue ) );
					}
				}
			}

			objectPropertyGrid.SelectedObjects = bags;
		}

	}
}
