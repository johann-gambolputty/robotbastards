
using Bob.Core.Controls.Interfaces;
using Bob.Core.Projects;

namespace Bob.Core.Controls
{
	/// <summary>
	/// View factory
	/// </summary>
	public interface IViewFactory
	{
		/// <summary>
		/// Creates a new project view
		/// </summary>
		/// <param name="projectTypes">Project type array to select from</param>
		INewProjectView CreateNewProjectView( ProjectType[] projectTypes );
	}
}
