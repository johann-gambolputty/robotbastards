using System;
using System.ComponentModel;
using System.Threading;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Abstract base class for marshalling delegate calls across threads
	/// </summary>
	public class DelegateMarshaller
	{
		/// <summary>
		/// Default constructor. Initialises the synchronization context
		/// </summary>
		public DelegateMarshaller( )
		{
			m_SynchronizationContext = AsyncOperationManager.SynchronizationContext;
			if ( m_SynchronizationContext == null )
			{
				throw new InvalidOperationException( "Cannot create DelegateMarshaller. Synchronization context does not exist in the current thread context" );
			}
		}

		/// <summary>
		/// Posts an event handler to the marshaller
		/// </summary>
		/// <typeparam name="T">EventArgs type</typeparam>
		/// <param name="sender">Event sender</param>
		/// <param name="handler">Event handler delegate</param>
		/// <param name="args">Event arguments parameter</param>
		public void PostEventHandler<T>( EventHandler<T> handler, object sender, T args )
			where T : EventArgs
		{
			m_SynchronizationContext.Post( delegate { handler( sender, args ); }, null );
		}

		/// <summary>
		/// Posts an action without any parameters
		/// </summary>
		public void PostAction( ActionDelegates.Action action )
		{
			m_SynchronizationContext.Post( delegate { action( ); }, null );
		}
		
		
		#region Private Members
		
		private SynchronizationContext m_SynchronizationContext;
 
		#endregion
	}
}
