using System;
using System.Collections;

namespace RbEngine.Components
{

	/// <summary>
	/// Delegate, used by IParentObject.VisitChildren()
	/// </summary>
	public delegate bool ChildVisitorDelegate( Object child );

	/// <summary>
	/// Delegate for the IParentObject.ChildAdded event
	/// </summary>
	public delegate void ChildAddedDelegate( Object parent, Object child );

	/// <summary>
	/// Delegate for the IParentObject.ChildAdded event
	/// </summary>
	public delegate void ChildRemovedDelegate( Object parent, Object child );


	/// <summary>
	/// Interface for objects that can store child objects
	/// </summary>
	public interface IParentObject
	{
		/// <summary>
		/// Event, invoked by AddChild() after a child object has been added
		/// </summary>
		event ChildAddedDelegate	ChildAdded;

		/// <summary>
		/// Event, invoked by AddChild() before a child object has been removed
		/// </summary>
		event ChildRemovedDelegate	ChildRemoved;

		/// <summary>
		/// Gets the child collection
		/// </summary>
		ICollection					Children
		{
			get;
		}

		/// <summary>
		/// Adds a child object to this object
		/// </summary>
		/// <param name="childObject">Child object to add</param>
		/// <remarks>
		/// Implementations must call ChildAdded
		/// </remarks>
        void						AddChild( Object childObject );

		/// <summary>
		/// Removes a specific child object
		/// </summary>
		/// <param name="childObject">Child object to remove</param>
		/// <remarks>
		/// Implementations must call ChildRemoved
		/// </remarks>
		void						RemoveChild( Object childObject );

		/// <summary>
		/// Visits all children, calling visitor() for each
		/// </summary>
		/// <param name="visitor">Visitor function</param>
		void						VisitChildren( ChildVisitorDelegate visitor );
	}
}
