using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Summary description for CommandCursorEventArgs.
	/// </summary>
	public class CommandCursorEventArgs
	{
		/// <summary>
		/// X position of the cursor
		/// </summary>
		public int		X
		{
			get
			{
				return m_X;
			}
		}

		/// <summary>
		/// Y position of the cursor
		/// </summary>
		public int		Y
		{
			get
			{
				return m_Y;
			}
		}

		/// <summary>
		/// X movement of the cursor since the last update
		/// </summary>
		public int		DeltaX
		{
			get
			{
				return m_X - m_LastX;
			}
		}

		/// <summary>
		/// Y movement of the cursor since the last update
		/// </summary>
		public int		DeltaY
		{
			get
			{
				return m_Y - m_LastY;
			}
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="client">Bound client</param>
		public CommandCursorEventArgs( Command cmd, Scene.SceneView view, int x, int y, int lastX, int lastY ) :
			base( cmd, view )
		{
			m_X 	= x;
			m_Y 	= y;
			m_LastX = lastX;
			m_LastY = lastY;
		}

		private int	m_X;
		private int	m_Y;
		private int	m_LastX;
		private int	m_LastY;
	}
}
