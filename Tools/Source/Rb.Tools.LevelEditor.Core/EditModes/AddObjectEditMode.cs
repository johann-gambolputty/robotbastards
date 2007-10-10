using System;
using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Log;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

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
		/// <param name="pickOptions">Parameters for intersections used to find the position of new objects</param>
		public AddObjectEditMode( MouseButtons actionButton, object template, RayCastOptions pickOptions )
		{
			m_ActionButton = actionButton;
			m_Template = template;
			m_PickOptions = pickOptions;
		}

		/// <summary>
		/// Gets the mouse buttons used by this edit mode
		/// </summary>
		public override MouseButtons Buttons
		{
			get { return m_ActionButton; }
		}

		/// <summary>
		/// Returns a description of the edit mode inputs
		/// </summary>
		public override string InputDescription
		{
			get
			{
				return string.Format( Properties.Resources.AddObjectInputs, ResourceHelper.MouseButtonName( Buttons ) );
			}
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

			ILineIntersection pick = picker.FirstPick( args.X, args.Y, m_PickOptions );
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
					AppLog.Exception( ex, "Failed to add new object" );
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
		private readonly RayCastOptions m_PickOptions;

		#endregion
	}
}
