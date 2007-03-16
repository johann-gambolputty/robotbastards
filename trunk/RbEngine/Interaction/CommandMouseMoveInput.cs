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
		/// Setup constructor
		/// </summary>
		/// <param name="button">Button to check for</param>
		public CommandMouseMoveInput( )
		{
			m_Button = MouseButtons.None;
		}

		/// <summary>
		/// Setup constructor
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
