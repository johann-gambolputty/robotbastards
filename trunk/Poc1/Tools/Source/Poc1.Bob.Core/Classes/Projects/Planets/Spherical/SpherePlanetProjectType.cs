using System;
using System.IO;
using Poc1.Bob.Core.Interfaces.Projects;
using Poc1.Universe.Planets.Spherical.Models.Templates;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Projects.Planets.Spherical
{
	/// <summary>
	/// Sphere planet template
	/// </summary>
	public class SpherePlanetProjectType : ProjectType
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SpherePlanetProjectType( ) :
			base( "Sphere Planet", "Sphere Planet Template" )
		{
		}

		/// <summary>
		/// Gets the file extension used to store serialized instances of this template
		/// </summary>
		public override string SupportedExtension
		{
			get { return "sphere.planet.xml"; }
		}

		/// <summary>
		/// Opens an instance from a stream
		/// </summary>
		/// <param name="streamLocation">Location of the stream. Used to identify the stream in exceptions</param>
		/// <param name="stream">Stream containing serialized instance</param>
		/// <returns>Returns the new template instance</returns>
		/// <exception cref="System.ArgumentNullException">Thrown if stream is null</exception>
		public override Project OpenProject( string streamLocation, Stream stream )
		{
			Arguments.CheckNotNull( stream, "stream" );
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Creates an instance of this template
		/// </summary>
		/// <returns>Returns the new instance</returns>
		public override Project CreateProject( string name )
		{
			return new SpherePlanetProject( this, name );
		}
	}
}
