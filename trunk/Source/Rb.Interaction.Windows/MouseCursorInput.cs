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
		/// <param name="key">Key to press</param>
		public MouseCursorInput( InputContext context, MouseButtons button, Keys key ) :
			base( context )
		{
			m_Button = button;
			m_Key = key;

			Control control = ( ( Control )context.Control );
			if ( m_Key != Keys.None )
			{
				control.KeyDown += OnKeyDown;
				control.KeyUp += OnKeyUp;
			}
			else
			{
				m_KeyPressed = true;
			}

			control.MouseMove += new MouseEventHandler( OnMouseMove );
			control.MouseLeave += new EventHandler( OnMouseLeave );
		}

		/// <summary>
		/// Called when a key is pressed
		/// </summary>
		private void OnKeyDown( object sender, KeyEventArgs args )
		{
			m_KeyPressed = args.KeyCode == m_Key;
		}

		/// <summary>
		/// Called when a key is er... unpressed?
		/// </summary>
		private void OnKeyUp( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == m_Key )
			{
				m_KeyPressed = false;
			}
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
				IsActive = m_KeyPressed;
			}
			else
			{
                IsActive = ( m_KeyPressed ) && ( args.Button == m_Button );
			}

			m_X = args.X;
			m_Y = args.Y;
		}

		private void OnMouseLeave( object sender, EventArgs args )
		{
			IsActive = false;
		}

		private bool m_KeyPressed;
		private MouseButtons m_Button;
		private Keys m_Key;
    }
}
