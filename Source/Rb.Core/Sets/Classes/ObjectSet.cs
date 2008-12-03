using System.Collections;
using Rb.Core.Sets.Interfaces;
using Rb.Core.Utils;

namespace Rb.Core.Sets.Classes
{
	/// <summary>
	/// Object set implementation
	/// </summary>
	public class ObjectSet : IObjectSet
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
			this( serviceMap, new ArrayList( ) )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public ObjectSet( IObjectSetServiceMap serviceMap, IList objectList )
		{
			m_ServiceMap = serviceMap;
			m_Objects = objectList;
		}

		#endregion

		#region IObjectSet Members

		/// <summary>
		/// Event, raised when an object is added to the set
		/// </summary>
		public event ObjectSetObjectAddedDelegate ObjectAdded;

		/// <summary>
		/// Event, raised when an object is removed from the set
		/// </summary>
		public event ObjectSetObjectRemovedDelegate ObjectRemoved;

		/// <summary>
		/// Gets the objects in the set
		/// </summary>
		IEnumerable IObjectSet.Objects
		{
			get { return m_Objects; }
		}

		/// <summary>
		/// Adds an object to the set
		/// </summary>
		/// <param name="obj">Object to add</param>
		/// <exception cref="System.ArgumentNullException">Thrown if obj is null</exception>
		public void Add( object obj )
		{
			Arguments.CheckNotNull( obj, "obj" );
			m_Objects.Add( obj );
			OnObjectAdded( obj );
		}

		/// <summary>
		/// Removes an object from the set
		/// </summary>
		/// <param name="obj">Object to remove</param>
		public void Remove( object obj )
		{
			Arguments.CheckNotNull( obj, "obj" );
			m_Objects.Remove( obj );
			OnObjectRemoved( obj );
		}

		/// <summary>
		/// Returns true if the set contains the specified object
		/// </summary>
		/// <param name="obj">Object to find</param>
		/// <returns>Returns true if the specified object exists in the set</returns>
		public bool Contains( object obj )
		{
			return m_Objects.Contains( obj );
		}

		/// <summary>
		/// Gets the services associated with this set
		/// </summary>
		public IObjectSetServiceMap Services
		{
			get { return m_ServiceMap; }
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Raises the ObjectAdded event
		/// </summary>
		protected void OnObjectAdded( object obj )
		{
			if ( ObjectAdded != null )
			{
				ObjectAdded( this, obj );
			}
		}

		/// <summary>
		/// Raises the ObjectRemoved event
		/// </summary>
		protected void OnObjectRemoved( object obj )
		{
			if ( ObjectRemoved != null )
			{
				ObjectRemoved( this, obj );
			}
		}

		#endregion

		#region Private Members

		private readonly IList m_Objects;
		private readonly IObjectSetServiceMap m_ServiceMap;

		#endregion
	}

}
