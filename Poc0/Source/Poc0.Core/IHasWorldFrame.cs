
using Rb.Core.Maths;

namespace Poc0.Core
{
	/// <summary>
	/// For objects that have a world position and orientation
	/// </summary>
	public interface IHasWorldFrame
	{
		/// <summary>
		/// Gets the world frame for this object
		/// </summary>
		Matrix44 WorldFrame
		{
			get;
		}
	}
}
