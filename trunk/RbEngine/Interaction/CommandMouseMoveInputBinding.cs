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
		/// Client-specific binding
		/// </summary>
		private new class ClientBinding : CommandCursorInputBinding.ClientBinding
		{
			/// <summary>
			/// Setup constructor. Specifies the button that must be pressed while the mouse is being moved, for the command to fire
			/// </summary>
			/// <param name="button">Button to press</param>
			public ClientBinding( Network.Client client, MouseButtons button ) :
					base( client )
			{
				m_Button = button;
				
				//	TODO: Missing client now...
			//	client.Control.MouseMove += new MouseEventHandler( OnMouseMove );
			}

			/// <summary>
			/// Triggers the command, if the appropriate mouse button is pressed
			/// </summary>
			private void				OnMouseMove( object sender, MouseEventArgs args )
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
			private MouseButtons	m_Button;
		}

		/// <summary>
		/// Setup constructor. Responds to mouse movement, with no buttons pressed
		/// </summary>
		public CommandMouseMoveInputBinding( )
		{
			m_Button = MouseButtons.None;
		}

		/// <summary>
		/// Setup constructor. Specifies the button that must be pressed while the mouse is being moved, for the command to fire
		/// </summary>
		/// <param name="button">Button to press</param>
		public CommandMouseMoveInputBinding( MouseButtons button )
		{
			m_Button = button;
		}

		/// <summary>
		/// Binds to the specified client
		/// </summary>
		/// <param name="client">Client to bind to</param>
		public override CommandInputBinding.ClientBinding	BindToClient( Network.Client client )
		{
			return new ClientBinding( client, m_Button );
		}


		private MouseButtons	m_Button;
	}
}
