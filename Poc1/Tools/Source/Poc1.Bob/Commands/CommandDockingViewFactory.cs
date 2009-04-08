using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Windows.Forms.Ui.Docking;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Controls.Rendering;
using Poc1.Bob.Core.Interfaces.Commands;
using Rb.Core.Utils;

namespace Poc1.Bob.Commands
{
	/// <summary>
	/// Implements docking command views
	/// </summary>
	public class CommandDockingViewFactory : ICommandViewFactory
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="viewManager">View manager</param>
		public CommandDockingViewFactory( IViewManager viewManager )
		{
			Arguments.CheckNotNull( viewManager, "viewManager" );
			m_ViewManager = viewManager;

			m_RenderTargetsView = new DockingViewInfo( "Render Targets", CreateRenderTargetsViewControl, true );
		}

		#region ICommandViewFactory Members

		/// <summary>
		/// Shows the render targets view
		/// </summary>
		public void ShowRenderTargetViews( IWorkspace workspace )
		{
			m_ViewManager.Show( workspace, m_RenderTargetsView );
		}

		#endregion

		#region Private Members

		private readonly IViewManager m_ViewManager;
		private readonly IViewInfo m_RenderTargetsView;

		/// <summary>
		///	Called to create a render targets view control for a docking view
		/// </summary>
		private Control CreateRenderTargetsViewControl( IWorkspace workspace )
		{
			return new RenderTargetViewControl( );	
		}

		#endregion
	}
}
