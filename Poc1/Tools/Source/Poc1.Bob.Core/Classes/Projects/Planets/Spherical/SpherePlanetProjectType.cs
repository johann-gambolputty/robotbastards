using System;
using System.IO;
using Bob.Core.Ui.Interfaces.Views;
using Poc1.Bob.Core.Interfaces.Projects;
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
		/// <param name="views">Planet view provider</param>
		public SpherePlanetProjectType( IPlanetViews views ) :
			this( views, "Sphere Planet", "Sphere Planet Project Type" )
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
		/// Gets views associated with this project type
		/// </summary>
		public override IViewInfo[] Views
		{
			get { return m_Views.Views; }
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

		#region Protected Members

		protected SpherePlanetProjectType( IPlanetViews views, string name, string description ) :
			base( name, description )
		{
			Arguments.CheckNotNull( views, "views" );
			m_Views = views;
		}


		#endregion

		#region Private Members

		private readonly IPlanetViews m_Views;

		#endregion
	}
}
