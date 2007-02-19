using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Summary description for ClosestRay3IntersectionQuery.
	/// </summary>
	public class ClosestRay3IntersectionQuery : SpatialQuery
	{
		/// <summary>
		/// Returns the type for Maths.IRay3Intersector
		/// </summary>
		public override Type		RequiredInterface
		{
			get
			{
				return typeof( Maths.IRay3Intersector );
			}
		}

		/// <summary>
		/// Access to the query ray
		/// </summary>
		public Maths.Ray3	Ray
		{
			get
			{
				return m_Ray;
			}
		}

		/// <summary>
		/// Information about the closest ray intersection
		/// </summary>
		public Maths.Ray3Intersection	ClosestIntersection
		{
			get
			{
				return m_Intersection;
			}
		}

		/// <summary>
		/// Sets the query ray
		/// </summary>
		/// <param name="ray">Query ray</param>
		public ClosestRay3IntersectionQuery( Maths.Ray3 ray )
		{
			m_Ray = ray;
		}

		/// <summary>
		/// Checks if a specified object is intersected by the stored ray
		/// </summary>
		/// <param name="obj">Object to test</param>
		/// <returns>true if the object is intersected by the ray passed to the query constructor</returns>
		public override bool Select( object obj )
		{
			Maths.Ray3Intersection intersection = ( ( Maths.IRay3Intersector )obj ).GetIntersection( m_Ray );
			if ( ( intersection != null ) && ( intersection.Distance < m_ClosestIntersection ) )
			{
				m_ClosestIntersection	= intersection.Distance;
				m_Intersection			= intersection;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Helper for finding raycast intersections in a scene
		/// </summary>
		/// <param name="objects">Object set</param>
		/// <returns>Details about the first intersection, or null if there were no intersections</returns>
		public static Maths.Ray3Intersection	Get( Maths.Ray3 ray, ObjectSet objects )
		{
			ClosestRay3IntersectionQuery query = new ClosestRay3IntersectionQuery( ray );
			objects.Select( query );
			return query.ClosestIntersection;
		}

		private Maths.Ray3				m_Ray;
		private Maths.Ray3Intersection	m_Intersection;
		private float					m_ClosestIntersection = float.MaxValue;
	}
}
