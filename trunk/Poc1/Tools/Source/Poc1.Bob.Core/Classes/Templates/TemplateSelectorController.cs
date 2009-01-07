using System;
using Poc1.Bob.Core.Interfaces.Templates;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Templates
{
	/// <summary>
	/// Template selection view controller
	/// </summary>
	public class TemplateSelectorController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">View</param>
		/// <param name="rootGroup">Root group (model)</param>
		/// <exception cref="ArgumentNullException">Thrown if view or root group is null</exception>
		public TemplateSelectorController( ITemplateSelectorView view, TemplateGroupContainer rootGroup )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( rootGroup, "rootGroup" );

			view.RootGroup = rootGroup;
			view.SelectionChanged += OnSelectionChanged;

			m_View = view;
		}

		#region Private Members

		private readonly ITemplateSelectorView m_View;

		/// <summary>
		/// Handles the event <see cref="ITemplateSelectorView.SelectionChanged"/>
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
			if ( m_View.SelectedTemplateBase != null )
			{
				m_View.Description = m_View.SelectedTemplateBase.Description;
			}
			else
			{
				m_View.Description = "";
			}	
		}

		#endregion
	}
}
