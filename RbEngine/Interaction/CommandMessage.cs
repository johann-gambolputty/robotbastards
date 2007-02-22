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
		/// <param name="cmd"></param>
		public CommandMessage( Command cmd )
		{
			m_Command = cmd;
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
		/// Associated command
		/// </summary>
		public Command	Command
		{
			get
			{
				return m_Command;
			}
		}

		private Command	m_Command;
	}
}
