using System.Collections.Generic;
using System.Windows.Forms;
using Poc0.Core;
using Poc0.LevelEditor.Core.Actions;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.World;
using Rectangle=Rb.Core.Maths.Rectangle;

namespace Poc0.LevelEditor.Core.EditModes
{
	/// <summary>
	/// Edit mode that adds and removes objects from a selection
	/// </summary>
	public class SelectEditMode : EditMode
	{
		/// <summary>
		/// Binds the edit mode to a control and viewer
		/// </summary>
		/// <param name="actionButton">The mouse button that this edit mode listens out for</param>
		public SelectEditMode( MouseButtons actionButton )
		{
			m_ActionButton = actionButton;
		}

		#region Public members

		/// <summary>
		/// Called when the edit mode is activated
		/// </summary>
		public override void Start( )
		{
			foreach ( Control control in Controls )
			{
				control.MouseDown += OnMouseDown;
				control.MouseMove += OnMouseMove;
				control.MouseUp += OnMouseUp;
				control.KeyDown += OnKeyDown;
				control.KeyUp += OnKeyUp;
			}
		}

		/// <summary>
		/// Called when the edit mode is deactivated
		/// </summary>
		public override void Stop( )
		{
			foreach ( Control control in Controls )
			{
				control.MouseDown -= OnMouseDown;
				control.MouseMove -= OnMouseMove;
				control.MouseUp -= OnMouseUp;
				control.KeyDown -= OnKeyDown;
				control.KeyUp -= OnKeyUp;
			}
		}

		/// <summary>
		/// Gets the key that switches to this edit mode
		/// </summary>
		public override Keys HotKey
		{
			get { return Keys.Escape; }
		}

		/// <summary>
		/// Returns false (select can co-exist with other edit modes)
		/// </summary>
		public override bool Exclusive
		{
			get { return false; }
		}

		#endregion

		#region Control event handlers

		private void OnKeyDown( object sender, KeyEventArgs args )
		{
			if ( args.Control )
			{
				m_AddToSelection = true;
			}
		}

		private void OnKeyUp( object sender, KeyEventArgs args )
		{
			m_AddToSelection = false;
		}

		private void OnMouseDown( object sender, MouseEventArgs args )
		{
			if ( ( args.Button & m_ActionButton ) == 0 )
			{
				return;
			}

			Point2 pos = ( ( ITilePicker )sender ).CursorToWorld( args.X, args.Y );
			object obj = FirstObjectUnderCursor( pos );
			if ( obj == null )
			{
				m_SelectionStart = pos;
				m_MoveAction = null;
			}
			else
			{
				if ( !EditModeContext.Instance.Selection.IsSelected( obj ) )
				{
					EditModeContext.Instance.Selection.ApplySelect( obj, m_AddToSelection );
					m_DeselectOnNoMove = false;
				}
				else
				{
					m_DeselectOnNoMove = true;
				}
				m_LastCursorPos = pos;
				m_MoveAction = new MoveObjectsAction( EditModeContext.Instance.Selection.ToArray( ), new Vector3( ) );
			}
		}

		private void OnMouseMove( object sender, MouseEventArgs args )
		{
			ITilePicker picker = ( ITilePicker )sender;
			Point2 pos = picker.CursorToWorld( args.X, args.Y );

			if ( ( ( args.Button & m_ActionButton ) == 0 ) || ( !IsMoving ) )
			{
				if ( m_LastHighlit != null )
				{
					m_LastHighlit.Highlight = false;
				}
				m_LastHighlit = FirstObjectUnderCursor( pos ) as ISelectable;
				if ( m_LastHighlit != null )
				{
					m_LastHighlit.Highlight = true;
				}
			}
			else
			{
				Vector2 vec = pos - m_LastCursorPos;
				if ( vec.Length > 0 )
				{
					m_MoveAction.ApplyDelta( new Vector3( vec.X, 0, vec.Y ) );
				}
			}

			m_LastCursorPos = pos;
		}

		
		private void OnMouseUp( object sender, MouseEventArgs args )
		{
			if ( ( args.Button & m_ActionButton ) == 0 )
			{
				return;
			}
			if ( IsMoving )
			{
				if ( ( m_MoveAction.MovementDistance == 0 ) && ( m_DeselectOnNoMove ) )
				{
					object obj = FirstObjectUnderCursor( m_LastCursorPos );
					if ( obj != null )
					{
						EditModeContext.Instance.Selection.ApplySelect( obj, m_AddToSelection );
					}
				}
				else
				{
					EditModeContext.Instance.UndoStack.Push( m_MoveAction );
				}
				m_MoveAction = null;
			}
			else
			{
				m_SelectionEnd = ( ( ITilePicker )sender ).CursorToWorld( args.X, args.Y );

				Rectangle selectionBox = new Rectangle(  );
				selectionBox.X = Utils.Min( m_SelectionStart.X, m_SelectionEnd.X );
				selectionBox.Y = Utils.Min( m_SelectionStart.Y, m_SelectionEnd.Y );

				selectionBox.Width	= System.Math.Abs( m_SelectionStart.X -  m_SelectionEnd.X );
				selectionBox.Height	= System.Math.Abs( m_SelectionStart.Y - m_SelectionEnd.Y );

				object[] objects = ObjectsInBox( selectionBox );

				if ( objects.Length > 0 )
				{
					EditModeContext.Instance.Selection.ApplySelect( objects, m_AddToSelection );
				}
				else if ( !m_AddToSelection )
				{
					EditModeContext.Instance.Selection.ClearSelection( );
				}
			}
		}

		#endregion

		#region Private members

		private bool					m_AddToSelection;
		private readonly MouseButtons	m_ActionButton;
		private Point2					m_SelectionStart;
		private Point2					m_SelectionEnd;
		private Point2					m_LastCursorPos;
		private MoveObjectsAction		m_MoveAction;
		private bool					m_DeselectOnNoMove;
		private ISelectable				m_LastHighlit;

		/// <summary>
		/// Gets the shape of a given object
		/// </summary>
		/// <param name="obj">Object to retrieve shape from</param>
		/// <returns>Returns a 2D shape representation of the object</returns>
		private static IShape2 GetObjectShape( IHasPosition obj )
		{
			float minX = obj.Position.X - 5;
			float minY = obj.Position.Z - 5;

			return new Rectangle( minX, minY, 10.0f, 10.0f );
		}

		/// <summary>
		/// Gets the first object under the cursor
		/// </summary>
		/// <param name="pos">Cursor position</param>
		/// <returns>Returns null if there's nothing under the cursor. Otherwise, returns the first object under it</returns>
		private static object FirstObjectUnderCursor( Point2 pos )
		{
			Scene scene = EditModeContext.Instance.Scene;

			foreach ( object obj in scene.Objects.GetAllOfType< object >( ) )
			{
				IHasPosition frame = Parent.GetType< IHasPosition >( obj );
				if ( ( frame != null ) && ( GetObjectShape( frame ).Contains( pos ) ) )
				{
					return obj;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets an array of objects whose bounding areas intersect a given rectangle
		/// </summary>
		/// <param name="rect">Selection rectangle</param>
		/// <returns>Returns all objects that lie (at least partly) inside rect</returns>
		private static object[] ObjectsInBox( IShape2 rect )
		{
			List< object > objects = new List< object >( );

			Scene scene = EditModeContext.Instance.Scene;

			foreach ( object obj in scene.Objects.GetAllOfType< object >( ) )
			{
				IHasPosition frame = Parent.GetType< IHasPosition >( obj );
				if ( ( frame != null ) && ( GetObjectShape( frame ).Overlaps( rect ) ) )
				{
					objects.Add( obj );
				}
			}

			return objects.ToArray( );
		}

		/// <summary>
		/// True if the edit mode is moving about a selection of objects
		/// </summary>
		private bool IsMoving
		{
			get { return m_MoveAction != null; }
		}

		#endregion

	}
}
