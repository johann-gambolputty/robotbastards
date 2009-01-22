using Bob.Core.Commands;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Interfaces;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Commands
{
	/// <summary>
	/// Project command listener
	/// </summary>
	public class ProjectCommandListener
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public ProjectCommandListener( IViewFactory factory )
		{
			Arguments.CheckNotNull( factory, "factory" );
			m_Factory = factory;
		}

		/// <summary>
		/// Starts listening for project commands
		/// </summary>
		public void StartListening( )
		{
			ProjectCommands.NewProject.CommandTriggered += OnNewProject;
			ProjectCommands.CloseProject.CommandTriggered += null;
		}

		/// <summary>
		/// Stops listening for project commands
		/// </summary>
		public void StopListening( )
		{
			ProjectCommands.NewProject.CommandTriggered -= OnNewProject;
			ProjectCommands.CloseProject.CommandTriggered -= null;
		}

		#region Private Members

		private readonly IViewFactory m_Factory;
		
		/// <summary>
		/// Handles the NewProject command
		/// </summary>
		private void OnNewProject( WorkspaceCommandTriggerData triggerData )
		{
			WorkspaceViewFactory.ShowCreateProjectView( ( WorkspaceEx )triggerData.Workspace, m_Factory );
		}

		/// <summary>
		/// Handles the CloseProject command
		/// </summary>
		private void OnCloseProject( WorkspaceCommandTriggerData triggerData )
		{
			//	TODO: AP: Need to remove the commands from the UI also
			IWorkspace workspace = triggerData.Workspace;
			workspace.MainDisplay.Views.EndLayout( workspace );
		}

		#endregion
	}
}
