using System;

namespace Rb.Core.Threading
{
	/// <summary>
	/// Simple implementation of <see cref="IWorkItem"/>, with only DoWork() to implement
	/// </summary>
	public abstract class AbstractWorkItem : IWorkItem
	{
		#region IWorkItem Members

		/// <summary>
		/// Gets the name of this work item
		/// </summary>
		public virtual string Name
		{
			get { return GetType( ).Name; }
		}

		/// <summary>
		/// Performs work
		/// </summary>
		/// <param name="progress">Work progress monitor</param>
		public abstract void DoWork( IProgressMonitor progress );

		/// <summary>
		/// Called when work fails
		/// </summary>
		/// <param name="progress">Work progress monitor</param>
		/// <param name="ex">Work failure exception</param>
		public virtual void WorkFailed( IProgressMonitor progress, Exception ex )
		{
			progress.WorkFailed( ex );
		}

		/// <summary>
		/// Called when work is complete
		/// </summary>
		/// <param name="progress">Work progress monitor</param>
		public virtual void WorkComplete( IProgressMonitor progress )
		{
			progress.WorkComplete( );
		}

		#endregion
	}
}
