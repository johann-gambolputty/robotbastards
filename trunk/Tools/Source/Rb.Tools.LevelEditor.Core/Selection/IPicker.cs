namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Interface for generating pick information
	/// </summary>
	/// <remarks>
	/// Usually implemented by a derived camera class and accessed from a control's <see cref="Rb.Rendering.Viewer"/>.
	/// </remarks>
	public interface IPicker
	{
		/// <summary>
		/// Creates cursor pick information
		/// </summary>
		/// <param name="cursorX">Cursor X position</param>
		/// <param name="cursorY">Cursor Y position</param>
		/// <returns>Returns pick information</returns>
		PickInfoCursor CreateCursorPickInfo( int cursorX, int cursorY );

		/// <summary>
		/// Creates a pick box
		/// </summary>
		/// <param name="topLeft">Box top left corner</param>
		/// <param name="bottomRight">Box bottom right corner</param>
		/// <returns>Returns the created pick box</returns>
		IPickInfo CreatePickBox( PickInfoCursor topLeft, PickInfoCursor bottomRight );
	}
}
