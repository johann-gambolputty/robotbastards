
namespace Rb.Core.Graphs
{
	/// <summary>
	/// Named data source for a render graph
	/// </summary>
	public interface IGraphDataSource
	{
		/// <summary>
		/// Gets the name of this data source
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Updates the value of a data target
		/// </summary>
		void UpdateTarget( IGraphDataTarget target );
	}
}
