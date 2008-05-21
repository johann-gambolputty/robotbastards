using System.Windows.Forms;

namespace Rb.NiceControls.Graph
{
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
	}
}
