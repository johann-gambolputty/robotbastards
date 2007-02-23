using System;
using RbEngine.Components;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Command message
	/// </summary>
	public class CommandMessage : Components.Message
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="cmd">Command that this message represents</param>
		/// <param name="cmd">Client that generated the input, that generated this message</param>
		public CommandMessage( Command cmd, Network.Client client )
		{
			m_Command	= cmd;
			m_Client	= client;
		}

		/// <summary>
		/// Associated Command ID
		/// </summary>
		public ushort	Id
		{
			get
			{
				return m_Command.Id;
			}
		}

		/// <summary>
		/// Client that generated the input, that generated the command message
		/// </summary>
		public Network.Client	Client
		{
			get
			{
				return m_Client;
			}
		}

		/// <summary>
		/// Associated command
		/// </summary>
		public Command	Command
		{
			get
			{
				return m_Command;
			}
		}

		private Command			m_Command;
		private Network.Client	m_Client;
	}
}
