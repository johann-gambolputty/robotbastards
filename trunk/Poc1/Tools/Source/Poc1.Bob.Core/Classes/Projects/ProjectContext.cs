
using System.Collections.Generic;
using Bob.Core.Commands;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes.Commands;
using Poc1.Bob.Core.Interfaces.Projects;
using Rb.Core.Sets.Interfaces;
using Rb.Core.Utils;
using Rb.Interaction.Classes;
using Rb.Log;

namespace Poc1.Bob.Core.Classes.Projects
{
	/// <summary>
	/// Template instance context
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
			UnlinkCurrentTemplate( workspace );
			m_Instance = instance;
			LinkCurrentTemplate( workspace );
		}

		#region Private Members

		private Project m_Instance;
		private Dictionary<IViewInfo, Command> m_DisplayCommands = new Dictionary<IViewInfo, Command>( );

		/// <summary>
		/// Unlinks the current template instance (removes commands from UI)
		/// </summary>
		private void UnlinkCurrentTemplate( IWorkspace workspace )
		{
			if ( m_Instance == null )
			{
				return;
			}
			List<Command> viewCommands = GetDisplayCommandListFromViews( m_Instance.Template.Views );
			workspace.MainDisplay.CommandUi.RemoveCommands( viewCommands );
		}

		/// <summary>
		/// Links the current template instance (adds commands to UI)
		/// </summary>
		private void LinkCurrentTemplate( IWorkspace workspace )
		{
			if ( m_Instance == null )
			{
				return;
			}

			List<Command> viewCommands = GetDisplayCommandListFromViews( m_Instance.Template.Views );
			workspace.MainDisplay.CommandUi.AddCommands( viewCommands );
			workspace.MainDisplay.Views.BeginLayout( workspace, m_Instance.Template.Name, m_Instance.Template.Views );
		}

		/// <summary>
		/// Gets a list of commands supported by a set of views
		/// </summary>
		private List<Command> GetDisplayCommandListFromViews( IEnumerable<IViewInfo> views )
		{
			List<Command> viewCommands = new List<Command>( );
			foreach ( IViewInfo view in views )
			{
				if ( !view.AvailableAsCommand )
				{
					continue;
				}
				Command command;
				if ( !m_DisplayCommands.TryGetValue( view, out command ) )
				{
					IViewInfo commandView = view;
					command = new Command( view.Name, view.Name, view.Name );
					command.CommandTriggered +=
						delegate( CommandTriggerData triggerData )
						{
							OnViewCommandTriggered( commandView, triggerData );
						};

					DefaultCommands.ViewCommands.AddCommand( command );
					m_DisplayCommands.Add( view, command );
				}

				viewCommands.Add( command );
			}
			return viewCommands;
		}

		/// <summary>
		/// Handles a view command triggering
		/// </summary>
		private static void OnViewCommandTriggered( IViewInfo view, CommandTriggerData triggerData )
		{
			WorkspaceCommandTriggerData workspaceTriggerData = Arguments.CheckedNonNullCast <WorkspaceCommandTriggerData>( triggerData, "triggerData" );
			AppLog.Info( "\"{0}\" view command triggered", triggerData.Command.NameUi );

			IViewManager views = workspaceTriggerData.Workspace.MainDisplay.Views;
			views.Show( workspaceTriggerData.Workspace, view );
		}

		#endregion
	}
}
