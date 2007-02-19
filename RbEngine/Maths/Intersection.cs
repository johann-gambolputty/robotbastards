using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Summary description for Intersection.
	/// </summary>
	public class Intersection
	{
		/// <summary>
		/// Tests for an intersection between a ray and a sphere
		/// </summary>
		public static Ray3Intersection GetIntersection( Ray3 ray, Sphere3 sphere )
		{
			Vector3	originToCentre			= ray.Origin - sphere.Centre;

			float	a0	= originToCentre.SqrLength - ( sphere.SqrRadius );
			float	a1	= ray.Direction.Dot( originToCentre );
			float	discriminant;
			Vector3 intersectionPt;
			
			if ( a0 <= 0 )
			{
				//	1 intersection: The origin of the ray is inside the sphere
				discriminant = ( a1 * a1 ) - a0;
				float t = -a1 + ( float )System.Math.Sqrt( discriminant );

				intersectionPt = ray.GetPointOnRay( t );
				return new Ray3Intersection(  intersectionPt, ( intersectionPt - sphere.Centre ).MakeNormal( ), t );
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
				return new Ray3Intersection(  intersectionPt, ( intersectionPt - sphere.Centre ).MakeNormal( ), t );
			}

			//	2 intersections: 2 roots to the ray/sphere equation. Choose the closest
			float root = ( float )System.Math.Sqrt( discriminant );
			float t0 = -a1 - root;
			float t1 = -a1 + root;

			float closestT = ( t0 < t1 ) ? t0 : t1;

			intersectionPt = ray.GetPointOnRay( closestT );
			return new Ray3Intersection(  intersectionPt, ( intersectionPt - sphere.Centre ).MakeNormal( ), closestT );
		}

		/// <summary>
		/// Tests for an intersection between a ray and a plane
		/// </summary>
		public static bool TestIntersection( Ray3 ray, Plane3 plane )
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
		public static Ray3Intersection GetIntersection( Ray3 ray, Plane3 plane )
		{
			float	startDot	= plane.Normal.Dot( ray.Origin );
			float	diffDot		= plane.Normal.Dot( ray.Direction );

			if ( !Utils.CloseToZero( diffDot ) )
			{
				float t = ( startDot + plane.Distance ) / -diffDot;

				if ( t >= 0 )
				{
					Ray3Intersection result = new Ray3Intersection( );
					result.IntersectionPosition = ray.Origin + ( ray.Direction * t );
					result.IntersectionNormal	= plane.Normal;
					result.Distance				= t;
					return result;
				}
			}

			return null;
		}
	}
}
