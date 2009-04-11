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
		/// <exception cref="System.ArgumentNullException">Thrown if mainDisplay is null</exception>
		public WorkspaceEx( IMainApplicationDisplay mainDisplay ) :
			base( mainDisplay )
		{
			m_ProjectContext = new ProjectContext( );
			m_SelectedBiomeContext = new SelectedBiomeContext( );
			m_ProjectGroups = new ProjectGroupContainer( );
		}

		/// <summary>
		/// Gets/sets the project groups
		/// </summary>
		public ProjectGroupContainer ProjectGroups
		{
			get { return m_ProjectGroups; }
			set
			{
				Arguments.CheckNotNull( value, "value" );
				m_ProjectGroups = value;
			}
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

		#region Private Members

		private ProjectGroupContainer m_ProjectGroups;
		private readonly ProjectContext m_ProjectContext;
		private readonly SelectedBiomeContext m_SelectedBiomeContext;

		#endregion

	}
}
