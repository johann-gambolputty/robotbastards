
namespace Poc1.Bob.Core.Interfaces.Projects
{
	/// <summary>
	/// Project creation view
	/// </summary>
	public interface ICreateProjectView
	{
		/// <summary>
		/// Gets the project type selection sub-view
		/// </summary>
		IProjectTypeSelectorView SelectionView
		{
			get;
		}

		/// <summary>
		/// Gets/sets whether or not the OK button is enabled
		/// </summary>
		bool OkEnabled
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the name of the new project
		/// </summary>
		string ProjectName
		{
			get; set;
		}

		/// <summary>
		/// Shows this view
		/// </summary>
		/// <returns>Returns true if the currently selected template should be instanced</returns>
		bool ShowView( );

	}
}
