using System;
using System.Threading;
using System.Collections.Generic;
using Rb.Core.Utils;

namespace Rb.Core.Threading
{
	/// <summary>
	/// Extended thread pool. Provides callbacks on completion on work items
	/// </summary>
	/// <remarks>
	/// Adapted from http://www.gotdotnet.com/community/usersamples/Default.aspx?query=ManagedThreadPool
	/// </remarks>
	public class ExtendedThreadPool
	{
		/// <summary>
		/// Maximum number of threads the thread pool has at its disposal.
		/// </summary>
		public const int MaxWorkerThreads = 2;

		/// <summary>
		/// Gets the default singleton instance
		/// </summary>
		public static ExtendedThreadPool Instance
		{
			get { return s_Instance; }
		}
		
		#region Construction

		/// <summary>
		/// Initialize the thread pool.
		/// </summary>
		public ExtendedThreadPool()
		{
			// Create our thread stores; we handle synchronization ourself
			// as we may run into situtations where multiple operations need to be atomic.
			// We keep track of the threads we've created just for good measure; not actually
			// needed for any core functionality.
			m_WorkItems = new Queue<IWorkItem>();
			m_WorkerThreads = new List<Thread>();
			m_InUseThreads = 0;

			// Create our "thread needed" event
			m_WorkerThreadNeeded = new Semaphore( 0 );
			
			// Create all of the worker threads
			for ( int i = 0; i < MaxWorkerThreads; ++i )
			{
				// Create a new thread and add it to the list of threads.
				Thread newThread = new Thread( new ThreadStart( ProcessQueuedItems ) );
				m_WorkerThreads.Add( newThread );

				// Configure the new thread and start it
				newThread.Name = "ExtendedThreadPool #" + i;
				newThread.IsBackground = true;
				newThread.Start( );
			}
		}
		#endregion

		#region Public Methods

		/// <summary>
		/// Queues up a new work item
		/// </summary>
		/// <param name="action">Work item action</param>
		public void Enqueue( ActionDelegates.Action action )
		{
			Enqueue( new DelegateWorkItem( action, null, null, null ) );
		}

		/// <summary>
		/// Queues up a new work item
		/// </summary>
		/// <param name="action">Work item action</param>
		/// <param name="p0">Parameter 0 value to pass to action</param>
		public void Enqueue<P0>( ActionDelegates.Action<P0> action, P0 p0 )
		{
			Enqueue( new DelegateWorkItem( action, new object[] { p0 }, null, null ) );
		}

		/// <summary>
		/// Queues up a new work item
		/// </summary>
		/// <param name="action">Work item action</param>
		/// <param name="p0">Parameter 0 value to pass to action</param>
		/// <param name="p1">Parameter 1 value to pass to action</param>
		public void Enqueue<P0, P1>( ActionDelegates.Action<P0> action, P0 p0, P1 p1 )
		{
			Enqueue( new DelegateWorkItem( action, new object[] { p0, p1 }, null, null ) );
		}

		/// <summary>
		/// Adds a new work item to the queue
		/// </summary>
		/// <param name="workItem">Work item to add</param>
		/// <exception cref="ArgumentNullException">Thrown if workItem is null</exception>
		public void Enqueue( IWorkItem workItem )
		{
			if ( workItem == null )
			{
				throw new ArgumentNullException( "workItem" );
			}
			lock ( m_WorkItems )
			{
				m_WorkItems.Enqueue( workItem );
			}
			m_WorkerThreadNeeded.AddOne( );
		}

		/// <summary>
		/// Empties the work queue of any queued work items.
		/// </summary>
		public void EmptyQueue( )
		{
			lock ( m_WorkItems ) 
			{ 
				// Try to dispose of all remaining state
				foreach( IWorkItem workItem in m_WorkItems )
				{
					try
					{
						DisposableHelper.Dispose( workItem );
					}
					catch
					{
					}
				}

				// Clear all waiting items and reset the number of worker threads currently needed
				// to be 0 (there is nothing for threads to do)
				m_WorkItems.Clear( );
				m_WorkerThreadNeeded.Reset( 0 );
			}
		}
		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the number of currently active threads in the thread pool.
		/// </summary>
		public int ActiveThreads
		{
			get { return m_InUseThreads; }
		}

		/// <summary>
		/// Gets the number of callback delegates currently waiting in the thread pool.
		/// </summary>
		public int WaitingCallbacks
		{
			get
			{
				lock( m_WorkItems )
				{
					return m_WorkItems.Count;
				}
			}
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Default singleton instance
		/// </summary>
		private readonly static ExtendedThreadPool s_Instance = new ExtendedThreadPool( );

		/// <summary>
		/// Delegate marshaller is used to call completion callbacks on the main UI thread (or whatever thread was
		/// responsible for creating this extended thread pool)
		/// </summary>
		private readonly DelegateMarshaller m_Marshaller = new DelegateMarshaller( );

		/// <summary>
		/// Queue of all the callbacks waiting to be executed.
		/// </summary>
		private readonly Queue<IWorkItem> m_WorkItems;

		/// <summary>
		/// Used to signal that a worker thread is needed for processing.  Note that multiple
		/// threads may be needed simultaneously and as such we use a semaphore instead of
		/// an auto reset event.
		/// </summary>
		private readonly Semaphore m_WorkerThreadNeeded;

		/// <summary>
		/// List of all worker threads at the disposal of the thread pool.
		/// </summary>
		private readonly List<Thread> m_WorkerThreads;

		/// <summary>
		/// Number of threads currently active.
		/// </summary>
		private int m_InUseThreads;

		/// <summary>
		/// A thread worker function that processes items from the work queue.
		/// </summary>
		private void ProcessQueuedItems( )
		{
			// Process indefinitely
			while( true )
			{
				// Get the next item in the queue.  If there is nothing there, go to sleep
				// for a while until we're woken up when a callback is waiting.
				IWorkItem workItem = null;
				while ( workItem == null )
				{
					// Try to get the next callback available.  We need to lock on the 
					// queue in order to make our count check and retrieval atomic.
					lock( m_WorkItems )
					{
						if ( m_WorkItems.Count > 0 )
						{
							workItem = m_WorkItems.Dequeue( );
						}
					}

					// If we can't get one, go to sleep.
					if ( workItem == null )
					{
						m_WorkerThreadNeeded.WaitOne( );
					}
				}

				// We now have a callback.  Execute it.  Make sure to accurately
				// record how many callbacks are currently executing.
				try 
				{
					Interlocked.Increment( ref m_InUseThreads );
					workItem.DoWork( );
					m_Marshaller.PostAction( workItem.WorkComplete );
				} 
				catch
				{
					// Make sure we don't throw here.  Errors are not our problem.
				}
				finally 
				{
					Interlocked.Decrement( ref m_InUseThreads );
				}
			}
		}

		#endregion

		#region Private Types

		#region Semaphore Class
		
		/// <summary>
		/// Thread pool semaphore
		/// </summary>
		private class Semaphore
		{

			#region Construction

			/// <summary>
			///  Initialize the semaphore as a counting semaphore.
			/// </summary>
			/// <param name="count">Initial number of threads that can take out units from this semaphore.</param>
			/// <exception cref="ArgumentException">Throws if the count argument is less than 1.</exception>
			public Semaphore( int count )
			{
				if ( count < 0 )
				{
					throw new ArgumentException( "Semaphore must have a count of at least 0.", "count" );
				}
				m_Count = count;
			}

			#endregion

			#region Synchronization Operations

			/// <summary>
			/// V the semaphore (add 1 unit to it).
			/// </summary>
			public void AddOne( )
			{
				// Lock so we can work in peace.  This works because lock is actually
				// built around Monitor.
				lock ( this )
				{
					// Release our hold on the unit of control.  Then tell everyone
					// waiting on this object that there is a unit available.
					++m_Count;
					Monitor.Pulse( this );
				}
			}

			/// <summary>
			/// P the semaphore (take out 1 unit from it).
			/// </summary>
			public void WaitOne( )
			{
				// Lock so we can work in peace.  This works because lock is actually
				// built around Monitor.
				lock ( this )
				{
					// Wait until a unit becomes available.  We need to wait
					// in a loop in case someone else wakes up before us.  This could
					// happen if the Monitor.Pulse statements were changed to Monitor.PulseAll
					// statements in order to introduce some randomness into the order
					// in which threads are woken.
					while ( m_Count <= 0 )
					{
						Monitor.Wait( this, Timeout.Infinite );
					}
					--m_Count;
				}
			}

			/// <summary>
			/// Resets the semaphore to the specified count.  Should be used cautiously.
			/// </summary>
			public void Reset( int count )
			{
				lock ( this )
				{
					m_Count = count;
				}
			}

			#endregion

			#region Private Members

			/// <summary>
			/// The number of units alloted by this semaphore.
			/// </summary>
			private int m_Count;

			#endregion	
		}
 
		#endregion

		#endregion
	}
}
