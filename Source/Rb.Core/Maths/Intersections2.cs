
namespace Rb.Core.Maths
{
	/// <summary>
	/// 2D intersection functions
	/// </summary>
	public static class Intersections2
	{
		/// <summary>
		/// Determines the intersection point between two planes
		/// </summary>
		/// <param name="plane0">First plane</param>
		/// <param name="plane1">Second plane</param>
		/// <returns>Returns the intersection point, or null if there is no intersection point</returns>
		public static Point2? GetPlanePlaneIntersection( Plane2 plane0, Plane2 plane1 )
		{
			float den = plane0.Normal.X * plane1.Normal.Y - plane0.Normal.Y * plane1.Normal.X;
			if ( Utils.CloseToZero( den ) )
			{
				return null;
			}

			float x = ( plane0.Normal.Y * plane1.Distance - plane1.Normal.Y * plane0.Distance ) / den;
			float y = ( plane1.Normal.X * plane0.Distance - plane0.Normal.X * plane1.Distance ) / den;

			return new Point2( x, y );
		}

		public static Point2 RandomPoint( System.Random rand )
		{
			return new Point2( ( float )rand.NextDouble( ), ( float )rand.NextDouble( ) );
		}

		public static void TestPlanePlaneIntersection( )
		{
			System.Random rand = new System.Random( );

			for ( int i = 0; i < 100; ++i )
			{
				Point2 p0 = RandomPoint( rand );
				Point2 p1 = RandomPoint( rand );
				Point2 p2 = RandomPoint( rand );

				Plane2 plane0 = new Plane2( p0, p1 );
				Plane2 plane1 = new Plane2( p1, p2 );

				Point2? intersection = GetPlanePlaneIntersection( plane0, plane1 );

				if ( intersection != null )
				{
					System.Diagnostics.Debug.Assert( intersection.Value.DistanceTo( p1 ) < 0.001f );
				}
			}
			
		}

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
