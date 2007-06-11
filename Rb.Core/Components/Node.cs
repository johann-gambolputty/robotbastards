using System;
using System.Collections;

namespace Rb.Core.Components
{
	/// <summary>
	/// Handy base class that implements IChild and IParent
	/// </summary>
	public class Node : IChild, IParent
	{
		#region Public properties

		/// <summary>
		/// Gets the parent object, set by the implementation of IChild.AddedToParent()
		/// </summary>
		public object Parent
		{
			get { return m_Parent;  }
		}

		#endregion

		#region IParent Members

		/// <summary>
		/// Returns the child list
		/// </summary>
		public ICollection Children
		{
			get { return m_Children; }
		}

		/// <summary>
		/// Adds a child object
		/// </summary>
		/// <param name="obj">Child object to add</param>
		public virtual void AddChild( object obj )
		{
			m_Children.Add( obj );
			if ( OnChildAdded != null )
			{
				OnChildAdded( this, obj );
			}
		}

		/// <summary>
		/// Removes a child object
		/// </summary>
		/// <param name="obj">Child object to remove</param>
		public virtual void RemoveChild( object obj )
		{
			m_Children.Remove( obj );
			if ( OnChildRemoved != null )
			{
				OnChildRemoved( this, obj );
			}
		}

		/// <summary>
		/// Called when a child is added to this object
		/// </summary>
		public event OnChildAddedDelegate OnChildAdded;

		/// <summary>
		/// Called when a child is removed from this object
		/// </summary>
		public event OnChildRemovedDelegate OnChildRemoved;

		#endregion


		#region IChild Members

		/// <summary>
		/// Called when this object is added to a parent object
		/// </summary>
		/// <param name="parent">Parent object</param>
		public virtual void AddedToParent( object parent )
		{
			m_Parent = parent;
		}

		#endregion
		
		#region Private stuff

		private ArrayList	m_Children = new ArrayList( );
		private object		m_Parent;
		
		#endregion
	}
}