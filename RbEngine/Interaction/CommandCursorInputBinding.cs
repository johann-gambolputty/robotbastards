using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Input binding from a screen cursor (e.g. mouse pointer)
	/// </summary>
	public class CommandCursorInputBinding : CommandInputBinding
	{
		/// <summary>
		/// Sets up this binding
		/// </summary>
		public CommandCursorInputBinding( Scene.SceneView view ) :
			base( view )
		{
		}

		/// <summary>
		/// Creates a CommandCursorEventArgs object
		/// </summary>
		public override CommandEventArgs CreateEventArgs(Command cmd)
		{
			return new CommandCursorEventArgs( cmd, View, m_X, m_Y, m_LastX, m_LastY );
		}

		protected int m_X;
		protected int m_Y;
		protected int m_LastX;
		protected int m_LastY;
	}
}
