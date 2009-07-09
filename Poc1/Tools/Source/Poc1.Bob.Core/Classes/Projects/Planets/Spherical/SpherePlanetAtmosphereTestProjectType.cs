
using Poc1.Bob.Core.Interfaces.Projects;
using Poc1.Core.Classes.Astronomical.Planets.Models.Templates;
using Poc1.Core.Classes.Astronomical.Planets.Spherical.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;

namespace Poc1.Bob.Core.Classes.Projects.Planets.Spherical
{
	/// <summary>
	/// Atmosphere test project
	/// </summary>
	public class SpherePlanetAtmosphereTestProjectType : SpherePlanetProjectType
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="views">Views supported by this project type</param>
		public SpherePlanetAtmosphereTestProjectType( IPlanetViews views ) :
			base( views, "Spherical Planet Atmosphere Test", "Creates a spherical planet terrain and an atmosphere" )
		{
		}

		/// <summary>
		/// Creates a project
		/// </summary>
		public override Project CreateProject( string name )
		{
			ISpherePlanetModelTemplate template = new SpherePlanetModelTemplate( );
		//	template.Add( new PlanetHomogenousProceduralTerrainTemplate( ) );
			template.Add( new PlanetAtmosphereScatteringTemplate( ) );

			ISpherePlanet planet = new SpherePlanet( );

			return new SpherePlanetProject( this, name, template, planet );
		}
	}
}
