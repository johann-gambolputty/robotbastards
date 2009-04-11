using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Interfaces.Projects;
using Rb.Core.Sets.Interfaces;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Projects
{
	/// <summary>
	/// ProjectType instance context
	/// </summary>
	public class ProjectContext : IObjectSetService
	{
		/// <summary>
		/// Gets the currently selected template instance
		/// </summary>
		public Project CurrentProject
		{
			get { return m_Instance; }
		}

		/// <summary>
		/// Sets the current instance
		/// </summary>
		/// <param name="workspace">Workspace</param>
		/// <param name="instance">New instance. Can be null</param>
		public void SetCurrentProject( IWorkspace workspace, Project instance )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			m_Instance = instance;
			LinkCurrentProjectContext( workspace );
		}

		#region Private Members

		private Project m_Instance;

		/// <summary>
		/// Links the current template instance (adds commands to UI)
		/// </summary>
		private void LinkCurrentProjectContext( IWorkspace workspace )
		{
			if ( m_Instance == null )
			{
				return;
			}

		//	List<Command> viewCommands = GetDisplayCommandListFromViews( m_Instance.ProjectType.Views );
		//	workspace.MainDisplay.CommandUi.AddCommands( viewCommands );
			workspace.MainDisplay.Views.BeginLayout( workspace, m_Instance.ProjectType.Name, m_Instance.ProjectType.Views );
		}

		#endregion
	}
}
