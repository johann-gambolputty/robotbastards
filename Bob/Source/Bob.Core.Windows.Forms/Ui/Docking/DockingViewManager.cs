using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Workspaces.Interfaces;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace Bob.Core.Windows.Forms.Ui.Docking
{
	/// <summary>
	/// Creates dockable views
	/// </summary>
	public class DockingViewManager : IViewManager
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="mainDockPanel">Main docking panel to dock views to</param>
		public DockingViewManager( DockPanel mainDockPanel )
		{
			Arguments.CheckNotNull( mainDockPanel, "mainDockPanel" );
			m_MainDockPanel = mainDockPanel;
		}

		/// <summary>
		/// Shows a docking view
		/// </summary>
		public void Show( IWorkspace workspace, DockingViewInfo dockView )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			Arguments.CheckNotNull( dockView, "dockView" );

			if ( !m_Views.Contains( dockView ) )
			{
				Arguments.ThrowArgumentException( "view", "View \"{0}\" did not appear in registered view list (use RegisterViews())", dockView.Name );
			}

			DockContent content;
			if ( !m_ViewDocks.TryGetValue( dockView, out content ) )
			{
				content = CreateViewDockContent( workspace, dockView );
				content.Show( m_MainDockPanel, dockView.DefaultDockState );
			}
			else if ( content.DockPanel != null )
			{
				content.Show( );	//	Content is just hidden
			}
			else
			{
				m_ViewDocks.Remove( dockView );
				content = CreateViewDockContent( workspace, dockView );
				content.Show( m_MainDockPanel, dockView.DefaultDockState );
			}
		}

		#region IViewManager Members

		/// <summary>
		/// Starts a layout
		/// </summary>
		/// <param name="workspace">Current workspace</param>
		/// <param name="name">Layout name</param>
		/// <param name="views">All views that can be displayed in this layout</param>
		public void BeginLayout( IWorkspace workspace, string name, IViewInfo[] views )
		{
			SwitchLayout( workspace, name, views );
		}


		/// <summary>
		/// Creates a view from a view object
		/// </summary>
		/// <param name="workspace">Workspace context</param>
		/// <param name="view">View information</param>
		public void Show( IWorkspace workspace, IViewInfo view )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			DockingViewInfo dockView = Arguments.CheckedNonNullCast<DockingViewInfo>( view, "view" );
			Show( workspace, dockView );
		}

		/// <summary>
		/// Saves the current layout
		/// </summary>
		public void SaveLayout( )
		{
			string layoutFile = GetLayoutFilePath( m_LayoutName );
			m_MainDockPanel.SaveAsXml( layoutFile );
		}


		/// <summary>
		/// Closes the current layout
		/// </summary>
		public void EndLayout( IWorkspace workspace )
		{
			SwitchLayout( workspace, "Default", new IViewInfo[ 0 ] );
		}

		#endregion

		#region Private Members

		private string m_LayoutName = "Default";
		private readonly List<DockingViewInfo> m_Views = new List<DockingViewInfo>( );
		private readonly Dictionary<DockingViewInfo, DockContent> m_ViewDocks = new Dictionary<DockingViewInfo, DockContent>( );
		private readonly DockPanel m_MainDockPanel;

		/// <summary>
		/// Saves then closes the current layout before loading a new one
		/// </summary>
		private void SwitchLayout( IWorkspace workspace, string layoutName, IViewInfo[] views )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			Arguments.CheckNotNullOrEmpty( layoutName, "layoutName" );
			SaveLayout( );
			CloseAllViews( );
			m_LayoutName = layoutName;
			RegisterViews( views );
			LoadLayout( workspace );	
		}

		/// <summary>
		/// Registers all views available in the current layout
		/// </summary>
		private void RegisterViews( IViewInfo[] views )
		{
			if ( views == null )
			{
				return;
			}
			foreach ( IViewInfo view in views )
			{
				if ( view is DockingViewInfo )
				{
					m_Views.Add( ( DockingViewInfo )view );
				}
			}
		}

		/// <summary>
		/// Returns a dock content object from a string that has been persisted to a layout file
		/// </summary>
		private IDockContent DeserializeContent( IWorkspace workspace, string persistString )
		{
			foreach ( DockingViewInfo view in m_Views )
			{
				if ( view.Name == persistString )
				{
					return CreateViewDockContent( workspace, view );
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the name of a layout file
		/// </summary>
		private static string GetLayoutFilePath( string layoutName )
		{
			Assembly refAsm = Assembly.GetExecutingAssembly( );
			string filename = refAsm.GetName( ).Name + " " + layoutName + " layout for " + Environment.UserName + ".xml";
			return Path.Combine( Path.GetDirectoryName( refAsm.Location ), filename );
		}

		/// <summary>
		/// Loads the current layout
		/// </summary>
		private void LoadLayout( IWorkspace workspace )
		{
			string layoutFile = GetLayoutFilePath( m_LayoutName );
			if ( !File.Exists( layoutFile ) )
			{
				return;
			}
			DeserializeDockContent deserialize =
				delegate( string ps )
				{
					return DeserializeContent( workspace, ps );
				};
			m_MainDockPanel.LoadFromXml( layoutFile, deserialize );
		}

		/// <summary>
		/// Closes all current views
		/// </summary>
		private void CloseAllViews( )
		{
			m_Views.Clear( );
			while ( m_MainDockPanel.Contents.Count > 0 )
			{
				m_MainDockPanel.Contents[ 0 ].DockHandler.Close( );
			}
		}

		/// <summary>
		/// Creates a DockContent for a view, and registers it in the dock view map
		/// </summary>
		private DockContent CreateViewDockContent( IWorkspace workspace, DockingViewInfo dockView )
		{
			Control control = dockView.CreateControl( workspace );
			if ( control == null )
			{
				return null;
			}

			DockContent content = new WindowDockContent( dockView.Name );

			content.Text = dockView.Name;
			content.Controls.Add( control );
			content.AutoScroll = true;
		//	content.HideOnClose = true;

			control.Dock = DockStyle.Fill;

			m_ViewDocks.Add( dockView, content );
			return content;
		}

		/// <summary>
		/// Provides custom persistence strings to DockContent
		/// </summary>
		private class WindowDockContent : DockContent
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="persistString">String to write when content is persists</param>
			public WindowDockContent( string persistString )
			{
				m_PersistString = persistString;
			}

			/// <summary>
			/// Returns the persistence string passed to the constructor
			/// </summary>
			protected override string GetPersistString( )
			{
				return m_PersistString;
			}

			#region Private Members

			private readonly string m_PersistString;

			#endregion
		}


		#endregion
	}
}
