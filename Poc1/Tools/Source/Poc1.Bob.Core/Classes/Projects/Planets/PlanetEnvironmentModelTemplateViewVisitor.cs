using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Core.Classes.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Projects.Planets
{
	/// <summary>
	/// Creates docking views for planet environment templates
	/// </summary>
	public class PlanetEnvironmentModelTemplateViewVisitor : AbstractPlanetEnvironmentModelTemplateVisitor<bool>
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public PlanetEnvironmentModelTemplateViewVisitor( IWorkspace workspace, IViewManager viewManager, IPlanetViews views )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			Arguments.CheckNotNull( viewManager, "viewManager" );
			Arguments.CheckNotNull( views, "views" );
			m_Workspace = workspace;
			m_ViewManager = viewManager;
			m_Views = views;
		}

		/// <summary>
		/// Visits a model template that has no explicitly-typed support in this visitor
		/// </summary>
		/// <param name="modelTemplate">Model template to visit</param>
		public override bool Visit( IPlanetEnvironmentModelTemplate modelTemplate )
		{
			//	Does nothing (no view available for this type)
			return false;
		}

		/// <summary>
		/// Visits a cloud model template
		/// </summary>
		/// <param name="cloudModelTemplate">Model template to visit</param>
		public override bool Visit( IPlanetSimpleCloudTemplate cloudModelTemplate )
		{
			m_ViewManager.Show( m_Workspace, m_Views.CloudView );
			return true;
		}

		#region Private Members

		private readonly IWorkspace m_Workspace;
		private readonly IPlanetViews m_Views;
		private readonly IViewManager m_ViewManager;

		#endregion
	}
}
