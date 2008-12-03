
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Controllers
{
	/// <summary>
	/// Base class for 2d graph controllers. Stubs out all methods in <see cref="IGraph2dController"/>
	/// </summary>
	public class Graph2dControllerBase : IGraph2dController
	{
		#region IGraph2dController Members

		/// <summary>
		/// Called by the control owner when the left mouse button is pressed
		/// </summary>
		/// <param name="data">Graph data</param>
		/// <param name="transform">Graph view transform</param>
		/// <param name="dataX">Mouse X position in data space</param>
		/// <param name="dataY">Mouse Y position in data space</param>
		/// <param name="down">True if the button was pressed, false if the button was released</param>
		/// <returns>Returns true if the event was handled, and the control owner should perform no further processing</returns>
		public virtual bool OnMouseLeftButton( IGraph2dSource data, GraphTransform transform, float dataX, float dataY, bool down )
		{
			return false;
		}

		/// <summary>
		/// Called by the control owner when the right mouse button is pressed
		/// </summary>
		/// <param name="data">Graph data</param>
		/// <param name="transform">Graph view transform</param>
		/// <param name="dataX">Mouse X position in data space</param>
		/// <param name="dataY">Mouse Y position in data space</param>
		/// <param name="down">True if the button was pressed, false if the button was released</param>
		/// <returns>Returns true if the event was handled, and the control owner should perform no further processing</returns>
		public virtual bool OnMouseRightButton( IGraph2dSource data, GraphTransform transform, float dataX, float dataY, bool down )
		{
			return false;
		}

		/// <summary>
		/// Called by the control owner when the mouse is moved
		/// </summary>
		/// <param name="data">Graph data</param>
		/// <param name="transform">Graph view transform</param>
		/// <param name="lastDataX">Previous mouse X position in data space</param>
		/// <param name="lastDataY">Previous mouse Y position in data space</param>
		/// <param name="dataX">Mouse X position in data space</param>
		/// <param name="dataY">Mouse Y position in data space</param>
		/// <returns>Returns true if the event was handled, and the control owner should perform no further processing</returns>
		public virtual bool OnMouseMove( IGraph2dSource data, GraphTransform transform, float lastDataX, float lastDataY, float dataX, float dataY )
		{
			return false;
		}

		#endregion
	}
}
