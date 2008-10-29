
namespace Rb.Core.Threading
{
	/// <summary>
	/// A work item, to be processed by an IWorker
	/// </summary>
	public interface IWorkItem
	{
		/// <summary>
		/// Does work
		/// </summary>
		void DoWork( );

		/// <summary>
		/// Work complete callback
		/// </summary>
		void WorkComplete( );
	}
}
