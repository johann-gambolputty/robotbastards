
using System.Configuration;
using System.IO;

namespace Rb.Rendering
{
	/// <summary>
	/// Graphics initialization data.
	/// </summary>
	public class GraphicsInitialization
	{
		/// <summary>
		/// Creates initialization data from the application configuration file
		/// </summary>
		public static GraphicsInitialization FromAppConfig( )
		{
			//	Load the rendering assembly
			string renderAssemblyName = ConfigurationManager.AppSettings[ "renderAssembly" ];
			string platformAssemblyName = ConfigurationManager.AppSettings[ "renderPlatformAssembly" ];
			string effectsAssemblyName = ConfigurationManager.AppSettings[ "renderEffectsAssembly" ];

			if ( string.IsNullOrEmpty( renderAssemblyName ) )
			{
				throw new InvalidDataException( "Render assembly not specified in application configuration" );
			}

			return new GraphicsInitialization( renderAssemblyName, effectsAssemblyName, platformAssemblyName ); 
		}

		/// <summary>
		/// Creates graphics initialization data for OpenGl on windows
		/// </summary>
		public static GraphicsInitialization OpenGlCgWindows( )
		{
			GraphicsInitialization init = new GraphicsInitialization
				(
					"Rb.Rendering.OpenGl",
					"Rb.Rendering.OpenGl.Cg",
					"Rb.Rendering.OpenGl.Windows"
				);
			return init;
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public GraphicsInitialization( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public GraphicsInitialization( string renderAssembly, string effectsAssembly, string platformAssembly )
		{
			m_RenderingAssembly = renderAssembly;
			m_EffectsAssembly = effectsAssembly;
			m_PlatformAssembly = platformAssembly;
		}

		/// <summary>
		/// Gets/sets the name of the rendering assembly
		/// </summary>
		public string RenderingAssembly
		{
			get { return m_RenderingAssembly; }
			set { m_RenderingAssembly = value; }
		}

		/// <summary>
		///	Gets/sets the name of the platform assembly
		/// </summary>
		public string PlatformAssembly
		{
			get { return m_PlatformAssembly; }
			set { m_PlatformAssembly = value; }
		}

		/// <summary>
		///	Gets/sets the name of the effects assembly
		/// </summary>
		public string EffectsAssembly
		{
			get { return m_EffectsAssembly; }
			set { m_EffectsAssembly = value; }
		}

		#region Private Members

		private string	m_RenderingAssembly;
		private string	m_PlatformAssembly;
		private string	m_EffectsAssembly; 

		#endregion
	}
}
