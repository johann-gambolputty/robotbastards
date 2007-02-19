using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Checks if an object is intersected by a ray (uses Maths.IRay3Intersector.Intersects())
	/// </summary>
	/// <remarks>
	/// This query should be used with care - if run on a large object set without any form of optimisation for this query, it could
	/// take a long time.
	/// </remarks>
	public class RaycastQuery : SpatialQuery
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
		/// Information about the ray intersection
		/// </summary>
		/// <remarks>
		/// Only set if storeIntersectionInfo is set to true in the RaycastQuery constructor
		/// </remarks>
		public Maths.Ray3Intersection	Intersection
		{
			get
			{
				return m_Intersection;
			}
		}

		/// <summary>
		/// Sets the ray used by the query
		/// </summary>
		/// <param name="ray">Query ray</param>
		public RaycastQuery( Maths.Ray3 ray, bool storeIntersectionInfo )
		{
			m_Ray = ray;
			m_StoreIntersectionInfo = storeIntersectionInfo;
		}

		/// <summary>
		/// Checks if a specified object is intersected by the stored ray
		/// </summary>
		/// <param name="obj">Object to test</param>
		/// <returns>true if the object is intersected by the ray passed to the query constructor</returns>
		public override bool Select( object obj )
		{
			if ( m_StoreIntersectionInfo )
			{
				m_Intersection = ( ( Maths.IRay3Intersector )obj ).GetIntersection( m_Ray );
				return ( m_Intersection != null );
			}
			return ( ( Maths.IRay3Intersector )obj ).TestIntersection( m_Ray );
		}

		/// <summary>
		/// Helper for finding raycast intersections in a scene
		/// </summary>
		/// <param name="objects">Object set</param>
		/// <returns>Details about the first intersection, or null if there were no intersections</returns>
		public static Maths.Ray3Intersection	GetFirstIntersection( Maths.Ray3 ray, ObjectSet objects )
		{
			RaycastQuery query = new RaycastQuery( ray, true );
			objects.SelectFirst( query );
			return query.Intersection;
		}

		private Maths.Ray3				m_Ray;
		private Maths.Ray3Intersection	m_Intersection;
		private bool					m_StoreIntersectionInfo = false;
	}
}
