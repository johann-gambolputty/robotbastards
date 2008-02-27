using System;
using System.Windows.Forms;

namespace Rb.Tools.LevelEditor.Core.EditModes
{
	/// <summary>
	/// An edit mode determines how user actions get applied
	/// </summary>
	public interface IEditMode
	{
		/// <summary>
		/// Raised by Stop()
		/// </summary>
		event EventHandler Stopped;

		/// <summary>
		/// Gets the display name of this edit mode
		/// </summary>
		string DisplayName
		{
			get;
		}

		/// <summary>
		/// Creates a control used to manipulate the edit mode
		/// </summary>
		/// <remarks>
		/// This control is added to the edit mode tab pages (<see cref="Controls.Forms.EditModesControl"/>)
		/// </remarks>
		Control CreateControl( );

		/// <summary>
		/// Called when the edit mode is activated
		/// </summary>
		void Start( );

		/// <summary>
		/// Called when the edit mode is deactivated
		/// </summary>
		void Stop( );

		/// <summary>
		/// Returns true if the mode has started
		/// </summary>
		bool IsRunning
		{
			get;
		}

		/// <summary>
		/// Gets the key that switches to this edit mode
		/// </summary>
		Keys HotKey
		{
			get;
		}

		/// <summary>
		/// Gets the mouse buttons used by this edit mode
		/// </summary>
		MouseButtons Buttons
		{
			get;
		}

		/// <summary>
		/// Returns a description of the edit mode inputs
		/// </summary>
		string InputDescription
		{
			get;
		}

		/// <summary>
		/// There can be only one exclusive edit mode active at any one time
		/// </summary>
		bool Exclusive
		{
			get;
		}
	}
}
