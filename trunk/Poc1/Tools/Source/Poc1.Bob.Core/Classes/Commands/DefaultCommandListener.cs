using Bob.Core.Commands;
using Poc1.Bob.Core.Interfaces.Commands;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Commands
{
	public class DefaultCommandListener
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public DefaultCommandListener( ICommandViewFactory commandViews )
		{
			Arguments.CheckNotNull( commandViews, "commandViews" );
			m_CommandViews = commandViews;
		}

		/// <summary>
		/// Starts listening for default commands
		/// </summary>
		public void StartListening( )
		{
			DefaultCommands.FileExit.CommandTriggered += OnFileExit;
			DefaultCommands.ViewOutput.CommandTriggered += OnViewOutput;
			DefaultCommands.ViewRenderingRenderTargets.CommandTriggered += OnViewRenderingRenderTargets;
		}

		/// <summary>
		/// Stops listening for default commands
		/// </summary>
		public void StopListening( )
		{
			DefaultCommands.FileExit.CommandTriggered -= OnFileExit;
			DefaultCommands.ViewOutput.CommandTriggered -= OnViewOutput;
			DefaultCommands.ViewRenderingRenderTargets.CommandTriggered -= OnViewRenderingRenderTargets;
		}

		#region Private Members

		private readonly ICommandViewFactory m_CommandViews;

		/// <summary>
		/// Handles the FileExist command trigger
		/// </summary>
		private static void OnFileExit( WorkspaceCommandTriggerData triggerData )
		{
			triggerData.Workspace.MainDisplay.Close( );
		}

		/// <summary>
		/// Handles the ViewOutput command trigger
		/// </summary>
		private static void OnViewOutput( WorkspaceCommandTriggerData triggerData )
		{
		}

		/// <summary>
		/// Handles the ViewRenderingRenderTargets command trigger
		/// </summary>
		private void OnViewRenderingRenderTargets( WorkspaceCommandTriggerData triggerData )
		{
			m_CommandViews.ShowRenderTargetViews( triggerData.Workspace );
		//	triggerData.Workspace.MainDisplay.Views.Show( triggerData.Workspace, );
		}


		#endregion
	}
}
