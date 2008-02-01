using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Interface for 3D moveable objects
	/// </summary>
	/// <remarks>
	/// Used by the <see cref="Actions.MoveAction"/> action.
	/// </remarks>
	public interface IMoveable3
	{
		/// <summary>
		/// Moves this object by the specified delta
		/// </summary>
		/// <param name="delta">Movement delta</param>
		/// <param name="inputPos">Input position that defines the delta over time</param>
		void Move( Vector3 delta, Point3 inputPos );
	}
}
