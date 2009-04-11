using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bob.Core.Ui.Interfaces;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Workspaces.Interfaces;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace Bob.Core.Windows.Forms.Ui.Docking
{
	/// <summary>
	/// Keeps a docking property window. Views get added to this property window
	/// </summary>
	public class DockedHostPaneViewManager : IViewManager
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="commandUi">Command UI manager. Any show command attached to views get added to this object</param>
		/// <param name="mainPanel">Main docking panel</param>
		/// <param name="hostPaneName">Name of the host pane</param>
		public DockedHostPaneViewManager( ICommandUiManager commandUi, DockPanel mainPanel, string hostPaneName )
		{
			Arguments.CheckNotNull( commandUi, "commandUi" );
			Arguments.CheckNotNull( mainPanel, "mainPanel" );
			m_UnhostedViewManager = new DockingViewManager( commandUi, mainPanel );
			m_HostViewInfo = new DockingViewInfo( hostPaneName, CreateHostControl, DockState.DockLeft );
		}

		/// <summary>
		/// Starts a layout
		/// </summary>
		/// <param name="workspace">Current workspace</param>
		/// <param name="name">Layout name</param>
		/// <param name="views">All views that can be shown in this layout</param>
		public void BeginLayout( IWorkspace workspace, string name, IViewInfo[] views )
		{
			List<IViewInfo> dockingViews = new List<IViewInfo>( Array.FindAll( views, delegate( IViewInfo view ) { return view is DockingViewInfo; } ) );
			dockingViews.Add( m_HostViewInfo );
			m_UnhostedViewManager.BeginLayout( workspace, name, dockingViews.ToArray( ) );
		}

		/// <summary>
		/// Creates a view from a view object
		/// </summary>
		/// <param name="workspace">Workspace context</param>
		/// <param name="view">View information</param>
		public void Show( IWorkspace workspace, IViewInfo view )
		{
			if ( view is HostedViewInfo )
			{
				//	Make sure the host window is visible
				DockContent dockContent = m_UnhostedViewManager.Show( workspace, m_HostViewInfo );
				Panel hostPanel = ( Panel )dockContent.Controls[ 0 ];
				hostPanel.Controls.Clear( );

				Control hostedControl = ( ( HostedViewInfo )view ).CreateControl( workspace );
				hostedControl.Dock = DockStyle.Fill;
				hostPanel.Controls.Add( hostedControl );

				return;
			}
			if ( view is DockingViewInfo )
			{
				m_UnhostedViewManager.Show( workspace, view );
				return;
			}
			throw new NotSupportedException( "Unsupported view type " + view.GetType( ) );
		}

		/// <summary>
		/// Saves the current layout
		/// </summary>
		public void SaveLayout( )
		{
			m_UnhostedViewManager.SaveLayout( );
		}

		/// <summary>
		/// Closes the current layout
		/// </summary>
		public void EndLayout( IWorkspace workspace )
		{
			m_UnhostedViewManager.EndLayout( workspace );
		}

		#region Private Members

		private readonly DockingViewInfo m_HostViewInfo;
		private readonly DockingViewManager m_UnhostedViewManager;

		/// <summary>
		/// Creates the host host control
		/// </summary>
		private static Control CreateHostControl( IWorkspace workspace )
		{
			Panel panel = new Panel( );
			return panel;
		}

		#endregion
	}
}
