using Bob.Core.Ui.Interfaces;
using Bob.Core.Workspaces.Classes;
using Poc1.Bob.Core.Classes.Projects;
using Poc1.Bob.Core.Interfaces.Projects;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes
{
	/// <summary>
	/// Extended workspace
	/// </summary>
	public class WorkspaceEx : Workspace
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="mainDisplay">Main application display</param>
		/// <param name="rootGroup">Root template group</param>
		/// <exception cref="System.ArgumentNullException">Thrown if mainDisplay or rootGroup are null</exception>
		public WorkspaceEx( IMainApplicationDisplay mainDisplay, ProjectGroupContainer rootGroup ) :
			base( mainDisplay )
		{
			Arguments.CheckNotNull( rootGroup, "rootGroup" );
			m_ProjectContext = new ProjectContext( );
			m_SelectedBiomeContext = new SelectedBiomeContext( );
			m_TemplateRootGroup = rootGroup;
		}

		/// <summary>
		/// Gets the template instance context
		/// </summary>
		public ProjectContext ProjectContext
		{
			get { return m_ProjectContext; }
		}

		/// <summary>
		/// Gets the selected biome context
		/// </summary>
		public SelectedBiomeContext SelectedBiomeContext
		{
			get { return m_SelectedBiomeContext; }
		}

		/// <summary>
		/// Gets the template root group container
		/// </summary>
		public ProjectGroupContainer TemplateRootGroup
		{
			get { return m_TemplateRootGroup; }
		}

		#region Private Members

		private readonly ProjectContext m_ProjectContext;
		private readonly SelectedBiomeContext m_SelectedBiomeContext;
		private readonly ProjectGroupContainer m_TemplateRootGroup;

		#endregion

	}
}
