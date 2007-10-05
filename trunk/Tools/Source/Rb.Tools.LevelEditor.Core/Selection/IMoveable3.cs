using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Interface for 3D moveable objects
	/// </summary>
	public interface IMoveable3
	{
		/// <summary>
		/// Moves this object by the specified delta
		/// </summary>
		/// <param name="delta">Movement delta</param>
		void Move( Vector3 delta );
	}
}
