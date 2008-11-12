
namespace Rb.Core.Threading
{
	/// <summary>
	/// Default implementation of <see cref="IProgressMonitor"/>. Does nothing.
	/// </summary>
	public class ProgressMonitor : IProgressMonitor
	{
		/// <summary>
		/// Returns a default instance of this progress monitor type (that does nothing)
		/// </summary>
		public static ProgressMonitor Null
		{
			get { return s_Null; }
		}

		#region IProgressMonitor Members

		/// <summary>
		/// Updates progress. Does nothing.
		/// </summary>
		/// <param name="progress">Normalized progress</param>
		/// <returns>Returns true if the process should continue, false if the process should cancel.</returns>
		public virtual bool UpdateProgress( float progress )
		{
			return true;
		}

		/// <summary>
		/// Notification of failure. Does nothing.
		/// </summary>
		/// <param name="failureInfo">Failure information (usually exception)</param>
		public virtual void WorkFailed( object failureInfo )
		{
		}

		/// <summary>
		/// Notification of work completion. Does nothin
		/// </summary>
		public virtual void WorkComplete( )
		{
		}

		#endregion

		#region Private Members

		private static readonly ProgressMonitor s_Null = new ProgressMonitor( );

		#endregion
	}
}
