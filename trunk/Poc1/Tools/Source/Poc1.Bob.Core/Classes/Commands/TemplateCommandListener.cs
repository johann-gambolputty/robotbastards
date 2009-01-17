using Bob.Core.Commands;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Templates;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Commands
{
	/// <summary>
	/// Template command listener
	/// </summary>
	public class TemplateCommandListener
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public TemplateCommandListener( IViewFactory factory, TemplateGroupContainer rootGroup )
		{
			Arguments.CheckNotNull( factory, "factory" );
			Arguments.CheckNotNull( rootGroup, "rootGroup" );
			m_Factory = factory;
			m_RootGroup = rootGroup;
		}

		/// <summary>
		/// Starts listening for template commands
		/// </summary>
		public void StartListening( )
		{
			TemplateCommands.NewFromTemplate.CommandTriggered += OnNewFromTemplate;
		}

		/// <summary>
		/// Stops listening for template commands
		/// </summary>
		public void StopListening( )
		{
			TemplateCommands.NewFromTemplate.CommandTriggered -= OnNewFromTemplate;
		}

		#region Private Members

		private readonly TemplateGroupContainer m_RootGroup;
		private readonly IViewFactory m_Factory;
		
		/// <summary>
		/// Handles the NewFromTemplate command
		/// </summary>
		private void OnNewFromTemplate( WorkspaceCommandTriggerData triggerData )
		{
			WorkspaceViewFactory.ShowCreateTemplateInstanceView( ( WorkspaceEx )triggerData.Workspace, m_Factory );
		}

		#endregion
	}
}
