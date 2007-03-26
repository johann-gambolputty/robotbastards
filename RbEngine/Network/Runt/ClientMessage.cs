using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Summary description for ClientMessage.
	/// </summary>
	public class ClientMessage
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
