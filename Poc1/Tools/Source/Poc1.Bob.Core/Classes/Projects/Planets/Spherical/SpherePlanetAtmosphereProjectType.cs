
using Poc1.Bob.Core.Interfaces.Projects;

namespace Poc1.Bob.Core.Classes.Projects.Planets.Spherical
{
	/// <summary>
	/// Project type for spherical planet atmospheres
	/// </summary>
	public class SpherePlanetAtmosphereProjectType : ProjectType
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetAtmosphereProjectType( ) :
			base( null, "Atmosphere", "Spherical Planet Atmosphere Project Type" )
		{
		}

		/// <summary>
		/// Gets the extension used to store atmosphere template instances
		/// </summary>
		public override string SupportedExtension
		{
			get { return "sphere.atmosphere.xml"; }
		}

		/// <summary>
		/// Opens an atmosphere project
		/// </summary>
		/// <param name="streamLocation">Location of the stream</param>
		/// <param name="stream">Stream containing serialized atmosphere template instance</param>
		/// <returns>Returns a new template instance</returns>
		public override Project OpenProject( string streamLocation, System.IO.Stream stream )
		{
			throw new System.Exception( "The method or operation is not implemented." );
		}

		/// <summary>
		/// Creates a new atmosphere template instance
		/// </summary>
		/// <param name="name">Name of the new instance</param>
		/// <returns>Returns the new template instance</returns>
		public override Project CreateProject( string name )
		{
			SpherePlanetAtmosphereProjectType data = new SpherePlanetAtmosphereProjectType( );
			return new Project( this, name, data, null );
		}

	}
}
