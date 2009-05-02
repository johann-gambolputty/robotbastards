using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Windows.Forms.Ui.Docking;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Commands;
using Poc1.Bob.Controls.Atmosphere;
using Poc1.Bob.Controls.Components;
using Poc1.Bob.Controls.Planet;
using Poc1.Bob.Controls.Planet.Clouds;
using Poc1.Bob.Controls.Planet.Terrain;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Commands;
using Poc1.Bob.Core.Classes.Planets;
using Poc1.Bob.Core.Classes.Planets.Clouds;
using Poc1.Bob.Core.Classes.Planets.Terrain;
using Poc1.Bob.Core.Classes.Projects.Planets;
using Poc1.Bob.Core.Classes.Projects.Planets.Spherical;
using Poc1.Bob.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;
using Rb.Core.Utils;
using Rb.Interaction.Classes;
using WeifenLuo.WinFormsUI.Docking;


namespace Poc1.Bob.Projects
{
	/// <summary>
	/// Stores docking views for spherical planet compononents
	/// </summary>
	public class SpherePlanetDockingViews : DefaultCommandDockingViews, IPlanetViews
	{
		/// <summary>
		/// Sets up the views supported by this class
		/// </summary>
		public SpherePlanetDockingViews( IViewManager viewManager, IViewFactory viewFactory ) :
			base( viewManager )
		{
			Arguments.CheckNotNull( viewManager, "viewManager" );
			Arguments.CheckNotNull( viewFactory, "viewFactory" );
			m_ViewManager = viewManager;
			m_ViewFactory = viewFactory;

			//	Environment model templates with special views
			AsTemplateView<IPlanetSimpleCloudTemplate>( NewHostedView( "Simple Cloud Template Properties", CreateCloudView, null ) );
			AsTemplateView<IPlanetHomogenousProceduralTerrainTemplate>( NewHostedView( "Homogenous Procedural Terrain Template Properties", CreateHomogenousProceduralTerrainView, null ) );
			AsTemplateView<IPlanetAtmosphereTemplate>( NewHostedView( "Atmosphere Template Properties", CreateAtmosphereScatteringView, null ) );

			//	Environment model templates with generic views
			CreateGenericTemplateView<IPlanetRingTemplate>( "Ring Template Properties" );
			CreateGenericTemplateView<IPlanetOceanTemplate>( "Ocean Template Properties" );

			m_PlanetModelView = NewHostedView( "Sphere Planet Properties", CreatePlanetModelView, DefaultCommands.ViewCommands );

			//	Other views
			NewDockingView( "Planet Composition", CreatePlanetTemplateCompositionView, DefaultCommands.ViewCommands );
			NewDockingView( "Planet Display", CreatePlanetDisplay, DefaultCommands.ViewCommands, DockState.Document );

			m_DefaultView = NewHostedView( "Empty", CreateEmptyView, null );
		}

		/// <summary>
		/// Gets all views
		/// </summary>
		public override IViewInfo[] Views
		{
			get
			{
				List<IViewInfo> views = new List<IViewInfo>( base.Views );
				views.AddRange( m_Views );
				return views.ToArray( );
			}
		}

		/// <summary>
		/// Gets the planet model view
		/// </summary>
		public IViewInfo PlanetModelView
		{
			get { return m_PlanetModelView; }
		}

		/// <summary>
		/// Gets the default view
		/// </summary>
		public IViewInfo DefaultView
		{
			get { return m_DefaultView; }
		}

		/// <summary>
		/// Gets a view for a specified environment model template
		/// </summary>
		public IViewInfo GetTemplateView( IPlanetEnvironmentModelTemplate template )
		{
			foreach ( KeyValuePair<Type, IViewInfo> templateView in m_TemplateViews )
			{
				if ( templateView.Key.IsInstanceOfType( template ) )
				{
					return templateView.Value;
				}
			}
			return null;
		}

		#region Private Members

		private readonly IViewManager m_ViewManager;
		private readonly IViewFactory m_ViewFactory;
		private readonly List<IViewInfo> m_Views = new List<IViewInfo>( );
		private readonly Dictionary<Type, IViewInfo> m_TemplateViews = new Dictionary<Type, IViewInfo>( );

		private readonly IViewInfo m_DefaultView;
		private readonly IViewInfo m_PlanetModelView;

		/// <summary>
		/// Assigns a view to a template type
		/// </summary>
		private IViewInfo AsTemplateView<TTemplate>( IViewInfo view )
		{
			m_TemplateViews.Add( typeof( TTemplate ), view );
			return view;
		}

		/// <summary>
		/// Creates a generic template view (property grid only), and assigns it to the specified template type
		/// </summary>
		private IViewInfo CreateGenericTemplateView<TTemplate>( string title )
			where TTemplate : class, IPlanetEnvironmentModelTemplate
		{
			IViewInfo view = NewHostedView( title, CreateGenericTemplatePropertiesView<TTemplate>, null );
			m_TemplateViews.Add( typeof( TTemplate ), view );
			return view;
		}

		/// <summary>
		/// Creates a new docking view, and adds it to the m_Views list
		/// </summary>
		private HostedViewInfo NewHostedView( string name, DockingViewInfo.CreateViewDelegate createView, CommandGroup showCommandGroup )
		{
			HostedViewInfo view = new HostedViewInfo( name, createView, showCommandGroup );
			m_Views.Add( view );
			return view;
		}

		/// <summary>
		/// Creates a new docking view, and adds it to the m_Views list
		/// </summary>
		private DockingViewInfo NewDockingView( string name, ControlViewInfo.CreateViewDelegate createView, CommandGroup showCommandGroup )
		{
			DockingViewInfo view = new DockingViewInfo( name, createView, showCommandGroup );
			m_Views.Add( view );
			return view;
		}
		
		/// <summary>
		/// Creates a new docking view, and adds it to the m_Views list
		/// </summary>
		private DockingViewInfo NewDockingView( string name, ControlViewInfo.CreateViewDelegate createView, CommandGroup showCommandGroup, DockState initialDockState )
		{
			DockingViewInfo view = new DockingViewInfo( name, createView, showCommandGroup, initialDockState );
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
			new EditablePlanetTemplateViewController( workspace, m_ViewFactory, control, currentProject.PlanetTemplate, TemplateCompositionViewActionHandler );
			control.PlanetTemplate = currentProject.PlanetTemplate;
			return control;
		}

		/// <summary>
		/// Action handler for a template composition view. Shows a view for the selected template
		/// </summary>
		private void TemplateCompositionViewActionHandler( IWorkspace workspace, object templateComponent )
		{
			//	TODO: AP: Can go to base class
			if ( templateComponent is IPlanetModelTemplate )
			{
				m_ViewManager.Show( workspace, m_PlanetModelView );
				return;
			}
			IPlanetEnvironmentModelTemplate template = templateComponent as IPlanetEnvironmentModelTemplate;
			IViewInfo view = GetTemplateView( template );
			if ( view != null )
			{
				m_ViewManager.Show( workspace, view );
			}
			else
			{
				m_ViewManager.Show( workspace, DefaultView );
			}
		}

		/// <summary>
		/// Creates an empty view
		/// </summary>
		private static Control CreateEmptyView( IWorkspace workspace )
		{
			Label label = new Label( );
			label.Text = "No Properties Available";
			return label;
		}

		/// <summary>
		/// Creates a view for a planet model
		/// </summary>
		private static Control CreatePlanetModelView( IWorkspace workspace )
		{
			SpherePlanetProject project = CurrentProject( workspace );
			ISpherePlanetModelTemplate template = ( ISpherePlanetModelTemplate )project.PlanetTemplate;
			ISpherePlanetModel model = ( ISpherePlanetModel )project.PlanetModel;

			SpherePlanetModelTemplateViewControl view = new SpherePlanetModelTemplateViewControl( );
			new SpherePlanetTemplateViewController( view, template, model );
			return view;
		}

		/// <summary>
		/// Creates a view for the current planet's terrain template and model
		/// </summary>
		private static Control CreateHomogenousProceduralTerrainView( IWorkspace workspace )
		{
			SpherePlanetProject project = CurrentProject( workspace );
			IPlanetHomogenousProceduralTerrainTemplate template = project.PlanetTemplate.GetModelTemplate<IPlanetHomogenousProceduralTerrainTemplate>( );
			if ( template == null )
			{
				return null;
			}

			IPlanetHomogenousProceduralTerrainModel model = project.PlanetModel.GetModel<IPlanetHomogenousProceduralTerrainModel>( );

			HomogenousProcTerrainTemplateControl control = new HomogenousProcTerrainTemplateControl( );
			new HomogenousProceduralTerrainViewController( control, template, model );

			return control;
		}

		/// <summary>
		/// Creates a cloud template model control
		/// </summary>
		private static Control CreateCloudView( IWorkspace workspace )
		{
			SpherePlanetProject project = CurrentProject( workspace );
			IPlanetSimpleCloudTemplate cloudModelTemplate = project.PlanetTemplate.GetModelTemplate<IPlanetSimpleCloudTemplate>( );
			if ( cloudModelTemplate == null )
			{
				//	Show empty view
				return null;
			}
			IPlanetSimpleCloudModel cloudModel = project.PlanetModel.GetModel<IPlanetSimpleCloudModel>( );
			FlatCloudModelTemplateControl view = new FlatCloudModelTemplateControl( );
			new CloudModelTemplateViewController( view, cloudModelTemplate, cloudModel );
			return view;
		}

		/// <summary>
		/// Creates a ring template model control
		/// </summary>
		private static Control CreateRingView( IWorkspace workspace )
		{
			SpherePlanetProject project = CurrentProject( workspace );
			IPlanetRingTemplate template = project.PlanetTemplate.GetModelTemplate<IPlanetRingTemplate>( );
			if ( template == null )
			{
				//	Show empty view
				return null;
			}
			PropertyGrid control = new PropertyGrid( );
			control.SelectedObject = template;
			return control;
		}

		/// <summary>
		/// Creates a property grid for a given template type
		/// </summary>
		private static Control CreateGenericTemplatePropertiesView<TTemplate>( IWorkspace workspace )
			where TTemplate : class, IPlanetEnvironmentModelTemplate
		{
			SpherePlanetProject project = CurrentProject( workspace );
			TTemplate template = project.PlanetTemplate.GetModelTemplate<TTemplate>( );
			if ( template == null )
			{
				//	Show empty view
				return null;
			}
			PropertyGrid control = new PropertyGrid( );
			control.SelectedObject = template;
			return control;
		}

		/// <summary>
		/// Creates an atmosphere template model control
		/// </summary>
		private static Control CreateAtmosphereScatteringView( IWorkspace workspace )
		{
			SpherePlanetProject project = CurrentProject( workspace );
			IPlanetAtmosphereScatteringTemplate template = project.PlanetTemplate.GetModelTemplate<IPlanetAtmosphereScatteringTemplate>( );
			if ( template == null )
			{
				//	Show empty view
				return null;
			}
			IPlanetAtmosphereScatteringModel model = project.PlanetModel.GetModel<IPlanetAtmosphereScatteringModel>( );
			ScatteringAtmosphereBuildControl control = new ScatteringAtmosphereBuildControl( );
			control.Template = template;
			control.Model = model;
			return control;
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
