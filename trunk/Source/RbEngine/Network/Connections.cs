using System;
using System.Collections;

namespace RbEngine.Network
{
	/// <summary>
	/// Delegate, used by the Connections new connection events
	/// </summary>
	public delegate void NewConnectionDelegate( IConnection connection );

	/// <summary>
	/// A set of client-to-server and server-to-client connections
	/// </summary>
	public class Connections : Components.IParentObject, Components.IXmlLoader, Scene.ISceneObject
	{
		#region	Connections

		/// <summary>
		/// Gets the list of all connections
		/// </summary>
		public ArrayList	AllConnections
		{
			get
			{
				return m_Connections;
			}
		}

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

		private ArrayList		m_Connections = new ArrayList( );
		private Scene.SceneDb	m_Scene;

		#endregion

		#region IParentObject Members

		/// <summary>
		/// Called when a connection is added to this object
		/// </summary>
		public event Components.ChildAddedDelegate		ChildAdded;

		/// <summary>
		/// Called when a connection is removed from this object
		/// </summary>
		public event Components.ChildRemovedDelegate	ChildRemoved;

		/// <summary>
		/// Event, invoked when a client connection is added. A typed version of ChildAdded
		/// </summary>
		public event NewConnectionDelegate				NewClientConnection;

		/// <summary>
		/// Event, invoked when a server connection is added. A typed version of ChildAdded
		/// </summary>
		public event NewConnectionDelegate				NewServerConnection;

		/// <summary>
		/// Event, invoked when a connection is added. A typed version of ChildAdded
		/// </summary>
		public event NewConnectionDelegate				NewConnection;


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
			m_Connections.Add( childObject );
			if ( ChildAdded != null )
			{
				ChildAdded( this, childObject );
			}
			IConnection connection = childObject as IConnection;
			if ( connection != null )
			{
				if ( NewConnection != null )
				{
					NewConnection( connection );
				}
				if ( connection.ConnectionToClient )
				{
					//	Client connection. Invoke the NewClientConnection event, and create handlers
					if ( NewClientConnection != null )
					{
						NewClientConnection( connection );
					}
					foreach ( HandlerBuilder builder in m_ClientHandlerBuilders )
					{
						builder.Build( m_Scene, connection );
					}
				}
				else
				{
					//	Server connection. Invoke the NewServerConnection event, and create handlers
					if ( NewServerConnection != null )
					{
						NewServerConnection( connection );
					}
					foreach ( HandlerBuilder builder in m_ClientHandlerBuilders )
					{
						builder.Build( m_Scene, connection );
					}
				}
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

		#region IXmlLoader Members

		private ArrayList	m_ClientHandlerBuilders = new ArrayList( );
		private ArrayList	m_ServerHandlerBuilders = new ArrayList( );

		/// <summary>
		/// Builder for new client or server handlers
		/// </summary>
		private class HandlerBuilder
		{
			/// <summary>
			/// Handler builder setup
			/// </summary>
			public HandlerBuilder( Type handlerType )
			{
				m_Type = handlerType;
			}

			/// <summary>
			/// Builds the handler
			/// </summary>
			public void Build( Scene.SceneDb scene, IConnection connection )
			{
				Object handler = System.Activator.CreateInstance( m_Type );

				( ( IClientManager )handler ).ClientConnection = connection;

				//	TODO: Should be able to add new handlers to places other that the scene systems
				scene.Systems.AddChild( scene );
				scene.AddToContext( handler );
			}

			private Type m_Type;
		}

		/// <summary>
		/// Parses the element that was responsible for generating this object
		/// </summary>
		public void ParseGeneratingElement( System.Xml.XmlElement element )
		{
		}

		/// <summary>
		/// Parses a sub-element
		/// </summary>
		public bool ParseElement(System.Xml.XmlElement element)
		{
			if ( element.Name == "newClientHandler" )
			{
				Type handlerType = AppDomainUtils.FindType( element.Attributes[ "type" ].Value );
				Output.WriteLineCall( Output.NetworkInfo, "Adding builder for new client handler type \"{0}\"", handlerType );
				m_ClientHandlerBuilders.Add( new HandlerBuilder( handlerType ) );
				return true;
			}
			else if ( element.Name == "newServerHandler" )
			{
				Type handlerType = AppDomainUtils.FindType( element.Attributes[ "type" ].Value );
				Output.WriteLineCall( Output.NetworkInfo, "Adding builder for new server handler type \"{0}\"", handlerType );
				m_ServerHandlerBuilders.Add( new HandlerBuilder( handlerType ) );
				return true;
			}

			return false;
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to the scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			m_Scene = db;
		}

		/// <summary>
		/// Called when this object is removed from the scene
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
			m_Scene = null;
		}

		#endregion
	}
}
