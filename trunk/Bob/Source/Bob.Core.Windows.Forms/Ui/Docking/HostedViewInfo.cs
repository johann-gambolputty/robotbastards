
namespace Bob.Core.Windows.Forms.Ui.Docking
{
	/// <summary>
	/// Information about a view that is hosted in a host pane
	/// </summary>
	/// <seealso cref="DockedHostPaneViewManager"/>
	public class HostedViewInfo : ControlViewInfo
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public HostedViewInfo( string name, CreateViewDelegate createView, bool availableAsCommand ) :
			base( name, createView, availableAsCommand )
		{
		}
	}
}
