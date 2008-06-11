using System.Drawing;
using System.Windows.Forms;

namespace Rb.NiceControls.Graph
{
	public interface IGraphControl
	{
		/// <summary>
		/// Event, invoked when the graph changes
		/// </summary>
		event System.EventHandler GraphChanged;
		
		/// <summary>
		/// Maps a client y coordinate to a graph y coordinate
		/// </summary>
		float ClientXToGraphX( int x );

		/// <summary>
		/// Maps a client y coordinate to a graph y coordinate
		/// </summary>
		float ClientYToGraphY( int y );

		/// <summary>
		/// Maps a client point to a graph point
		/// </summary>
		PointF ClientToGraph( Point pt );

		/// <summary>
		/// Maps a graph point to a client point
		/// </summary>
		PointF GraphToClient( float x, float y );

		/// <summary>
		/// Gets the underlying control
		/// </summary>
		Control BaseControl
		{
			get;
		}

		/// <summary>
		/// Invokes the GraphChanged event, and invalidated the control
		/// </summary>
		void OnGraphChanged( );
	}
}
