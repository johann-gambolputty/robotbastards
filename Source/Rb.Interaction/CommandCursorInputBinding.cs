using System;

namespace Rb.Core.Interaction
{
	/// <summary>
	/// Input binding from a screen cursor (e.g. mouse pointer)
	/// </summary>
	public class CommandCursorInputBinding : CommandInputBinding
	{
		/// <summary>
		/// Sets up this binding
		/// </summary>
		public CommandCursorInputBinding( Command cmd, Scene.SceneView view ) :
			base( cmd, view )
		{
		}

		/// <summary>
		/// Gets the X coordinate of the cursor
		/// </summary>
		public int X
		{
			get
			{
				return m_X;
			}
		}

		/// <summary>
		/// Gets the Y coordinate of the cursor
		/// </summary>
		public int Y
		{
			get
			{
				return m_Y;
			}
		}

		/// <summary>
		/// Gets the last X coordinate of the cursor
		/// </summary>
		public int LastX
		{
			get
			{
				return m_LastX;
			}
		}

		/// <summary>
		/// Gets the last Y coordinate of the cursor
		/// </summary>
		public int LastY
		{
			get
			{
				return m_LastY;
			}
		}

		/// <summary>
		/// X position of the cursor
		/// </summary>
		protected int m_X;

		/// <summary>
		/// Y position of the cursor
		/// </summary>
		protected int m_Y;

		/// <summary>
		/// Last X position of the cursor
		/// </summary>
		protected int m_LastX;

		/// <summary>
		/// Last Y position of the cursor
		/// </summary>
		protected int m_LastY;
	}
}
