using System;
using System.Windows.Forms;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Binds a command to a keypress
	/// </summary>
	public class CommandKeyInputBinding : CommandInputBinding
	{
		/// <summary>
		/// Client-specific binding
		/// </summary>
		private new class ClientBinding : CommandInputBinding.ClientBinding
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public ClientBinding( Network.Client client, Keys key ) :
					base( client )
			{
				m_Key = key;
				//	TODO: Missing client now...
			//	client.Control.KeyDown += new KeyEventHandler( OnKeyDown );
			//	client.Control.KeyUp += new KeyEventHandler( OnKeyUp );
			}

			private void	OnKeyDown( object sender, KeyEventArgs args )
			{
				if ( args.KeyCode == m_Key )
				{
					m_Active = true;
				}
			}

			private void	OnKeyUp( object sender, KeyEventArgs args )
			{
				if ( args.KeyCode == m_Key )
				{
					m_Active = false;
				}
			}
			
			private Keys	m_Key;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="key">Key to check for</param>
		public CommandKeyInputBinding( Keys key )
		{
			m_Key = key;
		}

		/// <summary>
		/// Binds to the specified client
		/// </summary>
		/// <param name="client">Client to bind to</param>
		public override CommandInputBinding.ClientBinding		BindToClient( Network.Client client )
		{
			return new ClientBinding( client, m_Key );
		}

		private Keys				m_Key;

	}
}
