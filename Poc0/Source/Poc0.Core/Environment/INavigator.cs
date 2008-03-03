using Poc0.Core.Objects;
using Rb.Core.Maths;

namespace Poc0.Core.Environment
{
	/// <summary>
	/// AI path planner
	/// </summary>
	interface INavigator
	{
		/// <summary>
		/// Creates a path between two points
		/// </summary>
		IPath CreatePath( Point3 start, Point3 end );
	}
}
