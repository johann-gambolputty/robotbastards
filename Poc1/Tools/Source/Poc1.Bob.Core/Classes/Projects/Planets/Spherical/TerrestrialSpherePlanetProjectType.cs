
using Poc1.Bob.Core.Interfaces.Projects;

namespace Poc1.Bob.Core.Classes.Projects.Planets.Spherical
{
	/// <summary>
	/// Sphere planet project type that creates terrestrial planets by default (planets with terrain, oceans
	/// clouds, etc.)
	/// </summary>
	public class TerrestrialSpherePlanetProjectType : SpherePlanetProjectType
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="views">Views supported by this project type</param>
		public TerrestrialSpherePlanetProjectType( IPlanetViews views ) :
			base( views, "Terrestrial Spherical Planet", "Creates a spherical planet with terrestrial planet characteristics")
		{
		}

		/// <summary>
		/// Creates a project
		/// </summary>
		public override Project CreateProject( string name )
		{
			return SpherePlanetProject.TerrestrialPlanetProject( this, name );
		}
	}
}
