
namespace Rb.Core.Maths
{
	/// <summary>
	/// 2D intersection functions
	/// </summary>
	public static class Intersections2
	{
		/// <summary>
		/// Determines the intersection between a 2d line and a plane
		/// </summary>
		/// <param name="start">Line start</param>
		/// <param name="end">Line end</param>
		/// <param name="plane">Plane</param>
		/// <returns>Returns intersection details, or null if there was no intersection.</returns>
		public static Line2Intersection GetLinePlaneIntersection( Point2 start, Point2 end, Plane2 plane )
		{
			Vector2 vec = end - start;
			vec.Normalise( );

			float startDot = plane.Normal.Dot( start );
			float diffDot = plane.Normal.Dot( vec );

			if ( !Utils.CloseToZero( diffDot ) )
			{
				float t = ( startDot + plane.Distance ) / -diffDot;

				if ( t >= 0 )
				{
					Line2Intersection result = new Line2Intersection( );
					result.IntersectionPosition = start + ( vec * t );
					result.IntersectionNormal = plane.Normal;
					result.Distance = t;
					return result;
				}
			}

			return null;
		}
	}
}
