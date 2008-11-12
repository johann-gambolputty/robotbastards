using System.Collections.Generic;
using Rb.Core.Sets.Interfaces;
using Rb.Core.Sets.Interfaces.Generic;

namespace Rb.Core.Sets.Classes.Generic
{
	/// <summary>
	/// Object set implementation
	/// </summary>
	/// <typeparam name="T">Type of objects to store in the set</typeparam>
	public class ObjectSet<T> : ObjectSet, IObjectSet<T>
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public ObjectSet( ) :
			this( new ObjectSetServiceMap( ) )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public ObjectSet( IObjectSetServiceMap serviceMap ) :
			this( serviceMap, new List<T>( ) )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public ObjectSet( IObjectSetServiceMap serviceMap, IList<T> objectList ) :
			base( serviceMap, ( System.Collections.IList )objectList )
		{
			m_Objects = objectList;
		}

		#endregion

		#region IObjectSet<T> Members

		/// <summary>
		/// Event, raised when an object is added to the set
		/// </summary>
		public new event ObjectSetObjectAddedDelegate<T> ObjectAdded;

		/// <summary>
		/// Event, raised when an object is removed from the set
		/// </summary>
		public new event ObjectSetObjectRemovedDelegate<T> ObjectRemoved;

		/// <summary>
		/// Gets the read-only list of objects in the set
		/// </summary>
		public IEnumerable<T> Objects
		{
			get { return m_Objects; }
		}

		/// <summary>
		/// Adds an object to the set
		/// </summary>
		/// <param name="obj">Object to add</param>
		public void Add( T obj )
		{
			m_Objects.Add( obj );
			OnObjectAdded( obj );
		}

		/// <summary>
		/// Removes an object from the set
		/// </summary>
		/// <param name="obj">Object to remove</param>
		public void Remove( T obj )
		{
			m_Objects.Remove( obj );
			OnObjectRemoved( obj );
		}

		/// <summary>
		/// Returns true if the set contains the specified object
		/// </summary>
		/// <param name="obj">Object to find</param>
		/// <returns>Returns true if the specified object exists in the set</returns>
		public bool Contains( T obj )
		{
			return m_Objects.Contains( obj );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Called when an object is added
		/// </summary>
		protected void OnObjectAdded( T obj )
		{
			base.OnObjectAdded( obj );
			if ( ObjectAdded != null )
			{
				ObjectAdded( this, obj );
			}
		}

		/// <summary>
		/// Called when an object is removed
		/// </summary>
		protected void OnObjectRemoved( T obj )
		{
			base.OnObjectRemoved( obj );
			if ( ObjectRemoved != null )
			{
				ObjectRemoved( this, obj );
			}
		}

		#endregion

		#region Private Members

		private readonly IList<T> m_Objects;

		#endregion
	}
}
