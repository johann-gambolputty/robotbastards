using Bob.Core.Ui.Interfaces;
using Bob.Core.Workspaces.Classes;
using Poc1.Bob.Core.Classes.Templates;
using Poc1.Bob.Core.Interfaces.Templates;
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
		public WorkspaceEx( IMainApplicationDisplay mainDisplay, TemplateGroupContainer rootGroup ) :
			base( mainDisplay )
		{
			Arguments.CheckNotNull( rootGroup, "rootGroup" );
			m_TemplateInstanceContext = new TemplateInstanceContext( );
			m_SelectedBiomeContext = new SelectedBiomeContext( );
			m_TemplateRootGroup = rootGroup;
		}

		/// <summary>
		/// Gets the template instance context
		/// </summary>
		public TemplateInstanceContext TemplateInstanceContext
		{
			get { return m_TemplateInstanceContext; }
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
		public TemplateGroupContainer TemplateRootGroup
		{
			get { return m_TemplateRootGroup; }
		}

		#region Private Members

		private readonly TemplateInstanceContext m_TemplateInstanceContext;
		private readonly SelectedBiomeContext m_SelectedBiomeContext;
		private readonly TemplateGroupContainer m_TemplateRootGroup;

		#endregion

	}
}
