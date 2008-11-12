using System.Collections.Generic;

namespace Rb.Core.Sets.Interfaces.Generic
{
	/// <summary>
	/// Delegate used by <see cref="IObjectSet{T}.ObjectAdded"/> event
	/// </summary>
	public delegate void ObjectSetObjectAddedDelegate<T>( IObjectSet<T> set, T obj );

	/// <summary>
	/// Delegate used by <see cref="IObjectSet{T}.ObjectRemoved"/> event
	/// </summary>
	public delegate void ObjectSetObjectRemovedDelegate<T>( IObjectSet<T> set, T obj );

	/// <summary>
	/// Strongly typed object set interface
	/// </summary>
	/// <typeparam name="T">Object type stored in this set</typeparam>
	public interface IObjectSet<T> : IObjectSet
	{
		/// <summary>
		/// Event is raised when an object is removed from the set
		/// </summary>
		new event ObjectSetObjectAddedDelegate<T> ObjectAdded;

		/// <summary>
		/// Event is raised when an object is added to the set
		/// </summary>
		new event ObjectSetObjectRemovedDelegate<T> ObjectRemoved;

		/// <summary>
		/// Gets the objects in the set
		/// </summary>
		new IEnumerable<T> Objects
		{
			get;
		}

		/// <summary>
		/// Adds an object to this set. Raises the ObjectAdded event
		/// </summary>
		/// <param name="obj">Object to add</param>
		void Add( T obj );

		/// <summary>
		/// Removes an object from this set. Raises the ObjectRemoved event
		/// </summary>
		/// <param name="obj">Object to remove</param>
		void Remove( T obj );

		/// <summary>
		/// Returns true if the set contains the specified object
		/// </summary>
		/// <param name="obj">Object to find</param>
		/// <returns>Returns true if the specified object exists in the set</returns>
		bool Contains( T obj );
	}
}
