using Bob.Core.Workspaces.Interfaces;
using Rb.Core.Utils;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;

namespace Bob.Core.Commands
{
	/// <summary>
	/// Command trigger that contains a reference to the workspace that the command
	/// was executed in
	/// </summary>
	public class WorkspaceCommandTriggerData : CommandTriggerData
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="workspace">Workspace that the command was triggered in</param>
		/// <param name="user">User that triggered the command</param>
		/// <param name="command">Command that was triggered</param>
		/// <param name="inputState">Command input state</param>
		public WorkspaceCommandTriggerData( IWorkspace workspace, ICommandUser user, Command command, ICommandInputState inputState ) :
			base( user, command, inputState )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			m_Workspace = workspace;
		}

		/// <summary>
		/// Gets the workspace
		/// </summary>
		public IWorkspace Workspace
		{
			get { return m_Workspace; }
		}

		#region Private Members

		private readonly IWorkspace m_Workspace;

		#endregion
	}
}
