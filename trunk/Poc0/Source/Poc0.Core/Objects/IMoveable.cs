using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Interface for objects that can be moved
	/// </summary>
	public interface IMoveable : IPlaceable
	{
		/// <summary>
		/// Position and orientation over time
		/// </summary>
		Frame3Interpolator Travel
		{
			get;
		}
	}
}
