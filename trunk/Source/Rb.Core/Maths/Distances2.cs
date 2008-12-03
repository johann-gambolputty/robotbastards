
using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Contains functions for calculating distances between 2D objects
	/// </summary>
	public static class Distances2
	{
		/// <summary>
		/// Returns the distance from a point to a circle. If the point is within the
		/// circle, a negative distance is returned
		/// </summary>
		public static float DistancePointToCircle( Point2 pt, Point2 centre, float radius )
		{
			return pt.DistanceTo( centre ) - radius;
		}

		/// <summary>
		/// Returns the distance from a point to a line segment
		/// </summary>
		public static float DistancePointToLineSegment( Point2 pt, Point2 start, Point2 end )
		{
			throw new NotImplementedException( "Use LineSegment2 for now" );
		}
	}
}
