using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Rb.Tools.LevelEditor.Core;

namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// UI editor for Material objects
	/// </summary>
	public class MaterialUITypeEditor : UITypeEditor
	{
		/// <summary>
		/// Returns the UITypeEditorEditStyle for this class
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context )
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Displays the UI for value selection.
		public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value )
		{
			IWindowsFormsEditorService edSvc = ( IWindowsFormsEditorService )provider.GetService( typeof( IWindowsFormsEditorService ) );
			if ( edSvc == null)
			{
				return value;
			}

			//	Get the material set from the current scene
			MaterialSet matSet = MaterialSet.FromScene( EditorState.Instance.CurrentScene, true );

			//	Can't show an actual combo box, because this is displayed under the original drop-down icon
			ListBox combo = new ListBox( );
			foreach ( Material mat in matSet.Materials )
			{
				combo.Items.Add( mat );
			}
			combo.SelectedItem = value;
			combo.BorderStyle = BorderStyle.None;
			combo.SelectedIndexChanged += delegate { edSvc.CloseDropDown( ); };
			edSvc.DropDownControl( combo );

			return combo.SelectedItem;
		}


	}
}
