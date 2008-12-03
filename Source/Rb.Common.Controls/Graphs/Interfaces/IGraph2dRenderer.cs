
using System.Drawing;

namespace Rb.Common.Controls.Graphs.Interfaces
{
	/// <summary>
	/// 2D graph data rendering interface
	/// </summary>
	public interface IGraph2dRenderer
	{
		/// <summary>
		/// Gets/sets the principle colour of the graph
		/// </summary>
		Color Colour
		{
			get; set;
		}

		/// <summary>
		/// Renders graph data
		/// </summary>
		/// <param name="graphics">Graphics object to render into</param>
		/// <param name="transform">Graph transform</param>
		/// <param name="data">Data to render</param>
		/// <param name="cursorDataPt">Position of the mouse cursor in data space</param>
		/// <param name="enabled">The enabled state of the graph control</param>
		void Render( IGraphCanvas graphics, GraphTransform transform, IGraph2dSource data, PointF cursorDataPt, bool enabled );
	}
}
