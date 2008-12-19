using Bob.Core.Controls.Interfaces;
using Bob.Core.Projects;
using Rb.Core.Utils;

namespace Bob.Core.Controls.Classes
{


	/// <summary>
	/// New project view controller
	/// </summary>
	public class NewProjectViewController
	{
		/// <summary>
		/// Setup controller
		/// </summary>
		/// <param name="mainApp">Main application window</param>
		/// <param name="view">New project view</param>
		/// <param name="projectTypes">Project type list</param>
		public NewProjectViewController( IMainApplicationWindow mainApp, INewProjectView view, ProjectType[] projectTypes )
		{
			Arguments.CheckNotNull( mainApp, "mainApp" );
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( projectTypes, "projectTypes" );

			view.OkClicked += OnOk;
			view.CancelClicked += OnCancel;
			view.ProjectTypes = projectTypes;
			view.SelectedProjectType = projectTypes.Length == 0 ? null : projectTypes[ 0 ];
			view.OkEnabled = projectTypes.Length > 0;

			m_View = view;
		}

		#region Private Members

		private readonly INewProjectView m_View;
		private readonly IMainApplicationWindow m_MainApp;

		private void OnOk( )
		{
			m_View.Close( );
		}

		private void OnCancel( )
		{
			m_View.Close( );
		}
		
		#endregion
	}
}
