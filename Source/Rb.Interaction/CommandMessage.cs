using System;
using Rb.Core.Components;

namespace Rb.Interaction
{
    /// <summary>
    /// Command message
    /// </summary>
    [Serializable]
    public class CommandMessage : Message
    {
		/// <summary>
		/// Setup constructor
		/// </summary>
		public CommandMessage( Command cmd )
		{
			m_CommandId = cmd.Id;
		}

		/// <summary>
		/// Gets the ID of the command
		/// </summary>
		public int CommandId
		{
			get { return m_CommandId; }
		}

		private readonly byte m_CommandId;
    }
}
