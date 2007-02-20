using System;
using System.Collections;

namespace RbEngine.Entities
{
	/// <summary>
	/// Request handler chain
	/// </summary>
	public class ActionRequestChain : IActionRequestHandler
	{
		/// <summary>
		/// Adds a request to the chain
		/// </summary>
		public void			AddRequest( ActionRequest request )
		{
			request.AddedToChain( this );
			( ( IActionRequestHandler )m_Handlers[ 0 ] ).PendingRequest = request;
		}

		/// <summary>
		/// Returns the indexed handler
		/// </summary>
		public IActionRequestHandler	GetHandler( int index )
		{
			return ( index >= m_Handlers.Count ) ? this : ( IActionRequestHandler )m_Handlers[ index ];
		}

		/// <summary>
		/// Adds a handler to the chain
		/// </summary>
		public void			AddHandler( Object obj )
		{
			m_Handlers.Add( obj );
		}

		#region	Private stuff

		private ArrayList	m_Handlers = new ArrayList( );

		#endregion

		#region IActionRequestHandler Members

		public ActionRequest PendingRequest
		{
			get
			{
				// TODO:  Add RequestChain.PendingRequest getter implementation
				return null;
			}
			set
			{
				// TODO:  Add RequestChain.PendingRequest setter implementation
			}
		}

		#endregion
	}
}
