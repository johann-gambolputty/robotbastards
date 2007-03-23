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
		}
		/// <summary>
		/// Sets the scene view that this input binding is associated with
		/// </summary>
		/// <param name="view"></param>
		public CommandInputBinding( Scene.SceneView view )
		{
			m_View = view;
		}

		/// <summary>
		/// Creates a CommandEventArgs object for this binding
		/// </summary>
		public virtual CommandEventArgs CreateEventArgs( Command cmd )
		{
			return new CommandEventArgs( cmd, View );
		}

		private Scene.SceneView	m_View;

		/// <summary>
		/// Active binding flag
		/// </summary>
		protected bool			m_Active = false;
	}
}
