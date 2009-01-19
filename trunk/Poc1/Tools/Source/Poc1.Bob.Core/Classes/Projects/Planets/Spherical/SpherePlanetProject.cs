using Poc1.Bob.Core.Interfaces.Projects;
using Poc1.Universe.Planets.Spherical.Models.Templates;

namespace Poc1.Bob.Core.Classes.Projects.Planets.Spherical
{
	/// <summary>
	/// Sphere planet project instance
	/// </summary>
	public class SpherePlanetProject : Project
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="projectType">Project type that created this project</param>
		/// <param name="name">Project name</param>
		public SpherePlanetProject( SpherePlanetProjectType projectType, string name ) :
			base( projectType, name, null, null )
		{
		}

		#region Private Members

		private SpherePlanetModelTemplate m_PlanetTemplate = new SpherePlanetModelTemplate( );

		#endregion
	}
}
