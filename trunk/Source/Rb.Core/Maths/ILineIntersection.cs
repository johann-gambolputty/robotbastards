namespace Rb.Core.Maths
{
	/// <summary>
	/// Interface for line intersection information
	/// </summary>
	public interface ILineIntersection
	{
		/// <summary>
		/// Gets the object that was intersected
		/// </summary>
		object IntersectedObject
		{
			get;
		}

		/// <summary>
		/// Gets the distance along the line that the intersection occurred
		/// </summary>
		/// <remarks>
		/// For rays, this is the actual distance along the ray normal from the origin (intersection point can be
		/// retrieved using, e.g. <see cref="Ray3.GetPointOnRay"/>). 
		/// For lines, this is the fraction along the line that the intersection occurred (intersection point can
		///  be retrieved using, e.g. <see cref="LineSegment3.GetPointOnLine"/>)
		/// </remarks>
		float Distance
		{
			get;
		}
	}
}
