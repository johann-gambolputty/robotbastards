
namespace Rb.Core.Graphs
{
	/// <summary>
	/// Render graph data sources interface
	/// </summary>
	public interface IGraphDataSources
	{
		/// <summary>
		/// Adds a new data source
		/// </summary>
		/// <param name="source">Data source</param>
		void Add( IGraphDataSource source );
		
		/// <summary>
		/// Removes a data source
		/// </summary>
		/// <param name="source">Data source</param>
		void Remove( IGraphDataSource source );

		/// <summary>
		/// Gets a render graph data source by name
		/// </summary>
		/// <param name="name">Data source name</param>
		/// <returns>Named data source</returns>
		IGraphDataSource this[ string name ]
		{
			get;
		}
	}
}
