using System;
using System.Windows.Forms;
using Poc0.LevelEditor.Core.Actions;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Log;

namespace Poc0.LevelEditor.Core.EditModes
{
	/// <summary>
	/// Edit mode for adding objects
	/// </summary>
	public class AddObjectEditMode : EditMode
	{
		/// <summary>
		/// Binds the edit mode to a control and viewer
		/// </summary>
		/// <param name="actionButton">The mouse button that this edit mode listens out for</param>
		/// <param name="template">Object template used for creating new objects. Must be ICloneable or IInstanceBuilder</param>
		public AddObjectEditMode( MouseButtons actionButton, object template )
		{
			m_ActionButton = actionButton;
			m_Template = template;
		}

		#region Control event handlers

		/// <summary>
		/// Handles mouse click events. Adds an object to the tile under the cursor
		/// </summary>
		private void OnMouseClick( object sender, MouseEventArgs args )
		{
			if ( args.Button != m_ActionButton )
			{
				return;
			}
			ITilePicker picker = ( ITilePicker )sender;
			Tile tile = picker.PickTile( EditModeContext.Instance.Grid, args.X, args.Y );
			if ( tile != null )
			{
				Point2 pt = picker.CursorToWorld( args.X, args.Y );

				EditorScene scene = EditModeContext.Instance.Scene;
				Guid id = Guid.NewGuid( );

				try
				{
					EditModeContext.Instance.UndoStack.Push( new AddObjectAction( scene, m_Template, pt.X, pt.Y, id ) );
				}
				catch ( Exception ex )
				{
					AppLog.Error( "Failed to add new object" );
					ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
					MessageBox.Show( Properties.Resources.FailedToAddObject, Properties.Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error );
				}
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Called when the edit mode is activated
		/// </summary>
		public override void Start( )
		{
			foreach ( Control control in Controls )
			{
				control.MouseClick += OnMouseClick;
			}
		}

		/// <summary>
		/// Called when the edit mode is deactivated
		/// </summary>
		public override void Stop( )
		{
			foreach ( Control control in Controls )
			{
				control.MouseClick -= OnMouseClick;
			}
		}

		#endregion

		#region Private members

		private readonly MouseButtons m_ActionButton;
		private readonly object m_Template;

		#endregion
	}
}
