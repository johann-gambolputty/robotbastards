using System;
using System.Windows.Forms;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Summary description for CommandMousePosInput.
	/// </summary>
	public class CommandMouseMoveInput : CommandInput
	{
		/// <summary>
		/// Default constructor. Bindings to this input will trigger when the mouse moves with no buttons pressed
		/// </summary>
		public CommandMouseMoveInput( )
		{
			m_Button = MouseButtons.None;
		}

		/// <summary>
		/// Setup constructor. Bindings to this input will trigger when the mouse moves with a particular button pressed
		/// </summary>
		/// <param name="button">Button to check for along with movement</param>
		public CommandMouseMoveInput( MouseButtons button )
		{
			m_Button = button;
		}

		/// <summary>
		/// Creates a CommandMouseMoveInputBinding associated with the specified view
		/// </summary>
		public override CommandInputBinding BindToView( Scene.SceneView view )
		{
			return new CommandMouseMoveInputBinding( view, m_Button );
		}

		private MouseButtons m_Button;
	}
}
