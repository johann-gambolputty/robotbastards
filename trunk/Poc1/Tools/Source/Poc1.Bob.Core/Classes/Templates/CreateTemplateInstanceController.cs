using System;
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
		/// <param name="view">View to control</param>
		/// <param name="rootGroup">Template model to view</param>
		/// <exception cref="ArgumentNullException">Thrown if rootGroup or view are null</exception>
		public CreateTemplateInstanceController( ICreateTemplateInstanceView view, TemplateGroupContainer rootGroup )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( rootGroup, "rootGroup" );

			AppLog.Info( "Created {0}", GetType( ) );

			view.SelectionView.SelectionChanged += OnSelectionChanged;
			m_View = view;

			//	Create a controller for the selection view
			new TemplateSelectorController( view.SelectionView, rootGroup );

			//	Show the view as a modal dialog
			if ( view.ShowView( ) )
			{
				AppLog.Info( "Creating new template instance " + view.SelectionView.SelectedTemplateBase );
				//	Create an instance of the selected template
				if ( view.SelectionView.SelectedTemplate == null )
				{
					throw new InvalidOperationException( "Create template view in invalid state - user clicked OK but no template was selected" );
				}
			}
		}

		#region Private Members

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
