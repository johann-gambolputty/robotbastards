using System.Collections.Generic;
using System.Windows.Forms;
using Poc0.Core;
using Rb.World;

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
				control.MouseClick += OnMouseClick;
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
				control.MouseClick -= OnMouseClick;
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
			if ( args.Control )
			{
				m_AddToSelection = false;
			}
		}

		private void OnMouseClick( object sender, MouseEventArgs args )
		{
			if ( args.Button != m_ActionButton )
			{
				return;
			}

			Scene scene = EditModeContext.Instance.Scene;
			IEnumerable< IHasWorldFrame > hasFrames = scene.Objects.GetAllOfType< IHasWorldFrame >( );
			foreach ( IHasWorldFrame hasFrame in hasFrames )
			{
				Frame frame = hasFrame.WorldFrame;
				int minX = ( int )frame.Position.X - 5;
				int minY = ( int )frame.Position.Z - 5;
				int maxX = minX + 10;
				int maxY = minY + 10;

				if ( ( args.X >= minX ) && ( args.X <= maxX ) && ( args.Y >= minY ) && ( args.Y <= maxY ) )
				{
					EditModeContext.Instance.Selection.ApplySelect( hasFrame, m_AddToSelection );
				}
			}
		}

		#endregion

		#region Private members

		private bool					m_AddToSelection;
		private readonly MouseButtons	m_ActionButton;

		#endregion
	}
}
