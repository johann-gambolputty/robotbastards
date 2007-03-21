using System;
using System.Collections;

namespace RbEngine.Network
{
	/// <summary>
	/// A set of client-to-server and server-to-client connections
	/// </summary>
	public class Connections : Components.IParentObject
	{
		#region	Connections

		/// <summary>
		/// Gets a named client to server connection
		/// </summary>
		public IConnection	GetConnection( string connectionName )
		{
			//	Run through the connection list
			foreach ( IConnection curConnection in m_Connections )
			{
				if ( string.Compare( curConnection.Name, connectionName, true ) == 0 )
				{
					return curConnection;
				}
			}
			return null;
		}

		#endregion

		#region	Private stuff

		private ArrayList m_Connections = new ArrayList( );

		#endregion

		#region IParentObject Members

		/// <summary>
		/// Called when a connection is added to this object
		/// </summary>
		public event Components.ChildAddedDelegate ChildAdded;

		/// <summary>
		/// Called when a connection is removed from this object
		/// </summary>
		public event Components.ChildRemovedDelegate ChildRemoved;

		/// <summary>
		/// The connection collection
		/// </summary>
		public ICollection Children
		{
			get
			{
				return m_Connections;
			}
		}

		/// <summary>
		/// Adds a connection (child must implement the IConnection interface)
		/// </summary>
		/// <param name="childObject"></param>
		public void AddChild( Object childObject )
		{
			m_Connections.Add( ( IConnection )childObject );
			if ( ChildAdded != null )
			{
				ChildAdded( this, childObject );
			}
		}

		/// <summary>
		/// Removes a connection
		/// </summary>
		public void RemoveChild(Object childObject)
		{
			m_Connections.Remove( childObject );
			if ( ChildRemoved != null )
			{
				ChildRemoved( this, childObject );
			}
		}

		#endregion
	}
}
