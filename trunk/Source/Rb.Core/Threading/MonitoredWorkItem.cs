
namespace Rb.Core.Threading
{
	/// <summary>
	/// Stores a work item and a pair of monitors. Used by work queue implementations
	/// </summary>
	public class MonitoredWorkItem : IProgressMonitor
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public MonitoredWorkItem( IWorkItem item, IProgressMonitor localMonitor, IWorkItemProgressMonitor globalMonitor )
		{
			m_WorkItem = item;
			m_LocalMonitor = localMonitor ?? ProgressMonitor.Null;
			m_GlobalMonitor = globalMonitor;
		}

		/// <summary>
		/// Gets the stored work item
		/// </summary>
		public IWorkItem WorkItem
		{
			get { return m_WorkItem; }
		}


		#region IProgressMonitor Members

		/// <summary>
		/// Updates progress
		/// </summary>
		public bool UpdateProgress( float progress )
		{
			return m_LocalMonitor.UpdateProgress( progress ) && m_GlobalMonitor.UpdateProgress( m_WorkItem, progress );
		}

		/// <summary>
		/// Work failed
		/// </summary>
		public void WorkFailed( object failureInfo )
		{
			m_LocalMonitor.WorkFailed( failureInfo );
			m_GlobalMonitor.WorkFailed( m_WorkItem, failureInfo );
		}

		/// <summary>
		/// Work complete
		/// </summary>
		public void WorkComplete( )
		{
			m_LocalMonitor.WorkComplete( );
			m_GlobalMonitor.WorkComplete( m_WorkItem );
		}

		#endregion

		#region Private Members

		private readonly IWorkItem m_WorkItem;
		private readonly IProgressMonitor m_LocalMonitor;
		private readonly IWorkItemProgressMonitor m_GlobalMonitor;

		#endregion

	}
}
