using System;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Interfaces.Templates;
using Rb.Core.Utils;
using Rb.Log;

namespace Poc1.Bob.Core.Classes.Templates
{
	/// <summary>
	/// Controller class for <see cref="ICreateTemplateInstanceView"/>
	/// </summary>
	public class CreateTemplateInstanceController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="workspace">Workspace controller runs in</param>
		/// <param name="instanceContext">Template instance context</param>
		/// <param name="view">View to control</param>
		/// <param name="rootGroup">Template model to view</param>
		/// <exception cref="ArgumentNullException">Thrown if any argument is null</exception>
		public CreateTemplateInstanceController( IWorkspace workspace, TemplateInstanceContext instanceContext, ICreateTemplateInstanceView view, TemplateGroupContainer rootGroup )
		{
			Arguments.CheckNotNull( workspace, "workspace" );
			Arguments.CheckNotNull( instanceContext, "instanceContext" );
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( rootGroup, "rootGroup" );

			AppLog.Info( "Created {0}", GetType( ) );

			m_Workspace = workspace;
			m_InstanceContext = instanceContext;

			view.SelectionView.SelectionChanged += OnSelectionChanged;
			m_View = view;

			//	Create a controller for the selection view
			new TemplateSelectorController( view.SelectionView, rootGroup );
		}

		/// <summary>
		/// Shows the attached view
		/// </summary>
		public void Show( )
		{
			//	Show the view as a modal dialog
			if ( !m_View.ShowView( ) )
			{
				return;
			}

			//	Create an instance of the selected template
			Template selectedTemplate = m_View.SelectionView.SelectedTemplate;
			if ( selectedTemplate == null )
			{
				throw new InvalidOperationException( "Create template view in invalid state - user clicked OK but no template was selected" );
			}

			AppLog.Info( "Creating new template instance " + m_View.SelectionView.SelectedTemplateBase );
			m_InstanceContext.SetInstance( m_Workspace, selectedTemplate.CreateInstance( m_View.InstanceName ) );
		}

		#region Private Members

		private readonly IWorkspace m_Workspace;
		private readonly TemplateInstanceContext m_InstanceContext;
		private readonly ICreateTemplateInstanceView m_View;

		/// <summary>
		/// Handles the <see cref="ITemplateSelectorView.SelectionChanged"/> event
		/// </summary>
		private void OnSelectionChanged( object sender, EventArgs args )
		{
			m_View.OkEnabled = ( m_View.SelectionView.SelectedTemplate != null );
		}

		#endregion
	}
}
