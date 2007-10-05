using System;
using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Log;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;

namespace Rb.Tools.LevelEditor.Core.EditModes
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

		/// <summary>
		/// Gets the mouse buttons used by this edit mode
		/// </summary>
		public override MouseButtons Buttons
		{
			get { return m_ActionButton; }
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
			IPicker picker = ( IPicker )sender;

			ILineIntersection pick = picker.FirstPick( args.X, args.Y );
			if ( pick != null )
			{
				Guid id = Guid.NewGuid( );

				try
				{
					IAction addAction = new AddObjectAction( m_Template, pick, id );
					EditorState.Instance.CurrentUndoStack.Push( addAction );
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
		/// Binds to the specified control
		/// </summary>
		/// <param name="control">Control to bind to</param>
		protected override void BindToControl( Control control )
		{
			control.MouseClick += OnMouseClick;
		}

		/// <summary>
		/// Unbinds to the specified control
		/// </summary>
		/// <param name="control">Control to unbind from</param>
		protected override void UnbindFromControl( Control control )
		{
			control.MouseClick -= OnMouseClick;
		}

		#endregion

		#region Private members

		private readonly MouseButtons m_ActionButton;
		private readonly object m_Template;

		#endregion
	}
}
