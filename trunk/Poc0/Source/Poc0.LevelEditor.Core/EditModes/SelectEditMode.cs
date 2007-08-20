using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Rb.Rendering;
using Rb.World;

namespace Poc0.LevelEditor.Core.EditModes
{
	/*
	/// <summary>
	/// Edit mode that adds and removes objects from a selection
	/// </summary>
	public class SelectEditMode : EditMode
	{
		/// <summary>
		/// Binds the edit mode to a control and viewer
		/// </summary>
		/// <param name="control">Control to listen to events from</param>
		/// <param name="viewer">Viewer attached to the control</param>
		/// <param name="actionButton">The mouse button that this edit mode listens out for</param>
		/// <param name="context">Editing context</param>
		public SelectEditMode( Control control, Viewer viewer, EditModeContext context, MouseButtons actionButton ) :
			base( control, viewer, context )
		{
			m_ActionButton = actionButton;
		}

		#region Public members

		/// <summary>
		/// Called when the edit mode is activated
		/// </summary>
		public override void Start( )
		{
			Control.MouseClick += OnMouseClick;
			Control.KeyDown += OnKeyDown;
			Control.KeyUp += OnKeyUp;
		}

		/// <summary>
		/// Called when the edit mode is deactivated
		/// </summary>
		public override void Stop( )
		{
			Control.MouseClick -= OnMouseClick;
			Control.KeyDown -= OnKeyDown;
			Control.KeyUp -= OnKeyUp;
		}

		/// <summary>
		/// Gets the key that switches to this edit mode
		/// </summary>
		public override Keys HotKey
		{
			get { return Keys.Escape; }
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

			//	TODO: AP: Select objects added
			ITilePicker picker = Viewer.Camera as ITilePicker;
			if ( picker == null )
			{
				return;
			}
			Tile pickedTile = picker.PickTile( EditContext.Grid, args.X, args.Y );
			if ( pickedTile != null )
			{
				EditContext.Selection.ApplySelect( pickedTile, m_AddToSelection );
			}
		}

		#endregion

		#region Private members

		private bool					m_AddToSelection;
		private readonly MouseButtons	m_ActionButton;

		#endregion
	}
	*/
}
