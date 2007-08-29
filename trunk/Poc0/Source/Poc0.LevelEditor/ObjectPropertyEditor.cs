using System;
using System.Drawing;
using System.Windows.Forms;
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

			subObjectComboBox.Items.Clear( );
			if ( selectedObjects.Length == 0 )
			{
				objectPropertyGrid.SelectedObject = null;
				return;
			}

			foreach ( object selectedObj in selectedObjects )
			{
				AddObjectToCombo( 0, selectedObj );
				subObjectComboBox.AddSeparator( );
			}
			subObjectComboBox.SelectedIndex = 0;
			objectPropertyGrid.SelectedObject = selectedObjects[ 0 ];
		}

		/// <summary>
		/// Adds an object to the sub object combo box
		/// </summary>
		/// <param name="depth">Current depth</param>
		/// <param name="obj">Current object</param>
		private void AddObjectToCombo( int depth, object obj )
		{
			string str = obj.GetType( ).ToString( );
			str = str.Substring( str.LastIndexOf( '.' ) + 1 );

			NiceComboBox.Item item = new NiceComboBox.Item( depth, str, depth == 0 ? FontStyle.Bold : 0, null, null, obj );
			subObjectComboBox.Items.Add( item );

			IParent parent = obj as IParent;
			if ( parent != null )
			{
				foreach ( object childObj in parent.Children )
				{
					AddObjectToCombo( depth + 1, childObj );
				}
			}
		}

		private void subObjectComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			objectPropertyGrid.SelectedObject = subObjectComboBox.GetTag( subObjectComboBox.SelectedIndex );
		}
	}
}
