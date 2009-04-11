
using Rb.Interaction.Classes;

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
		public HostedViewInfo( string name, CreateViewDelegate createView ) :
			base( name, createView )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public HostedViewInfo( string name, CreateViewDelegate createView, CommandGroup showCommandGroup ) :
			base( name, createView, showCommandGroup )
		{
		}
		
		/// <summary>
		/// Setup constructor with show command
		/// </summary>
		public HostedViewInfo( string name, CreateViewDelegate createView, Command showCommand ) :
			base( name, createView, showCommand )
		{
		}
	}
}
