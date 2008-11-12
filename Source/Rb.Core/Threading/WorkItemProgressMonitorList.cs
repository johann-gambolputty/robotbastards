using System.Collections.Generic;


namespace Rb.Core.Threading
{
	/// <summary>
	/// A work item progress monitor that stores a list of child progress monitors
	/// </summary>
	public class WorkItemProgressMonitorList : IWorkItemProgressMonitor
	{
		/// <summary>
		/// Gets the list of monitors
		/// </summary>
		public List<IWorkItemProgressMonitor> Monitors
		{
			get { return m_Monitors; }
		}

		#region IWorkItemProgressMonitor Members

		/// <summary>
		/// Notification of progress in a work item
		/// </summary>
		/// <param name="workItem">Work item that has progressed</param>
		/// <param name="progress">Normalized progress</param>
		/// <returns>Returns true if the process should continue, false if the process should cancel.</returns>
		public bool UpdateProgress( IWorkItem workItem, float progress )
		{
			bool cancel = false;
			foreach ( IWorkItemProgressMonitor monitor in m_Monitors )
			{
				cancel |= !monitor.UpdateProgress( workItem, progress );
			}
			return !cancel;
		}

		/// <summary>
		/// Notification of failure
		/// </summary>
		/// <param name="workItem">Work item that failed</param>
		/// <param name="failureInfo">Failure information (usually exception)</param>
		public void WorkFailed( IWorkItem workItem, object failureInfo )
		{
			foreach ( IWorkItemProgressMonitor monitor in m_Monitors )
			{
				monitor.WorkFailed( workItem, failureInfo );
			}
		}

		/// <summary>
		/// Notification of work completion
		/// </summary>
		/// <param name="workItem">Work item that completed</param>
		public void WorkComplete( IWorkItem workItem )
		{
			foreach ( IWorkItemProgressMonitor monitor in m_Monitors )
			{
				monitor.WorkComplete( workItem );
			}	
		}

		#endregion

		#region Private Members

		private readonly List<IWorkItemProgressMonitor> m_Monitors = new List<IWorkItemProgressMonitor>( );

		#endregion
	}
}
