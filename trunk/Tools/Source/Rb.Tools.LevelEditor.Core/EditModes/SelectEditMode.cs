using System.Drawing;
using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

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
		/// <param name="pickOptions">Pick raycast options</param>
		public SelectEditMode( MouseButtons actionButton, RayCastOptions pickOptions )
		{
			m_ActionButton = actionButton;
			m_PickOptions = pickOptions;
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
		/// Returns a description of the edit mode inputs
		/// </summary>
		public override string InputDescription
		{
			get
			{
				return string.Format( Properties.Resources.SelectInputs, ResourceHelper.MouseButtonName( Buttons ) );
			}
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

		#region Private members

		/// <summary>
		/// Raycast options for standard selection pick
		/// </summary>
		private RayCastOptions SelectionPickOptions
		{
			get { return m_PickOptions; }
		}

		/// <summary>
		/// Raycast options for standard selection pick, or the current pick action, if it is active
		/// </summary>
		private RayCastOptions CurrentPickOptions
		{
			get { return UsingPickAction ? m_PickAction.PickOptions : SelectionPickOptions; }
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

			if ( m_CursorPick == null )
			{
				m_SelectionStart = args.Location;
				m_PickAction = null;
			}
			else
			{
				IPickable pickable = m_CursorPick.IntersectedObject as IPickable;
				if ( pickable != null )
				{
					SelectionSet selection = EditorState.Instance.CurrentSelection;

					m_PickAction = pickable.CreatePickAction( m_CursorPick );
					m_PickAction.AddObjects( selection.Selection );

					if ( !selection.IsSelected( pickable ) )
					{
						m_PickAction.AddObjects( new object[] { pickable } );
					}
				}
			}
		}

		private void OnMouseMove( object sender, MouseEventArgs args )
		{
			IPicker picker = ( IPicker )sender;
			ILineIntersection pick = picker.FirstPick( args.X, args.Y, CurrentPickOptions );
			
			if ( pick != null )
			{
				if ( ( ( args.Button & m_ActionButton ) == 0 ) || ( !UsingPickAction ) )
				{
					if ( m_LastHighlit != null )
					{
						m_LastHighlit.Highlighted = false;
					}
					m_LastHighlit = pick.IntersectedObject as ISelectable;
					if ( m_LastHighlit != null )
					{
						m_LastHighlit.Highlighted = true;
					}
				}
				else
				{
					m_PickAction.PickChanged( m_CursorPick, pick );
				}
			}

			m_LastCursorPick = m_CursorPick;
			m_CursorPick = pick;
		}

		
		private void OnMouseUp( object sender, MouseEventArgs args )
		{
			if ( ( args.Button & m_ActionButton ) == 0 )
			{
				return;
			}
			if ( UsingPickAction )
			{
				if ( !m_PickAction.HasModifiedObjects )
				{
					object obj = m_LastCursorPick == null ? null : m_LastCursorPick.IntersectedObject;
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
				if ( ( m_SelectionStart.X == args.X ) && ( m_SelectionStart.Y == args.Y ) )
				{
					ILineIntersection pick = ( ( IPicker )sender ).FirstPick( args.X, args.Y, SelectionPickOptions );
					if ( pick != null )
					{
						objects = new object[] { pick.IntersectedObject };
					}
				}
				else
				{
					objects = ( ( IPicker )sender ).GetObjectsInBox( m_SelectionStart.X, m_SelectionStart.Y, args.X, args.Y );
				}
				
				if ( ( objects != null ) && ( objects.Length > 0 ) )
				{
					EditorState.Instance.CurrentSelection.ApplySelect( objects, m_AddToSelection );
				}
				else if ( !m_AddToSelection )
				{
					EditorState.Instance.CurrentSelection.ClearSelection( );
				}
			}
		}

		#endregion

		#region Private members

		private bool					m_AddToSelection;
		private readonly MouseButtons	m_ActionButton;
		private readonly RayCastOptions	m_PickOptions;
		private Point					m_SelectionStart;
		private ILineIntersection		m_LastCursorPick;
		private ILineIntersection		m_CursorPick;
		private IPickAction				m_PickAction;
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
