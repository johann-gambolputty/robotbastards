using System;
using System.Collections;

namespace RbEngine.Components
{
	/// <summary>
	/// Request handler chain.
	/// </summary>
	/// <remarks>
	/// Request chains are ordered collections of handlers, that pass a request for action along until it reaches the end of the chain.
	/// Each handler gets a chance to modify the request, or discard the request (by not calling <see cref="Request.Handled"/>, which passes
	/// the request on the to next handler). When all the handlers are finished, the final handler, called the commit handler
	/// <see cref="RequestChain.CommitHandler"/>, is called, that should apply the request.
	/// GRRAR I NEED GENERICS.
	/// </remarks>
	/// <example>
	/// An agent is shot in the face. The engine requests a RequestChain from the agent that handles damage, and adds a damage action to the
	/// chain, using <see cref="AddHandler"/>. The agent is wearing a hat, scarf and spectacles, that could conceivably divert the blast. These items 
	/// are represented on the chain by handlers. A handlers can modify the damage value stored in the action object, or not pass it on through the
	/// chain, thereby effectively deflecting the bullet. If the damage action request reaches the end of the chain, it is handled by the commit
	/// handler, that applies damage directly to the agent.
	/// </example>
	public class RequestChain
	{
		#region	Chain setup

		/// <summary>
		/// Adds a handler to the chain
		/// </summary>
		public void			AddHandler( Request.HandleDelegate handler )
		{
			m_Handlers.Add( handler );
		}

		/// <summary>
		/// Removes any handlers from the chain, whose Delegate.Target object is equal to obj
		/// </summary>
		public void			RemoveHandlerObject( Object obj )
		{
			for ( int handlerIndex = 0; handlerIndex < m_Handlers.Count; ++handlerIndex )
			{
				if ( ( ( Request.HandleDelegate )m_Handlers[ handlerIndex ] ).Target == obj )
				{
					//	Set the handler to null - this will be cleared later
					m_Handlers[ handlerIndex ] = null;
				}
			}
		}

		/// <summary>
		/// Removes a handler from the chain
		/// </summary>
		public void			RemoveHandler( Request.HandleDelegate handler )
		{
			m_Handlers.Remove( handler );
		}

		/// <summary>
		/// The commit handler
		/// </summary>
		public Request.HandleDelegate	CommitHandler
		{
			get
			{
				return m_CommitHandler;
			}
			set
			{
				m_CommitHandler = value;
			}
		}

		/// <summary>
		/// The number of handlers in the chain
		/// </summary>
		public int			Count
		{
			get
			{
				return m_Handlers.Count;
			}
		}

		#endregion

		#region	Chain usage

		/// <summary>
		/// Adds a request to the chain, and calls the first handler
		/// </summary>
		public void			AddRequest( Request request )
		{
			request.AddToChain( this );
			CallHandler( 0, request );
		}

		/// <summary>
		/// Returns the indexed handler
		/// </summary>
		public void			CallNextHandler( ref int index, Request request )
		{
			do
			{
				++index;
			}
			while ( ( index < Count ) && ( m_Handlers[ index ] == null ) );
			
			if ( index >= Count )
			{
				CommitHandler( request );
				request.RemoveFromChain( );
			}
			else
			{
				( ( Request.HandleDelegate )m_Handlers[ index ] )( request );
			}
		}

		#endregion

		#region	Private stuff

		private Request.HandleDelegate	m_CommitHandler;
		private ArrayList				m_Handlers = new ArrayList( );

		#endregion

	}
}
