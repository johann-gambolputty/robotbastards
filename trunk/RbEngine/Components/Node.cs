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
			if ( m_Children == null )
			{
				m_Children = new ArrayList( );
			}
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
			int childIndex = m_Children == null ? -1 : m_Children.IndexOf( childObject );
			if ( childIndex != -1 )
			{
				m_Children.RemoveAt( childIndex );
				if ( ChildRemoved != null )
				{
					ChildRemoved( this, childObject );
				}
			}
		}

		/// <summary>
		/// Calls visitor for each child node
		/// </summary>
		public void							VisitChildren( ChildVisitorDelegate visitor )
		{
			if ( m_Children != null )
			{
				for ( int childIndex = 0; childIndex < m_Children.Count; ++childIndex )
				{
					visitor( m_Children[ childIndex ] );
				}
			}
		}

		#endregion

		#region IChildObject Members

		/// <summary>
		/// Called when this object is added to a parent object
		/// </summary>
		public void AddedToParent( Object parentObject )
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
			if ( m_Children != null )
			{
				for ( int childIndex = 0; childIndex < m_Children.Count; ++childIndex )
				{
					if ( Convert.ChangeType( m_Children[ childIndex ], type ) != null )
				//	or
				//	if ( Reflection.Binder.ChangeType( .. ) )
					{
						return m_Children[ childIndex ];
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Finds the first child object that subclasses the specified type
		/// </summary>
		public Object	FindChild( string str, bool caseSensitive )
		{
			if ( m_Children != null )
			{
				for ( int childIndex = 0; childIndex < m_Children.Count; ++childIndex )
				{
					INamedObject namedChild = m_Children[ childIndex ] as INamedObject;
					if ( ( namedChild != null ) && ( string.Compare( namedChild.Name, str, caseSensitive ) == 0 ) )
					{
						return m_Children[ childIndex ];
					}
				}
			}

			return null;
		}

		#endregion

		#region	Protected stuff

		protected Object	m_Parent;
		protected ArrayList	m_Children;

		#endregion
	}
}
