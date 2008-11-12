using System;

namespace Rb.Core.Threading
{
	/// <summary>
	/// Queues up work items (<see cref="IWorkItem"/>) for processing.
	/// </summary>
	/// <remarks>
	/// To queue up a single delegate call, use <see cref="DelegateWorkItem"/> to create
	/// a work item encapsulating the delegate.
	/// For source-sink work items (delegate pairs, one which does work and returns a value,
	/// the other which handles the returned value), use <see cref="SourceSinkWorkItem"/>.
	/// Progress can be monitored on all work items by adding a progress monitor.
	/// </remarks>
	public interface IWorkItemQueue
	{
		/// <summary>
		/// Adds a progress monitor to the queue
		/// </summary>
		/// <param name="monitor">Progress monitor to add</param>
		void AddMonitor( IWorkItemProgressMonitor monitor );

		/// <summary>
		/// Removes a progress monitor from the queue
		/// </summary>
		/// <param name="monitor">Progress monitor to remove</param>
		void RemoveMonitor( IWorkItemProgressMonitor monitor );

		/// <summary>
		/// Adds a new work item to the queue
		/// </summary>
		/// <param name="workItem">Work item to add</param>
		/// <exception cref="ArgumentNullException">Thrown if workItem is null</exception>
		void Enqueue( IWorkItem workItem );

		/// <summary>
		/// Empties the work queue of any queued work items.
		/// </summary>
		void EmptyQueue( );
	}
}