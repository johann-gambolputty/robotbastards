using System.Windows.Forms;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;

namespace Rb.Tools.LevelEditor.Core.EditModes
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
		/// Gets the mouse buttons used by this edit mode
		/// </summary>
		public override MouseButtons Buttons
		{
			get { return m_ActionButton; }
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

		#region Protected members

		/// <summary>
		/// Called when the edit mode is activated
		/// </summary>
		protected override void BindToControl( Control control )
		{
			control.MouseDown += OnMouseDown;
			control.MouseMove += OnMouseMove;
			control.MouseUp += OnMouseUp;
			control.KeyDown += OnKeyDown;
			control.KeyUp += OnKeyUp;
		}

		/// <summary>
		/// Called when the edit mode is deactivated
		/// </summary>
		protected override void UnbindFromControl( Control control )
		{
			control.MouseDown -= OnMouseDown;
			control.MouseMove -= OnMouseMove;
			control.MouseUp -= OnMouseUp;
			control.KeyDown -= OnKeyDown;
			control.KeyUp -= OnKeyUp;
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

			PickInfoCursor pick = ( ( IPicker )sender ).CreateCursorPickInfo( args.X, args.Y );
			IPickable obj = EditorState.Instance.CurrentScene.PickObject( pick ) as IPickable;
			if ( obj == null )
			{
				m_SelectionStart = pick;
				m_PickAction = null;
			}
			else
			{
				SelectionSet selection = EditorState.Instance.CurrentSelection;
				if ( selection.IsSelected( obj ) )
				{
					selection.ApplySelect( obj, m_AddToSelection );
					m_DeselectOnNoMove = false;
				}
				else
				{
					m_DeselectOnNoMove = true;
				}

				m_LastCursorPick = pick;
				m_PickAction = obj.CreatePickAction( pick );
				m_PickAction.AddObjects( selection.Selection );
			}
		}

		private void OnMouseMove( object sender, MouseEventArgs args )
		{
			IPicker picker = ( IPicker )sender;
			PickInfoCursor pick = picker.CreateCursorPickInfo( args.X, args.Y );

			if ( ( ( args.Button & m_ActionButton ) == 0 ) || ( !UsingPickAction ) )
			{
				if ( m_LastHighlit != null )
				{
					m_LastHighlit.Highlighted = false;
				}
				m_LastHighlit = EditorState.Instance.CurrentScene.PickObject( pick ) as ISelectable;
				if ( m_LastHighlit != null )
				{
					m_LastHighlit.Highlighted = true;
				}
			}
			else
			{
				m_PickAction.PickChanged( m_LastCursorPick, pick );
			}

			m_LastCursorPick = pick;
		}

		
		private void OnMouseUp( object sender, MouseEventArgs args )
		{
			if ( ( args.Button & m_ActionButton ) == 0 )
			{
				return;
			}
			if ( UsingPickAction )
			{
				if ( ( !m_PickAction.HasModifiedObjects ) && ( m_DeselectOnNoMove ) )
				{
					object obj = EditorState.Instance.CurrentScene.PickObject( m_LastCursorPick );
					if ( obj != null )
					{
						EditorState.Instance.CurrentSelection.ApplySelect( obj, m_AddToSelection );
					}
				}
				else
				{
					EditorState.Instance.CurrentUndoStack.Push( m_PickAction );
				}
				m_PickAction = null;
			}
			else
			{
				object[] objects = null;
				if ( ( m_SelectionStart.CursorX == args.X ) && ( m_SelectionStart.CursorY == args.Y ) )
				{
					objects = EditorState.Instance.CurrentScene.PickObjects( m_SelectionStart );
				}
				else
				{
					PickInfoCursor selectionEnd = ( ( IPicker )sender ).CreateCursorPickInfo( args.X, args.Y );

					if ( ( m_SelectionStart != null ) && ( selectionEnd != null ) )
					{
						IPickInfo selectionBox = ( ( IPicker )sender ).CreatePickBox( m_SelectionStart, selectionEnd );
						objects = EditorState.Instance.CurrentScene.PickObjects( selectionBox );
					}
				}
				
				if ( ( objects != null ) && ( objects.Length > 0 ) )
				{
					EditorState.Instance.CurrentSelection.ApplySelect( objects, m_AddToSelection );
				}
				else if ( !m_AddToSelection )
				{
					EditorState.Instance.CurrentSelection.ClearSelection( );
				}
				m_SelectionStart = null;
			}
		}

		#endregion

		#region Private members

		private bool					m_AddToSelection;
		private readonly MouseButtons	m_ActionButton;
		private PickInfoCursor			m_SelectionStart;
		private PickInfoCursor			m_LastCursorPick;
		private IPickAction				m_PickAction;
		private bool					m_DeselectOnNoMove;
		private ISelectable				m_LastHighlit;

		/// <summary>
		/// True if the edit mode is moving about a selection of objects
		/// </summary>
		private bool UsingPickAction
		{
			get { return m_PickAction != null; }
		}

		#endregion

	}
}
