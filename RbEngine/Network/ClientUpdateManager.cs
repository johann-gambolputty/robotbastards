using System;

namespace RbEngine.Network
{
	/*
	 * TcpClientConnectionListener listens out for client connections. If it finds one, it creates
	 * a connection, and a ClientUpdateManager based on that connection. The manager updates on
	 * a fixed time-step. Entity client update components subscribe to the manager, which requests
	 * update messages, which it collates into a single large packet that is sent over the connection
	 */

	/// <summary>
	/// Listens for client update messages over the client-server connection
	/// </summary>
	public class ClientUpdateManager : Scene.ISceneObject
	{
		/// <summary>
		/// Sets the name of the connection used by the update manager
		/// </summary>
		public string ConnectionName
		{
			set
			{
				m_ConnectionName = value;
			}
			get
			{
				return m_ConnectionName;
			}
		}

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to the scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			//	Get the connection set from the scene
			Connections connections = ( Connections )db.GetSystem( typeof( Connections ) );
			if ( connections == null )
			{
				throw new ApplicationException( string.Format( "\"{0}\" must be added after a Connections object is added to the scene systems", GetType( ).Name ) );
			}

			//	Get the named connection from the connection set
			IConnection connection = connections.GetConnection( ConnectionName );
			if ( connection == null )
			{
				throw new ApplicationException( string.Format( "Unable to find connection named \"{0}\" for \"{1}\" instance", ConnectionName, GetType( ).Name ) );
			}

			m_ConnectionName	= string.Empty;
			m_Connection		= connection;
		}

		/// <summary>
		///	Called when this object is removed from the scene
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
		}

		#endregion

		private string		m_ConnectionName;
		private IConnection	m_Connection;
	}
}
