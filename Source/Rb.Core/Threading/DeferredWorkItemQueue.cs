using System.Collections.Generic;

namespace Rb.Core.Threading
{
	/// <summary>
	/// Defers execution of all work items in the queue until the Start() method is called
	/// </summary>
	/// <remarks>
	/// Useful to create a batch of tasks that can be represented in the UI prior to execution
	/// </remarks>
	public class DeferredWorkItemQueue : IWorkItemQueue
	{
		/// <summary>
		/// Sets the internal queue that actually executes the work items on Start()
		/// </summary>
		public DeferredWorkItemQueue( IWorkItemQueue internalQueue )
		{
			m_Queue = internalQueue;
		}

		/// <summary>
		/// Dumps all work item added to this queue, to the internal queue
		/// </summary>
		public void Start( )
		{
			foreach ( IWorkItem workItem in m_WorkItems )
			{
				m_Queue.Enqueue( workItem );
			}
			m_WorkItems.Clear( );
		}

		#region IWorkItemQueue Members

		/// <summary>
		/// Adds a monitor to the internal queue
		/// </summary>
		public void AddMonitor( IWorkItemProgressMonitor monitor )
		{
			m_Queue.AddMonitor( monitor );
		}

		/// <summary>
		/// Removes a monitor from the internal queue
		/// </summary>
		public void RemoveMonitor( IWorkItemProgressMonitor monitor )
		{
			m_Queue.RemoveMonitor( monitor );
		}

		/// <summary>
		/// Adds a work item to the deferred queue. When <see cref="Start()"/> is called, the work items
		/// will be passed to the internal queue
		/// </summary>
		public void Enqueue( IWorkItem workItem )
		{
			m_WorkItems.Add( workItem );
		}

		/// <summary>
		/// Returns the number of work items in the queue
		/// </summary>
		public int NumberOfWorkItemsInQueue
		{
			get
			{
				return m_WorkItems.Count + m_Queue.NumberOfWorkItemsInQueue;
			}
		}

		/// <summary>
		/// Empties this queue
		/// </summary>
		public void EmptyQueue( )
		{
			m_WorkItems.Clear( );
		}

		#endregion

		#region Private Members

		private readonly List<IWorkItem> m_WorkItems = new List<IWorkItem>( );
		private readonly IWorkItemQueue m_Queue;

		#endregion
	}
}
