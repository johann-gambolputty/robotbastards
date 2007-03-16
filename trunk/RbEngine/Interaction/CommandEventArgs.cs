using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Event arguments passed to command event subscribers
	/// </summary>
	public class CommandEventArgs
	{
		/// <summary>
		/// Stores the command that was fired
		/// </summary>
		public CommandEventArgs( Command cmd, Scene.SceneView view )
		{
			m_Command	= cmd;
			m_View		= View;
		}

		/// <summary>
		/// ID of the stored command
		/// </summary>
		public int	CommandId
		{
			get
			{
				return m_Command.Id;
			}
		}

		/// <summary>
		/// The view that triggered the command
		/// </summary>
		public Scene.SceneView	View
		{
			get
			{
				return m_View;
			}
		}

		private Command			m_Command;
		private Scene.SceneView	m_View;
	}
}
