
namespace Rb.Core.Maths
{
	/// <summary>
	/// Summary description for Intersection.
	/// </summary>
	public static class Intersections3
	{
		#region Ray-Quad intersections

		/// <summary>
		/// Intersects a ray with a quad
		/// </summary>
		/// <param name="ray">Ray </param>
		/// <param name="pt0">Quad corner position</param>
		/// <param name="pt1">Quad corner position</param>
		/// <param name="pt2">Quad corner position</param>
		/// <param name="pt3">Quad corner position</param>
		/// <returns>Returns intersection details, or null if there was no intersection</returns>
		public static Line3Intersection GetRayQuadIntersection( Ray3 ray, Point3 pt0, Point3 pt1, Point3 pt2, Point3 pt3 )
		{
			//	TODO: AP: This is very lazy...
			Line3Intersection intersection = GetRayTriangleIntersection( ray, pt0, pt1, pt2 );
			if ( intersection == null )
			{
				intersection = GetRayTriangleIntersection( ray, pt2, pt3, pt1 );
			}
			return intersection;
		}

		#endregion

		#region Ray-Tri intersections

		/// <summary>
		/// Intersects a ray with a triangle
		/// </summary>
		public static Line3Intersection GetRayTriangleIntersection( Ray3 ray, Point3 pt0, Point3 pt1, Point3 pt2 )
		{
			return GetRayTriangleIntersection( ray, pt0, pt1, pt2, float.MaxValue );
		}


		/// <summary>
		/// Intersects a ray with a triangle. If the intersection point is beyond a given distance from the ray origin, the intersection is rejected
		/// </summary>
		public static Line3Intersection GetRayTriangleIntersection( Ray3 ray, Point3 pt0, Point3 pt1, Point3 pt2, float maxDistance )
		{
			Vector3 uVec			= ( pt1 - pt0 );
			Vector3 vVec			= ( pt2 - pt0 );
			Vector3 nVec			= Vector3.Cross( uVec, vVec );
			float	nVecSqrLength	= nVec.SqrLength;
			if ( nVecSqrLength < 0.001f )
			{
				//	Degenerate tri - no intersection
				return null;
			}

			Vector3 w0	= ray.Origin - pt0;
			float	a	= -nVec.Dot( w0 );
			float	b	= nVec.Dot( ray.Direction );
			if ( System.Math.Abs( b ) < 0.001f )
			{
				//	Ray is parallel to the tri - reject
				return null;
			}
			float	r	= a / b;
			if ( r < 0 )
			{
				//	Ray goes away from the tri - reject
				return null;
			}

			//	Ray intersects tri plane - calculate position in tri
			Point3	pt	= ray.Origin + ray.Direction * r;

			float	uu	= uVec.Dot( uVec );
			float	uv	= uVec.Dot( vVec );
			float	vv	= vVec.Dot( vVec );
			Vector3	w	= pt - pt0;
			float	wu	= w.Dot( uVec );
			float	wv	= w.Dot( vVec );

			float	d	= ( uv * uv - uu * vv );
			float	s	= ( uv * wv - vv * wu ) / d;
			if ( s < 0 || s > 1 )
			{
				return null;
			}
			float	t	= ( uv * wu - uu * wv ) / d;
			if ( t < 0 || t > 1 )
			{
				return null;
			}

			nVec /= ( float )System.Math.Sqrt( nVecSqrLength );
			return new Line3Intersection( pt, nVec, r );
		}

		#endregion

		#region Ray-Sphere intersections

		/// <summary>
		/// Tests for an intersection between a ray and a sphere
		/// </summary>
		public static Line3Intersection GetRayIntersection( Ray3 ray, Sphere3 sphere )
		{
			Vector3	originToCentre = ray.Origin - sphere.Centre;

			float	a0	= originToCentre.SqrLength - ( sphere.SqrRadius );
			float	a1	= ray.Direction.Dot( originToCentre );
			float	discriminant;
			Point3	intersectionPt;
			
			if ( a0 <= 0 )
			{
				//	1 intersection: The origin of the ray is inside the sphere
				discriminant = ( a1 * a1 ) - a0;
				float t = -a1 + ( float )System.Math.Sqrt( discriminant );

				intersectionPt = ray.GetPointOnRay( t );
				return new Line3Intersection(  intersectionPt, ( intersectionPt - sphere.Centre ).MakeNormal( ), t );
			}
			if ( a1 >= 0 )
			{
				//	No intersections: Ray origin is outside the sphere, ray direction points away from the sphere
				return null;
			}

			discriminant = ( a1 * a1 ) - a0;
			if ( discriminant < 0 )
			{
				//	No intersections: Ray/sphere equation has no roots
				return null;
			}

			if ( discriminant < 0.0001f )	//	TODO: Magic number
			{
				//	1 intersection: Discriminant is close to zero - there's only root to the ray/sphere equation
				float t = -a1;
				intersectionPt = ray.GetPointOnRay( t );
				return new Line3Intersection(  intersectionPt, ( intersectionPt - sphere.Centre ).MakeNormal( ), t );
			}

			//	2 intersections: 2 roots to the ray/sphere equation. Choose the closest
			float root = ( float )System.Math.Sqrt( discriminant );
			float t0 = -a1 - root;
			float t1 = -a1 + root;

			float closestT = ( t0 < t1 ) ? t0 : t1;

			intersectionPt = ray.GetPointOnRay( closestT );
			return new Line3Intersection(  intersectionPt, ( intersectionPt - sphere.Centre ).MakeNormal( ), closestT );
		}

		#endregion

		#region Ray-Plane intersections

		/// <summary>
		/// Tests for an intersection between a ray and a plane
		/// </summary>
		public static bool TestRayIntersection( Ray3 ray, Plane3 plane )
		{
			float	startDot	= plane.Normal.Dot( ray.Origin );
			float	diffDot		= plane.Normal.Dot( ray.Direction );

			if ( !Utils.CloseToZero( diffDot ) )
			{
				float t = ( startDot + plane.Distance ) / -diffDot;
				return ( t >= 0 );
			}

			return false;
		}

		/// <summary>
		/// Returns information about an intersection between a ray and a plane
		/// </summary>
		public static Line3Intersection GetRayIntersection( Ray3 ray, Plane3 plane )
		{
			float	startDot	= plane.Normal.Dot( ray.Origin );
			float	diffDot		= plane.Normal.Dot( ray.Direction );

			if ( !Utils.CloseToZero( diffDot ) )
			{
				float t = ( startDot + plane.Distance ) / -diffDot;

				if ( t >= 0 )
				{
					Line3Intersection result = new Line3Intersection( );
					result.IntersectedObject	= plane;
					result.IntersectionPosition = ray.Origin + ( ray.Direction * t );
					result.IntersectionNormal	= plane.Normal;
					result.Distance				= t;
					return result;
				}
			}

			return null;
		}

		#endregion
	}
}
