
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes
{
	/// <summary>
	/// Stores a tuple of the a graph's controller, renderer and data objects
	/// </summary>
	public class GraphComponent
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public GraphComponent( )
		{
		}

		/// <summary>
		/// Setup constructor. Controller and renderer are created from <see cref="IGraph2dSource.CreateController()"/>
		/// and <see cref="IGraph2dSource.CreateRenderer()"/>.
		/// </summary>
		/// <param name="name">Graph component name</param>
		/// <param name="data">Graph data</param>
		public GraphComponent( string name, IGraph2dSource data ) :
			this( name, data, data.CreateRenderer( ), data.CreateController( ) )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Graph component name</param>
		/// <param name="data">Graph data</param>
		/// <param name="renderer">Graph renderer</param>
		/// <param name="controller">Graph controller</param>
		public GraphComponent( string name, IGraph2dSource data, IGraph2dRenderer renderer, IGraph2dController controller )
		{
			m_Name = name;
			m_Controller = controller;
			m_Source = data;
			m_Renderer = renderer;
		}

		/// <summary>
		/// Gets/sets the graph controller. If the controller is null, the graph parameters cannot be changed
		/// </summary>
		public IGraph2dController Controller
		{
			get { return m_Controller; }
			set
			{
				m_Controller = value;
			}
		}

		/// <summary>
		/// Gets/sets the graph renderer. If the graph renderer is null, the graph is not shown
		/// </summary>
		public IGraph2dRenderer Renderer
		{
			get { return m_Renderer; }
			set { m_Renderer = value; }
		}

		/// <summary>
		/// Gets/sets the graph data. If the graph data is null, the graph is not shown
		/// </summary>
		public IGraph2dSource Source
		{
			get { return m_Source; }
			set { m_Source = value; }
		}

		/// <summary>
		/// Gets/sets the component name
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set
			{
				m_Name = value;
				if ( m_Source != null )
				{
					m_Source.OnGraphChanged( );
				}
			}
		}

		#region Private Members

		private IGraph2dController m_Controller;
		private IGraph2dRenderer m_Renderer;
		private IGraph2dSource m_Source;
		private string m_Name;

		#endregion

	}
}
