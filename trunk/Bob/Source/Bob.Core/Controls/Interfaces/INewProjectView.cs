
using Bob.Core.Projects;
using Rb.Core.Utils;

namespace Bob.Core.Controls.Interfaces
{
	/// <summary>
	/// View for creating a new project
	/// </summary>
	public interface INewProjectView
	{
		/// <summary>
		/// Event raised when the user clicks OK
		/// </summary>
		event ActionDelegates.Action OkClicked;

		/// <summary>
		/// Event raised when the user clicks Cancel
		/// </summary>
		event ActionDelegates.Action CancelClicked;

		/// <summary>
		/// Get/sets the project type list
		/// </summary>
		ProjectType[] ProjectTypes
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the currently selected project type. Value of null indicates that there is no project type selected
		/// </summary>
		ProjectType SelectedProjectType
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the OK button enabled state
		/// </summary>
		bool OkEnabled
		{
			get; set;
		}

		/// <summary>
		/// Closes this view
		/// </summary>
		void Close( );
	}
}
