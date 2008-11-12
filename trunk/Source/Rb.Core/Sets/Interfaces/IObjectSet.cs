using System.Collections;

namespace Rb.Core.Sets.Interfaces
{
	/// <summary>
	/// Delegate used by <see cref="IObjectSet.ObjectAdded"/> event
	/// </summary>
	public delegate void ObjectSetObjectAddedDelegate( IObjectSet set, object obj );
	
	/// <summary>
	/// Delegate used by <see cref="IObjectSet.ObjectRemoved"/> event
	/// </summary>
	public delegate void ObjectSetObjectRemovedDelegate( IObjectSet set, object obj );

	/// <summary>
	/// Object set interface
	/// </summary>
	public interface IObjectSet
	{
		/// <summary>
		/// Event is raised when an object is removed from the set
		/// </summary>
		event ObjectSetObjectAddedDelegate ObjectAdded;

		/// <summary>
		/// Event is raised when an object is added to the set
		/// </summary>
		event ObjectSetObjectRemovedDelegate ObjectRemoved;

		/// <summary>
		/// Gets the objects in the set
		/// </summary>
		IEnumerable Objects
		{
			get;
		}

		/// <summary>
		/// Adds an object to this set. Raises the ObjectAdded event
		/// </summary>
		/// <param name="obj">Object to add</param>
		void Add( object obj );

		/// <summary>
		/// Removes an object from this set. Raises the ObjectRemoved event
		/// </summary>
		/// <param name="obj">Object to remove</param>
		void Remove( object obj );

		/// <summary>
		/// Returns true if the set contains the specified object
		/// </summary>
		/// <param name="obj">Object to find</param>
		/// <returns>Returns true if the specified object exists in the set</returns>
		bool Contains( object obj );

		/// <summary>
		/// Gets this set's service map
		/// </summary>
		IObjectSetServiceMap Services
		{
			get;
		}
	}

}
