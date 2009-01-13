
using Bob.Core.Workspaces.Interfaces;
using Rb.Core.Utils;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;

namespace Bob.Core.Commands
{
	/// <summary>
	/// Creates <see cref="WorkspaceCommandTriggerData"/> objects
	/// </summary>
	public class WorkspaceCommandTriggerDataFactory : ICommandTriggerDataFactory
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="workspace">Workspace to set in command trigger data objects</param>
		/// <exception cref="System.ArgumentNullException">Thrown if workspace is null</exception>
		public WorkspaceCommandTriggerDataFactory( IWorkspace workspace )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			m_Workspace = workspace; 
		}

		#region ICommandTriggerDataFactory Members

		/// <summary>
		/// Creates a <see cref="WorkspaceCommandTriggerData"/> object
		/// </summary>
		public CommandTriggerData Create( ICommandUser user, Command command, ICommandInputState inputState )
		{
			return new WorkspaceCommandTriggerData( m_Workspace, user, command, inputState );
		}

		#endregion

		#region Private Members

		private readonly IWorkspace m_Workspace;

		#endregion
	}
}
