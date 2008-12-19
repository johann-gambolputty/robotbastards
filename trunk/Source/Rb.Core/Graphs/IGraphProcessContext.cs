
namespace Rb.Core.Graphs
{
	/// <summary>
	/// Interface for graph processing
	/// </summary>
	public interface IGraphProcessContext
	{
		/// <summary>
		/// Gets/sets user data associated with this context
		/// </summary>
		object UserData
		{
			get; set;
		}
	}
}
