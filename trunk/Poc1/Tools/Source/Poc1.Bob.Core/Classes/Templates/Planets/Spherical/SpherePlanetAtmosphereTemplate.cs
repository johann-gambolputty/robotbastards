
using Bob.Core.Ui;
using Poc1.Bob.Core.Interfaces.Templates;

namespace Poc1.Bob.Core.Classes.Templates.Planets.Spherical
{
	/// <summary>
	/// Template for spherical planet atmospheres
	/// </summary>
	public class SpherePlanetAtmosphereTemplate : Template
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetAtmosphereTemplate( ) :
			base( null, "Atmosphere", "Spherical Planet Atmosphere Template" )
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
		/// Opens an atmosphere template instance
		/// </summary>
		/// <param name="streamLocation">Location of the stream</param>
		/// <param name="stream">Stream containing serialized atmosphere template instance</param>
		/// <returns>Returns a new template instance</returns>
		public override TemplateInstance OpenInstance( string streamLocation, System.IO.Stream stream )
		{
			throw new System.Exception( "The method or operation is not implemented." );
		}

		/// <summary>
		/// Creates a new atmosphere template instance
		/// </summary>
		/// <param name="name">Name of the new instance</param>
		/// <returns>Returns the new template instance</returns>
		public override TemplateInstance CreateInstance( string name )
		{
			SpherePlanetAtmosphereTemplate data = new SpherePlanetAtmosphereTemplate( );
			return new TemplateInstance( this, name, data, null );
		}

	}
}
