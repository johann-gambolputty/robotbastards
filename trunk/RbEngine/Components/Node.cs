using System;
using System.Collections;

namespace RbEngine.Components
{
	/// <summary>
	/// Simple implementation of IParentObject and IChildObject
	/// </summary>
	public class Node : IParentObject, IChildObject
	{
		#region IParentObject Members

		/// <summary>
		/// Event, invoked by AddChild() after a child object has been added
		/// </summary>
		public event ChildAddedDelegate		ChildAdded;


		/// <summary>
		/// Event, invoked by AddChild() before a child object has been removed
		/// </summary>
		public event ChildRemovedDelegate	ChildRemoved;

		/// <summary>
		/// Gets the child collection
		/// </summary>
		public ICollection					Children
		{
			get
			{
				return m_Children;
			}
		}

		/// <summary>
		/// Adds a child to the child object list
		/// </summary>
		/// <param name="childObject">Child object</param>
		/// <remarks>
		/// Calls ChildAdded event
		/// </remarks>
		public virtual void					AddChild( Object childObject )
		{
			m_Children.Add( childObject );

			if ( childObject is IChildObject )
			{
				( ( IChildObject )childObject ).AddedToParent( this );
			}

			if ( ChildAdded != null )
			{
				ChildAdded( this, childObject );
			}
		}

		/// <summary>
		/// Removes a child to the child object list
		/// </summary>
		/// <param name="childObject">Child object</param>
		/// <remarks>
		/// Calls ChildRemoved event
		/// </remarks>
		public virtual void					RemoveChild( Object childObject )
		{
			int childIndex = m_Children.IndexOf( childObject );
			if ( childIndex != -1 )
			{
				m_Children.RemoveAt( childIndex );
				if ( ChildRemoved != null )
				{
					ChildRemoved( this, childObject );
				}
			}
		}

		#endregion

		#region IChildObject Members

		/// <summary>
		/// Called when this object is added to a parent object
		/// </summary>
		public virtual void AddedToParent( Object parentObject )
		{
			m_Parent = parentObject;
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// Gets the parent object
		/// </summary>
		public Object	Parent
		{
			get
			{
				return m_Parent;
			}
		}

		#endregion

		#region	Child object search

		/// <summary>
		/// Finds the first child object that subclasses the specified type
		/// </summary>
		public Object	FindChild( Type type )
		{
			for ( int childIndex = 0; childIndex < m_Children.Count; ++childIndex )
			{
				if ( type.IsInstanceOfType( m_Children[ childIndex ] ) )
				{
					return m_Children[ childIndex ];
				}
			}

			return null;
		}

		/// <summary>
		/// Finds the first child object that implements INamedObject, and has the name specified in str
		/// </summary>
		public Object	FindChild( string str, bool caseSensitive )
		{
			for ( int childIndex = 0; childIndex < m_Children.Count; ++childIndex )
			{
				INamedObject namedChild = m_Children[ childIndex ] as INamedObject;
				if ( ( namedChild != null ) && ( string.Compare( namedChild.Name, str, caseSensitive ) == 0 ) )
				{
					return m_Children[ childIndex ];
				}
			}

			return null;
		}

		#endregion

		#region	Protected stuff

		/// <summary>
		/// This node's parent object
		/// </summary>
		protected Object	m_Parent;

		/// <summary>
		/// This node's child objects
		/// </summary>
		protected ArrayList	m_Children = new ArrayList( );

		#endregion
	}
}