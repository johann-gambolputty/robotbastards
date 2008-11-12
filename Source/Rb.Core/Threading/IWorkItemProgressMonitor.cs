namespace Rb.Core.Threading
{
	/// <summary>
	/// Interface for objects that monitor the progress of work items
	/// </summary>
	public interface IWorkItemProgressMonitor
	{
		/// <summary>
		/// Notification of progress in a work item
		/// </summary>
		/// <param name="workItem">Work item that has progressed</param>
		/// <param name="progress">Normalized progress</param>
		/// <returns>Returns true if the process should continue, false if the process should cancel.</returns>
		bool UpdateProgress( IWorkItem workItem, float progress );

		/// <summary>
		/// Notification of failure
		/// </summary>
		/// <param name="workItem">Work item that failed</param>
		/// <param name="failureInfo">Failure information (usually exception)</param>
		void WorkFailed( IWorkItem workItem, object failureInfo );

		/// <summary>
		/// Notification of work completion
		/// </summary>
		/// <param name="workItem">Work item that completed</param>
		void WorkComplete( IWorkItem workItem );
	}
}
