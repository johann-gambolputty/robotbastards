using System;
using System.Windows.Forms;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Binds a command to mouse movement
	/// </summary>
	public class CommandMouseMoveInputBinding : CommandCursorInputBinding
	{
		/// <summary>
		/// Setup constructor. Specifies the button that must be pressed while the mouse is being moved, for the command to fire
		/// </summary>
		/// <param name="button">Button to press</param>
		public CommandMouseMoveInputBinding( Scene.SceneView view, MouseButtons button ) :
			base( view )
		{
			m_Button = button;

			view.Control.MouseMove += new MouseEventHandler( OnMouseMove );
			view.Control.MouseLeave += new EventHandler( OnMouseLeave );
		}

		/// <summary>
		/// Triggers the command, if the appropriate mouse button is pressed
		/// </summary>
		private void OnMouseMove( object sender, MouseEventArgs args )
		{
			m_LastX = m_X;
			m_LastY = m_Y;

			if ( m_Button == MouseButtons.None )
			{
				m_Active = true;
			}
			else
			{
				m_Active = ( args.Button == m_Button );
			}

			m_X = args.X;
			m_Y = args.Y;
		}

		private void			OnMouseLeave( object sender, EventArgs args )
		{
			m_Active = false;
		}

		private MouseButtons	m_Button;

	}
}
