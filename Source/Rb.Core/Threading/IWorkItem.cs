
using System;

namespace Rb.Core.Threading
{
	/// <summary>
	/// A work item, to be processed by an IWorker
	/// </summary>
	public interface IWorkItem
	{
		/// <summary>
		/// Work item name
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Does work
		/// </summary>
		/// <param name="progress">Progress monitor. Never null</param>
		void DoWork( IProgressMonitor progress );

		/// <summary>
		/// Handles exceptions thrown by DoWork()
		/// </summary>
		/// <param name="progress">Progress monitor</param>
		/// <param name="ex">Exception details</param>
		void WorkFailed( IProgressMonitor progress, Exception ex );

		/// <summary>
		/// Work complete callback
		/// </summary>
		void WorkComplete( IProgressMonitor progress );
	}
}
