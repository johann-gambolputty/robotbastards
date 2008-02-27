using System;
using System.Windows.Forms;
using Rb.Log;
using Rb.Tools.LevelEditor.Core.EditModes;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	public partial class EditModesControl : UserControl
	{
		public EditModesControl( )
		{
			InitializeComponent( );

			foreach ( IEditMode mode in EditorState.Instance.RegisteredEditModes )
			{
				RegisterEditMode( mode );
			}

			EditorState.Instance.EditModeRegistered += RegisterEditMode;
		}

		/// <summary>
		/// Adds an edit mode to the control
		/// </summary>
		private void RegisterEditMode( IEditMode mode )
		{
			Control control = mode.CreateControl( );
			if ( control == null )
			{
				AppLog.Warning( "Edit mode \"{0}\" did not create a control - it will not appear in the edit mode tab pages", mode.DisplayName );
				return;
			}
			
			AppLog.Info( "Adding Edit mode \"{0}\" to edit mode tab pages", mode.DisplayName );
			TabPage containerPage = new TabPage( mode.DisplayName );
			containerPage.Controls.Add( control );
			control.Dock = DockStyle.Fill;

			containerPage.Tag = mode;
			containerPage.Enter += EditModeTagPageEntered;

			editModeTabs.TabPages.Add( containerPage );
		}

		/// <summary>
		/// Called when the tab page for an edit mode is entered. Starts the associated edit mode.
		/// </summary>
		private static void EditModeTagPageEntered( object sender, EventArgs args )
		{
			IEditMode editMode = ( IEditMode )( ( ( TabPage )sender ).Tag );
			EditorState.Instance.ActivateEditMode( editMode );
		}
	}
}
