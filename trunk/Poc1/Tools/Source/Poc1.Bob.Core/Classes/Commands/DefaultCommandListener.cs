using Bob.Core.Commands;

namespace Poc1.Bob.Core.Classes.Commands
{
	public class DefaultCommandListener
	{
		/// <summary>
		/// Starts listening for default commands
		/// </summary>
		public void StartListening( )
		{
			DefaultCommands.FileExit.CommandTriggered += OnFileExit;
		}

		/// <summary>
		/// Stops listening for default commands
		/// </summary>
		public void StopListening( )
		{
			DefaultCommands.FileExit.CommandTriggered -= OnFileExit;	
		}

		#region Private Members

		private void OnFileExit( WorkspaceCommandTriggerData triggerData )
		{
			triggerData.Workspace.MainDisplay.Close( );
		}

		#endregion
	}
}
