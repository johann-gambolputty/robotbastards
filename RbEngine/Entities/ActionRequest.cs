using System;

namespace RbEngine.Entities
{
	/// <summary>
	/// A chain of request handlers
	/// </summary>
	public abstract class ActionRequest
	{
		/// <summary>
		/// Called when this request is added to a chain
		/// </summary>
		public void AddedToChain( ActionRequestChain chain )
		{
			m_Chain = chain;
			m_Index = 0;
		}

		/// <summary>
		/// Moves the request to the next handler in the chain
		/// </summary>
		public void	Handled( )
		{
			if ( m_Chain.Count == m_Index )
			{
				Commit( );
			}
			else
			{
				m_Chain.GetHandler( ++m_Index ).PendingRequest = this;
			}
		}

		/// <summary>
		/// Commits the request
		/// </summary>
		public abstract void		Commit( );

		private ActionRequestChain	m_Chain;
		private int					m_Index;
	}
}

