using Bob.Core.Commands;
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
		}

		/// <summary>
		/// Stops listening for project commands
		/// </summary>
		public void StopListening( )
		{
			ProjectCommands.NewProject.CommandTriggered -= OnNewProject;
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

		#endregion
	}
}
