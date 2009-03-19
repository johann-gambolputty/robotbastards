using System.Collections.Generic;
using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Windows.Forms.Ui.Docking;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Controls.Components;
using Poc1.Bob.Controls.Planet.Clouds;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Planets;
using Poc1.Bob.Core.Classes.Planets.Clouds;
using Poc1.Bob.Core.Classes.Projects.Planets;
using Poc1.Bob.Core.Classes.Projects.Planets.Spherical;
using Poc1.Bob.Core.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace Poc1.Bob.Projects
{
	/// <summary>
	/// Stores docking views for spherical planet compononents
	/// </summary>
	public class SpherePlanetDockingViews : IPlanetViews
	{
		/// <summary>
		/// Sets up the views supported by this class
		/// </summary>
		public SpherePlanetDockingViews( IViewManager viewManager, IViewFactory viewFactory )
		{
			Arguments.CheckNotNull( viewManager, "viewManager" );
			Arguments.CheckNotNull( viewFactory, "viewFactory" );
			m_ViewManager = viewManager;
			m_ViewFactory = viewFactory;

			//	Environment model template views
			m_CloudView = NewDockingView( "Clouds", CreateCloudView, false );

			//	Other views
			NewDockingView( "Planet Composition", CreatePlanetTemplateCompositionView, true );
			NewDockingView( "Planet Display", CreatePlanetDisplay, true, DockState.Document );
		}

		/// <summary>
		/// Gets all views
		/// </summary>
		public IViewInfo[] Views
		{
			get { return m_Views.ToArray( ); }
		}

		/// <summary>
		/// Gets the cloud view
		/// </summary>
		public IViewInfo CloudView
		{
			get { return m_CloudView; }
		}

		#region Private Members

		private readonly IViewManager m_ViewManager;
		private readonly IViewFactory m_ViewFactory;
		private readonly List<IViewInfo> m_Views = new List<IViewInfo>( );

		private readonly IViewInfo m_CloudView;

		/// <summary>
		/// Creates a new docking view, and adds it to the m_Views list
		/// </summary>
		private DockingViewInfo NewDockingView( string name, DockingViewInfo.CreateViewDelegate createView, bool availableAsCommand )
		{
			DockingViewInfo view = new DockingViewInfo( name, createView, availableAsCommand );
			m_Views.Add( view );
			return view;
		}
		
		/// <summary>
		/// Creates a new docking view, and adds it to the m_Views list
		/// </summary>
		private DockingViewInfo NewDockingView( string name, DockingViewInfo.CreateViewDelegate createView, bool availableAsCommand, DockState initialDockState )
		{
			DockingViewInfo view = new DockingViewInfo( name, createView, availableAsCommand, initialDockState );
			m_Views.Add( view );
			return view;
		}

		#region View Creation

		/// <summary>
		/// Helper method for retrieving the current project from a workspace
		/// </summary>
		private static SpherePlanetProject CurrentProject( IWorkspace workspace )
		{
			return ( SpherePlanetProject )( ( WorkspaceEx )workspace ).ProjectContext.CurrentProject;
		}

		/// <summary>
		/// Creates a planet template composition view
		/// </summary>
		private Control CreatePlanetTemplateCompositionView( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject( workspace );
			EditableCompositeControl control = new EditableCompositeControl( );
			IPlanetEnvironmentModelTemplateVisitor visitor = new PlanetEnvironmentModelTemplateViewVisitor( workspace, m_ViewManager, this );
			new EditablePlanetTemplateViewController( m_ViewFactory, control, currentProject.PlanetTemplate, visitor );
			control.PlanetTemplate = currentProject.PlanetTemplate;
			return control;
		}

		/// <summary>
		/// Creates a cloud template model control
		/// </summary>
		private static Control CreateCloudView( IWorkspace workspace )
		{
			IPlanetCloudModelTemplate cloudModelTemplate = CurrentProject( workspace ).PlanetTemplate.GetModelTemplate<IPlanetCloudModelTemplate>( );
			if ( cloudModelTemplate == null )
			{
				//	Show empty view
				return null;
			}
			FlatCloudModelTemplateControl view = new FlatCloudModelTemplateControl( );
			new CloudModelTemplateViewController( view, cloudModelTemplate, null );
			return view;
		}

		/// <summary>
		/// Creates a planet view control
		/// </summary>
		private Control CreatePlanetDisplay( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject( workspace );
			return ( Control )m_ViewFactory.CreatePlanetView( currentProject.Planet );
		}


		#endregion
		
		#endregion
	}
}
