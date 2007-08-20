using System.Windows.Forms;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core.EditModes
{
	/// <summary>
	/// Edit mode for adding objects
	/// </summary>
	class AddObjectEditMode : EditMode
	{
		/// <summary>
		/// Binds the edit mode to a control and viewer
		/// </summary>
		/// <param name="actionButton">The mouse button that this edit mode listens out for</param>
		public AddObjectEditMode( MouseButtons actionButton )
		{
			m_ActionButton = actionButton;
		}

		#region Control event handlers

		/// <summary>
		/// Handles mouse click events. Adds an object to the tiloe under the cursor
		/// </summary>
		private void OnMouseClick( object sender, MouseEventArgs args )
		{
			if ( args.Button != m_ActionButton )
			{
				return;
			}
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

		#endregion
	}
}
