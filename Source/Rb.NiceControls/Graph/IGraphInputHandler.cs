using System.Drawing;

namespace Rb.NiceControls.Graph
{
	/// <summary>
	/// Graph input handler
	/// </summary>
	public interface IGraphInputHandler
	{
		/// <summary>
		/// Attaches this input handler to a control
		/// </summary>
		void Attach( IGraphControl control );

		/// <summary>
		/// Detaches this input handler to a control
		/// </summary>
		void Detach( IGraphControl control );

		/// <summary>
		/// Renders this graph
		/// </summary>
		void Render( Rectangle bounds, Graphics graphics );
	}
}
