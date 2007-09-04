using System.Reflection;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;

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
				bags[ index ] = ( ( ObjectPattern )selectedObjects[ index ] ).CreatePropertyBag( );
			}

			objectPropertyGrid.SelectedObjects = bags;
		}

	}
}
