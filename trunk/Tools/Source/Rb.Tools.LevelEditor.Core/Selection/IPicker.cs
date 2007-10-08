using Rb.Core.Maths;
using Rb.World;

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
		/// Creates a pick ray, and returns the first intersection in the scene
		/// </summary>
		ILineIntersection FirstPick( int cursorX, int cursorY, RayCastOptions options );

		/// <summary>
		/// Gets the objects whose shapes overlap a box (frustum in 3d)
		/// </summary>
		/// <param name="left">Top left corner X coordinate</param>
		/// <param name="top">Top left corner Y coordinate</param>
		/// <param name="right">Bottom right corner X coordinate</param>
		/// <param name="bottom">Bottom right corner Y coordinate</param>
		/// <returns>Returns a list of objects in the box</returns>
		object[] GetObjectsInBox( int left, int top, int right, int bottom );

	}
}
