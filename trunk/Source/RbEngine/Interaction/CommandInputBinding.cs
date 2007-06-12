using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Binds input events from a scene view to a Command object
	/// </summary>
	public abstract class CommandInputBinding
	{
		/// <summary>
		/// Scene view that this is bound to
		/// </summary>
		public Scene.SceneView	View
		{
			get
			{
				return m_View;
			}
		}

		/// <summary>
		/// True if the input associated with this binding is active
		/// </summary>
		public bool	Active
		{
			get
			{
				return m_Active;
			}
			set
			{
				m_Active = value;
			}
		}

		/// <summary>
		/// Gets the command that this input binding is associated with
		/// </summary>
		public Command Command
		{
			get
			{
				return m_Command;
			}
		}

		/// <summary>
		/// Sets the scene view and command that this input binding is associated with
		/// </summary>
		public CommandInputBinding( Command cmd, Scene.SceneView view )
		{
			m_View		= view;
			m_Command	= cmd;
		}

		/// <summary>
		/// Helper for creating a command message from this binding
		/// </summary>
		public CommandMessage CreateCommandMessage( )
		{
			return ( Command.Interpreter == null ) ? new CommandMessage( Command ) : Command.Interpreter.CreateMessage( this );
		}

		private Scene.SceneView	m_View;
		private bool			m_Active;
		private Command			m_Command;
	}
}
