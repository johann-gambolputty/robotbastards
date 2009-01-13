using System;
using System.IO;
using Poc1.Bob.Core.Interfaces.Templates;
using Poc1.Universe.Planets.Spherical.Models.Templates;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Templates.Planets.Spherical
{
	/// <summary>
	/// Sphere planet template
	/// </summary>
	public class SpherePlanetTemplate : Template
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SpherePlanetTemplate( ) :
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
		public override TemplateInstance OpenInstance( string streamLocation, Stream stream )
		{
			Arguments.CheckNotNull( stream, "stream" );
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Creates an instance of this template
		/// </summary>
		/// <returns>Returns the new instance</returns>
		public override TemplateInstance CreateInstance( string name )
		{
			SpherePlanetModelTemplate data = new SpherePlanetModelTemplate( );
			return new TemplateInstance( this, name, data, null );
		}
	}
}
