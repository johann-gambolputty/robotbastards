using System;
using System.Windows.Forms;
using Poc0.Core;
using Poc0.LevelEditor.Core.Actions;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Log;
using Rb.World;

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
		/// <param name="template">Object template used for creating new objects</param>
		public AddObjectEditMode( MouseButtons actionButton, ObjectPattern template )
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

				Scene scene = EditModeContext.Instance.Scene;
				Guid id = Guid.NewGuid( );

				EditModeContext.Instance.UndoStack.Push( new AddObjectAction( scene, m_Template, pt.X, pt.Y, id ) );
			}
		}

		/// <summary>
		/// Creates an object from the object template
		/// </summary>
		private object CreateObject( Scene scene, float x, float y, Guid id )
		{
			//  (doesn't actually instance the template; creates an ObjectHolder around it)
			ObjectPattern root = ( ObjectPattern )m_Template.Clone( );

			IHasWorldFrame frame = Parent.GetType< IHasWorldFrame >( root );
			if ( frame != null )
			{
				frame.WorldFrame.Translation = new Point3( x, 0, y );
				root.AddChild( new ObjectEditState( scene, frame ) );
			}

			return root;
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
		private readonly ObjectPattern m_Template;

		#endregion
	}
}
