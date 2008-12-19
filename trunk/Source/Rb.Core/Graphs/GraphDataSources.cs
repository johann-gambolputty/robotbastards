using System.Collections.Generic;
using Rb.Core.Utils;

namespace Rb.Core.Graphs
{
	/// <summary>
	/// Implementation of <see cref="IGraphDataSources"/>
	/// </summary>
	public class GraphDataSources : IGraphDataSources
	{
		/// <summary>
		/// Adds a new data source
		/// </summary>
		/// <param name="source">Data source</param>
		public void Add( IGraphDataSource source )
		{
			Arguments.CheckNotNull( source, "source" );
			m_Sources.Add( source.Name, source );
		}

		/// <summary>
		/// Removes a data source
		/// </summary>
		/// <param name="source">Data source</param>
		public void Remove( IGraphDataSource source )
		{
			Arguments.CheckNotNull( source, "source" );
			m_Sources.Remove( source.Name );
		}

		/// <summary>
		/// Gets a render graph data source by name
		/// </summary>
		/// <param name="name">Data source name</param>
		/// <returns>Named data source</returns>
		public IGraphDataSource this[ string name ]
		{
			get { return m_Sources[ name ]; }
		}

		#region Private Members

		private readonly Dictionary<string, IGraphDataSource> m_Sources = new Dictionary<string, IGraphDataSource>( );
		
		#endregion
	}
}
