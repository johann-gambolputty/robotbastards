
using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Base class for cursor pick information
	/// </summary>
	public abstract class PickInfoCursor : IPickInfo
	{
		/// <summary>
		/// Picker constructor
		/// </summary>
		/// <param name="cursorX">Cursor X position</param>
		/// <param name="cursorY">Cursor Y position</param>
		public PickInfoCursor( int cursorX, int cursorY )
		{
			m_CursorX = cursorX;
			m_CursorY = cursorY;
		}

		/// <summary>
		/// Gets the X position of the cursor
		/// </summary>
		public int CursorX
		{
			get { return m_CursorX; }
		}

		/// <summary>
		/// Gets the Y position of the cursor
		/// </summary>
		public int CursorY
		{
			get { return m_CursorY; }
		}

		private readonly int m_CursorX;
		private readonly int m_CursorY;
	}
}
