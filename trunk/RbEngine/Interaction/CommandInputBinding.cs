using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Binds input events from a client to a Command object
	/// </summary>
	/// <remarks>
	/// An input binding can be bound to 1 or more clients - it's up to the CommandInputBinding derived class
	/// to subscribe to input events from those clients, using the BindToClient() method.
	/// </remarks>
	public abstract class CommandInputBinding
	{
		/// <summary>
		/// Base class for client-specific bindings
		/// </summary>
		public class ClientBinding
		{
			/// <summary>
			/// true if the input that activates the command is present
			/// </summary>
			public bool					Active
			{
				get
				{
					return m_Active;
				}
			}

			/// <summary>
			/// The bound client
			/// </summary>
			public Network.Client		Client
			{
				get
				{
					return m_Client;
				}
			}

			/// <summary>
			/// Sets the bound client
			/// </summary>
			/// <param name="client">Client to bind to</param>
			public						 ClientBinding( Network.Client client )
			{
				m_Client = client;
			}

			/// <summary>
			/// Set to true if the input that activates the command is present
			/// </summary>
			protected bool				m_Active;

			/// <summary>
			/// The bound client
			/// </summary>
			protected Network.Client	m_Client;

		}

		/// <summary>
		/// Binds to the specified client
		/// </summary>
		/// <param name="client"></param>
		public abstract ClientBinding	BindToClient( Network.Client client );

	}
}
