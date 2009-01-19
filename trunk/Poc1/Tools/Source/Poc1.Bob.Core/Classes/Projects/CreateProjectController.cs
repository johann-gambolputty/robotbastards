using System;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Interfaces.Projects;
using Rb.Core.Utils;
using Rb.Log;

namespace Poc1.Bob.Core.Classes.Projects
{
	/// <summary>
	/// Controller class for <see cref="ICreateProjectView"/>
	/// </summary>
	public class CreateProjectController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="workspace">Workspace controller runs in</param>
		/// <param name="context">Project instance context</param>
		/// <param name="view">View to control</param>
		/// <param name="rootGroup">Project model to view</param>
		/// <exception cref="ArgumentNullException">Thrown if any argument is null</exception>
		public CreateProjectController( IWorkspace workspace, ProjectContext context, ICreateProjectView view, ProjectGroupContainer rootGroup )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			Arguments.CheckNotNull( context, "context" );
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( rootGroup, "rootGroup" );

			AppLog.Info( "Created {0}", GetType( ) );

			m_Workspace = workspace;
			m_Context = context;

			view.SelectionView.SelectionChanged += OnSelectionChanged;
			m_View = view;

			//	Create a controller for the selection view
			new ProjectTypeSelectorController( view.SelectionView, rootGroup );
		}

		/// <summary>
		/// Shows the attached view
		/// </summary>
		public void Show( )
		{
			//	Show the view as a modal dialog
			if ( !m_View.ShowView( ) )
			{
				return;
			}

			//	Create an instance of the selected template
			ProjectType selectedProjectType = m_View.SelectionView.SelectedProjectType;
			if ( selectedProjectType == null )
			{
				throw new InvalidOperationException( "Create project view in invalid state - user clicked OK but no template was selected" );
			}

			AppLog.Info( "Creating new project " + m_View.SelectionView.SelectedProjectNode );
			m_Context.SetInstance( m_Workspace, selectedProjectType.CreateProject( m_View.ProjectName ) );
		}

		#region Private Members

		private readonly IWorkspace m_Workspace;
		private readonly ProjectContext m_Context;
		private readonly ICreateProjectView m_View;

		/// <summary>
		/// Handles the <see cref="IProjectTypeSelectorView.SelectionChanged"/> event
		/// </summary>
		private void OnSelectionChanged( object sender, EventArgs args )
		{
			m_View.OkEnabled = ( m_View.SelectionView.SelectedProjectType != null );
		}

		#endregion
	}
}
