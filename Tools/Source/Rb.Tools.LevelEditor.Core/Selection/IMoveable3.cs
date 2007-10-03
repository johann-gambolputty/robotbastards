using System;
using Rb.Core.Maths;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Interface for 3D moveable objects
	/// </summary>
	public interface IMoveable3
	{
		/// <summary>
		/// Event, raised by <see cref="Move"/>
		/// </summary>
		event EventHandler Moved;

		/// <summary>
		/// Moves this object by the specified delta
		/// </summary>
		/// <param name="delta">Movement delta</param>
		void Move( Vector3 delta );
	}
}
