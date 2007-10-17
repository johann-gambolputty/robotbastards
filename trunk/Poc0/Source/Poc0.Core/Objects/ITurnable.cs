using Rb.Core.Maths;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Interface for objects that can be turned
	/// </summary>
	public interface ITurnable : IOriented
	{
		/// <summary>
		/// Angle over time
		/// </summary>
		FloatInterpolator Turn
		{
			get;
		}

	}
}
