
namespace Rb.Core.Threading
{
	/// <summary>
	/// Interface for classes that monitor the progress of work items
	/// </summary>
	/// <remarks>
	/// See <see cref="ProgressMonitor.Null"/> for default (empty) implementation
	/// </remarks>
	public interface IProgressMonitor
	{
		/// <summary>
		/// Notification of progress
		/// </summary>
		/// <param name="progress">Normalized progress (range 0-1)</param>
		/// <returns>Returns true if the process should continue, false if the process should cancel.</returns>
		bool UpdateProgress( float progress );

		/// <summary>
		/// Notification of failure
		/// </summary>
		/// <param name="failureInfo">Failure information (usually exception)</param>
		void WorkFailed( object failureInfo );

		/// <summary>
		/// Notification of completion
		/// </summary>
		void WorkComplete( );
	}
}
