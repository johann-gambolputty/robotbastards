using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Interface for objects that can be moved
	/// </summary>
	/// <remarks>
	/// Moveable objects have 3 positions - current, intermediate and next.
	/// Current is 
	/// </remarks>
	public interface IMoveable : IPlaceable
	{
		/// <summary>
		/// Position over time
		/// </summary>
		Point3Interpolator Travel
		{
			get;
		}
	}
}
