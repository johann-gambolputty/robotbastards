using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Interface for objects that can be intersected by a ray
	/// </summary>
	/// <seealso cref="Ray3">Ray3</seealso>
	public interface IRay3Intersector
	{
		/// <summary>
		/// Checks if a ray intersects this object
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>true if the ray intersects this object</returns>
		bool			TestIntersection( Ray3 ray );

		/// <summary>
		/// Checks if a ray intersects this object, returning information about the intersection if it does
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>Intersection information. If no intersection takes place, this method returns null</returns>
		Ray3Intersection	GetIntersection( Ray3 ray );
	}
}
