
namespace Rb.Core.Threading
{
	/// <summary>
	/// Adapts an <see cref="IProgressMonitor"/> to calls through to an <see cref="IWorkItemProgressMonitor"/>
	/// </summary>
	public class ProgressMonitorWorkItemAdapter : IProgressMonitor
	{
		/// <summary>
		/// Sets the work item progress monitor that gets called by this adapter
		/// </summary>
		public ProgressMonitorWorkItemAdapter( IWorkItemProgressMonitor workItemMonitor )
		{
			m_WorkItemMonitor = workItemMonitor;
		}

		/// <summary>
		/// Gets/sets the current work item
		/// </summary>
		public IWorkItem CurrentWorkItem
		{
			get { return m_CurrentWorkItem; }
			set { m_CurrentWorkItem = value; }
		}

		#region IProgressMonitor Members

		/// <summary>
		/// Notification of work item progress. Calls through to internal work item progress monitor
		/// </summary>
		public bool UpdateProgress( float progress )
		{
			return m_WorkItemMonitor.UpdateProgress( CurrentWorkItem, progress );
		}

		/// <summary>
		/// Notification of failure
		/// </summary>
		/// <param name="failureInfo">Failure information (usually exception)</param>
		public void WorkFailed( object failureInfo )
		{
			m_WorkItemMonitor.WorkFailed( CurrentWorkItem, failureInfo );
		}

		/// <summary>
		/// Notification of completion
		/// </summary>
		public void WorkComplete( )
		{
			m_WorkItemMonitor.WorkComplete( CurrentWorkItem );
		}

		#endregion

		#region Private Members

		private IWorkItem m_CurrentWorkItem;
		private readonly IWorkItemProgressMonitor m_WorkItemMonitor;

		#endregion
	}
}
