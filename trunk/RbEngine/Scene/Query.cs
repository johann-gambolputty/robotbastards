using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Abstract query class for selecting objects in the scene
	/// </summary>
	public abstract class Query
	{
		/// <summary>
		/// Queries an object for selection
		/// </summary>
		/// <param name="obj">Object to query</param>
		/// <returns>Returns true if the object passes the query</returns>
		public abstract bool	Select( Object obj );

		/// <summary>
		/// The interface that this query uses to select objects
		/// </summary>
		/// <remarks>
		/// Queries often require objects they select to implement a particular interface - for example, RaycastQuery requires
		/// that objects implement Maths.Ray3Intersector. If an ObjectSet doesn't optimise a specific query, it can at least
		/// do a partial optimisation if it can filter out objects that don't implement the expected interface. This is the strategy
		/// that TypeGraphObjectSet uses.
		/// </remarks>
		public virtual Type		RequiredInterface
		{
			get
			{
				return null;
			}
		}
	}
}
