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
		/// Sets the scene view that this input binding is associated with
		/// </summary>
		/// <param name="view"></param>
		public CommandInputBinding( Command cmd, Scene.SceneView view )
		{
			m_View		= view;
			m_Command	= cmd;
		}

		private Scene.SceneView	m_View;
		private bool			m_Active;
		private Command			m_Command;
	}
}
