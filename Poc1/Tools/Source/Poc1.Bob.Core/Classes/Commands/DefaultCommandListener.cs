using Bob.Core.Commands;
using Poc1.Bob.Core.Interfaces;

namespace Poc1.Bob.Core.Classes.Commands
{
	public class DefaultCommandListener
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="viewFactory"></param>
		public DefaultCommandListener( IViewFactory viewFactory )
		{
			m_ViewFactory = viewFactory;
		}

		/// <summary>
		/// Starts listening for default commands
		/// </summary>
		public void StartListening( )
		{
			DefaultCommands.FileExit.CommandTriggered += OnFileExit;
			DefaultCommands.ViewOutput.CommandTriggered += OnViewOutput;
		}

		/// <summary>
		/// Stops listening for default commands
		/// </summary>
		public void StopListening( )
		{
			DefaultCommands.FileExit.CommandTriggered -= OnFileExit;
			DefaultCommands.ViewOutput.CommandTriggered -= OnViewOutput;
		}

		#region Private Members

		private readonly IViewFactory m_ViewFactory;

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


		#endregion
	}
}
