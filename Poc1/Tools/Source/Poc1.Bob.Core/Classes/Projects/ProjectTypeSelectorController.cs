using System;
using Poc1.Bob.Core.Interfaces.Projects;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Projects
{
	/// <summary>
	/// ProjectType selection view controller
	/// </summary>
	public class ProjectTypeSelectorController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">Selector view</param>
		/// <param name="rootGroup">Root group (model)</param>
		/// <exception cref="ArgumentNullException">Thrown if view or root group is null</exception>
		public ProjectTypeSelectorController( IProjectTypeSelectorView view, ProjectGroupContainer rootGroup )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( rootGroup, "rootGroup" );

			view.RootGroup = rootGroup;
			view.SelectionChanged += OnSelectionChanged;

			m_View = view;
		}

		#region Private Members

		private readonly IProjectTypeSelectorView m_View;

		/// <summary>
		/// Handles the event <see cref="IProjectTypeSelectorView.SelectionChanged"/>
		/// </summary>
		private void OnSelectionChanged( object sender, EventArgs args )
		{
			RefreshViewProperties( );
		}

		/// <summary>
		/// Refreshes view properies
		/// </summary>
		private void RefreshViewProperties( )
		{
			if ( m_View.SelectedProjectNode != null )
			{
				m_View.Description = m_View.SelectedProjectNode.Description;
			}
			else
			{
				m_View.Description = "";
			}	
		}

		#endregion
	}
}
