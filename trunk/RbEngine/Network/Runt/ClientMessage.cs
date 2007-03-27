using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// A message sent from a server to a client
	/// </summary>
	public class ClientMessage : Components.Message
	{
		/// <summary>
		/// Sets up this client message
		/// </summary>
		public ClientMessage( uint sequence, UpdateMessage[] updateMessages )
		{
			m_Sequence			= sequence;
			m_UpdateMessages	= updateMessages;
		}

		private uint			m_Sequence;
		private UpdateMessage[] m_UpdateMessages;
	}
}
