using System;
using System.Net;
using System.Net.Sockets;

namespace RbEngine.Network
{
	/// <summary>
	/// Server base class
	/// </summary>
	/// <remarks>
	/// The ServerBase constructor adds the new server to the ServerManager singleton
	/// </remarks>
	public abstract class ServerBase : Components.Component, Components.INamedObject
	{
		/// <summary>
		/// Event, invoked when the name is changed
		/// </summary>
		public event Components.NameChangedDelegate NameChanged;

		/// <summary>
		/// The name of the server
		/// </summary>
		public string	Name
		{
			set
			{
				m_Name = value;
				if ( NameChanged != null )
				{
					NameChanged( this );
				}
			}
			get
			{
				return m_Name;
			}
		}

		/// <summary>
		/// The scene stored on the server
		/// </summary>
		public Scene.SceneDb	Scene
		{
			set
			{
				m_Scene = value;
				if ( m_Scene != null )
				{
					m_Scene.Server = this;
				}
			}
			get
			{
				return m_Scene;
			}
		}

		/// <summary>
		/// Constructor. Adds this server to the server manager
		/// </summary>
		public					ServerBase( )
		{
			ServerManager.Inst.RegisterServer( this );
		}

		/// <summary>
		/// Adds a client to the server
		/// </summary>
		/// <param name="client">Client to add</param>
		public virtual void	AddClient( Client client )
		{
		}

		/// <summary>
		/// Removes a client from the server
		/// </summary>
		/// <param name="client">Client to remove</param>
		public virtual void	RemoveClient( Client client )
		{
		}

		private Scene.SceneDb	m_Scene;
		string					m_Name;

	}
}
