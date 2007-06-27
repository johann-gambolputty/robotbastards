using System;
using System.Windows.Forms;

namespace Rb.Interaction.Windows
{
    /// <summary>
    /// Mouse cursor input
    /// </summary>
    public class MouseCursorInput : CursorInput
    {

		/// <summary>
		/// Returns true (otherwise the mouse input will remain active if the user doesn't move the mouse)
		/// </summary>
		public override bool DeactivateOnUpdate
		{
			get { return true; }
		} 

		/// <summary>
		/// Setup constructor. Specifies the button that must be pressed while the mouse is being moved, for the command to fire
		/// </summary>
        /// <param name="context">Input context</param>
		/// <param name="button">Button to press</param>
		public MouseCursorInput( InputContext context, MouseButtons button ) :
			base( context )
		{
			m_Button = button;

			( ( Control )context.Control ).MouseMove += new MouseEventHandler( OnMouseMove );
			( ( Control )context.Control ).MouseLeave += new EventHandler( OnMouseLeave );
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
                IsActive = true;
			}
			else
			{
                IsActive = ( args.Button == m_Button );
			}

			m_X = args.X;
			m_Y = args.Y;
		}

		private void OnMouseLeave( object sender, EventArgs args )
		{
			IsActive = false;
		}

		private MouseButtons m_Button;
    }
}
