using System;
using Rb.Core.Utils;

namespace Rb.Core.Threading
{
	/// <summary>
	/// Base implementation of <see cref="IWorkItemQueue"/>
	/// </summary>
	public abstract class WorkItemQueue : IWorkItemQueue
	{
		#region Work item queue helpers

		/// <summary>
		/// Adds a work item 
		/// </summary>
		/// <param name="queue">Item queue</param>
		/// <param name="work"></param>
		public static void Enqueue( IWorkItemQueue queue, ActionDelegates.Action work )
		{
			queue.Enqueue( new SimpleDelegateWorkItem( "", work, null ) );
		}

		/// <summary>
		/// Adds a work item 
		/// </summary>
		/// <param name="queue">Item queue</param>
		/// <param name="work">Work delegate</param>
		/// <param name="p0">Parameter to pass to action</param>
		public static void Enqueue<P0>( IWorkItemQueue queue, ActionDelegates.Action<P0> work, P0 p0 )
		{
			queue.Enqueue( SimpleDelegateWorkItem.Create( "", work, p0 ) );
		}

		/// <summary>
		/// Adds a work item 
		/// </summary>
		/// <param name="queue">Item queue</param>
		/// <param name="work">Work delegate</param>
		/// <param name="p0">Parameter to pass to action</param>
		/// <param name="p1">Parameter to pass to action</param>
		public static void Enqueue<P0, P1>( IWorkItemQueue queue, ActionDelegates.Action<P0, P1> work, P0 p0, P1 p1 )
		{
			queue.Enqueue( SimpleDelegateWorkItem.Create( "", work, p0, p1 ) );
		}

		#endregion

		#region IWorkItemQueue Members

		/// <summary>
		/// Adds a work item progress monitor to the queue
		/// </summary>
		/// <param name="monitor"></param>
		public void AddMonitor( IWorkItemProgressMonitor monitor )
		{
			if ( monitor == null )
			{
				throw new ArgumentNullException( "monitor" );
			}
			ProgressMonitors.Monitors.Add( monitor );
		}

		/// <summary>
		/// Removes a progress monitor from the queue
		/// </summary>
		/// <param name="monitor">Progress monitor to remove</param>
		public void RemoveMonitor( IWorkItemProgressMonitor monitor )
		{
			if ( monitor == null )
			{
				throw new ArgumentNullException( "monitor" );
			}
			ProgressMonitors.Monitors.Remove( monitor );
		}

		/// <summary>
		/// Enqueues a work item
		/// </summary>
		/// <param name="workItem">Work item to enqueue</param>
		public abstract void Enqueue( IWorkItem workItem );

		/// <summary>
		/// Returns the number of work items in the queue
		/// </summary>
		public abstract int NumberOfWorkItemsInQueue
		{
			get;
		}

		/// <summary>
		/// Empties this queue
		/// </summary>
		public abstract void EmptyQueue( );

		#endregion

		#region Protected Members

		/// <summary>
		/// Gets the progress monitor list
		/// </summary>
		protected WorkItemProgressMonitorList ProgressMonitors
		{
			get { return m_ProgressMonitors; }
		}

		#endregion

		#region Private Members

		private readonly WorkItemProgressMonitorList m_ProgressMonitors = new WorkItemProgressMonitorList( );

		#endregion
	}
}
