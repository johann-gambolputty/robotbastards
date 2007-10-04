using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rb.Tools.LevelEditor.Core.EditModes
{
	/// <summary>
	/// Base class implementation of <see cref="IEditMode"/>
	/// </summary>
	public class EditMode : IEditMode
	{
		#region IEditMode Members

		/// <summary>
		/// Raised by Stop()
		/// </summary>
		public event EventHandler Stopped;

		/// <summary>
		/// Returns true if the mode has started
		/// </summary>
		public bool IsRunning
		{
			get { return m_Started; }
		}

		/// <summary>
		/// Returns <see cref="Keys.None"/> (no hotkey activates this edit mode)
		/// </summary>
		public virtual Keys HotKey
		{
			get { return Keys.None; }
		}

		/// <summary>
		/// Gets the mouse buttons used by this edit mode
		/// </summary>
		public virtual MouseButtons Buttons
		{
			get { return MouseButtons.None; }
		}

		/// <summary>
		/// Returns true
		/// </summary>
		public virtual bool Exclusive
		{
			get { return true; }
		}

		/// <summary>
		/// Called when the edit mode is activated
		/// </summary>
		public virtual void Start( )
		{
			foreach ( Control control in EditorState.Instance.EditModeControls )
			{
				NewControl( control );
			}
			EditorState.Instance.ControlAdded += NewControl;
			m_Started = true;
		}

		/// <summary>
		/// Called when the edit mode is deactivated
		/// </summary>
		public virtual void Stop( )
		{
			m_Started = false;
			foreach ( Control control in m_Controls )
			{
				UnbindFromControl( control );
			}
			m_Controls.Clear( );

			EditorState.Instance.ControlAdded -= NewControl;

			if ( Stopped != null )
			{
				Stopped( this, null );
			}
		}

		#region Protected members

		/// <summary>
		/// Binds this edit mode to a given control
		/// </summary>
		/// <param name="control">Control to bind to</param>
		protected virtual void BindToControl( Control control )
		{
		}

		/// <summary>
		/// Unbinds this edit mode from a given control
		/// </summary>
		/// <param name="control">Control to unbind from</param>
		protected virtual void UnbindFromControl( Control control )
		{
		}

		#endregion

		#endregion

		#region Private members

		private readonly List< Control > m_Controls = new List< Control >( );
		private bool m_Started;
		
		/// <summary>
		/// Called to bind to a new control 
		/// </summary>
		/// <param name="control">New control</param>
		private void NewControl( Control control )
		{
			m_Controls.Add( control );
			BindToControl( control );
		}

		#endregion

	}
}
