using System;

namespace RbEngine.Components
{
	//	TODO: Requests should also be messages?

	/// <summary>
	/// A request is a wrapper around a message, allowing it to be carried along a <see cref="RequestChain"/>
	/// </summary>
	/// <seealso cref="RequestChain"/>
	public abstract class Request
	{
		/// <summary>
		/// Access to the stored message
		/// </summary>
		public Message	Message
		{
			get
			{
				return m_Message;
			}
		}

		/// <summary>
		/// Adds the request to the specified chain, wraps up the specified message
		/// </summary>
		/// <param name="chain">Request chain to add this request to</param>
		/// <param name="msg">Message to wrap</param>
		public Request( RequestChain chain, Message msg )
		{
			m_Chain = chain;
			m_Message = msg;
		}

		/// <summary>
		/// Used by action request handlers to handle pending action requests
		/// </summary>
		public delegate void	HandleDelegate( Request request );

		/// <summary>
		/// Called when this request is added to a chain
		/// </summary>
		public void				AddToChain( RequestChain chain )
		{
			m_Chain = chain;
			m_Index = 0;
		}

		/// <summary>
		/// Called when this request is removed from its current chain
		/// </summary>
		public void				RemoveFromChain( )
		{
			m_Chain = null;
		}

		/// <summary>
		/// Moves the request to the next handler in the chain
		/// </summary>
		public void				Handled( )
		{
			if ( m_Index < m_Chain.Count )
			{
				//	Request has not reached the end of the chain - pass it to the next handler
				m_Chain.CallHandler( ref m_Index, this );
			}
		}

		private Message			m_Message;
		private RequestChain	m_Chain;
		private int				m_Index;
	}
}

