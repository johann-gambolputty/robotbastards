
using System.Drawing;

namespace Rb.NiceControls.Graph
{
	/// <summary>
	/// Graph interface
	/// </summary>
	public interface IGraph
	{
		/// <summary>
		/// Gets the name of the graphed function (e.g. "Line", "Spline", "Gaussian", etc.)
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Creates an input handler for this graph
		/// </summary>
		IGraphInputHandler CreateInputHandler( );

		/// <summary>
		/// Samples the graph
		/// </summary>
		/// <param name="t">Normalized input</param>
		/// <returns>Value of the graphed function, sampled at t</returns>
		float Sample( float t );
		
		/// <summary>
		/// Renders the graph
		/// </summary>
		void Render( Rectangle bounds, Graphics graphics );
	}
}
