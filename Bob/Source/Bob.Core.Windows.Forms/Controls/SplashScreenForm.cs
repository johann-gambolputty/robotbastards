using System.Windows.Forms;
using Rb.Core.Threading;

namespace Bob.Core.Windows.Forms.Controls
{
	/// <summary>
	/// Shows a splash screen that can be used to process initialization tasks
	/// asynchronously
	/// </summary>
	public partial class SplashScreenForm : Form, IWorkItemProgressMonitor
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SplashScreenForm( )
		{
			InitializeComponent( );
		}

		/// <summary>
		/// Gets the work item queue
		/// </summary>
		public IWorkItemQueue WorkQueue
		{
			get { return m_WorkQueue;  }
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
			return true;
		}

		/// <summary>
		/// Notification of failure
		/// </summary>
		/// <param name="workItem">Work item that failed</param>
		/// <param name="failureInfo">Failure information (usually exception)</param>
		public void WorkFailed( IWorkItem workItem, object failureInfo )
		{
		}

		/// <summary>
		/// Notification of work completion
		/// </summary>
		/// <param name="workItem">Work item that completed</param>
		public void WorkComplete( IWorkItem workItem )
		{
			if ( m_WorkQueue.NumberOfWorkItemsInQueue == 0 )
			{
				Close( );
			}
		}

		#endregion

		#region Private Members

		private readonly DeferredWorkItemQueue m_WorkQueue = new DeferredWorkItemQueue( ExtendedThreadPool.Instance );

		#endregion

		#region Private Event Handlers

		private void SplashScreenForm_Shown( object sender, System.EventArgs e )
		{
			//	Add a work item to the end of the queue
			m_WorkQueue.AddMonitor( this );
			m_WorkQueue.Start( );
		}

		#endregion

	}
}