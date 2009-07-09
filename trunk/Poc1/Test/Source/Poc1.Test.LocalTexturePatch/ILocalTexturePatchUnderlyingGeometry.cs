
using Rb.Core.Maths;

namespace Poc1.Test.LocalTexturePatch
{
	/// <summary>
	/// Interfaces for the geometry that the local patch sits on
	/// </summary>
	public interface ILocalTexturePatchUnderlyingGeometry
	{
		/// <summary>
		/// Projects a point onto the surface of this object
		/// </summary>
		/// <param name="pointAboveSurface"></param>
		Point3 ProjectOntoSurface( Point3 pointAboveSurface );
	}
}
