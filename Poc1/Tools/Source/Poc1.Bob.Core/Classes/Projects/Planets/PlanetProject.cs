using Poc1.Bob.Core.Interfaces.Projects;
using Poc1.Tools.Waves;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Projects.Planets
{
	/// <summary>
	/// Planet project
	/// </summary>
	public abstract class PlanetProject : Project
	{
		/// <summary>
		/// Gets the planet template
		/// </summary>
		public IPlanetModelTemplate PlanetTemplate
		{
			get { return m_PlanetTemplate; }
		}

		/// <summary>
		/// Gets the wave animation model
		/// </summary>
		public WaveAnimationParameters WaveAnimationModel
		{
			get { return m_WaveAnimationModel; }
		}

		#region Protected Members
		
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="projectType">Project type that created this project</param>
		/// <param name="name">Project name</param>
		/// <param name="template">Template to use</param>
		/// <param name="planet">Planet to use</param>
		protected PlanetProject( ProjectType projectType, string name, IPlanetModelTemplate template, IPlanet planet ) :
			base( projectType, name, null, null )
		{
			Arguments.CheckNotNull( template, "template" );
			Arguments.CheckNotNull( planet, "planet" );
			m_Planet = planet;
			m_PlanetTemplate = template;
			m_PlanetModel = template.CreateModelInstance( planet, m_InstanceContext );
		}

		/// <summary>
		/// Gets the planet model instance
		/// </summary>
		public IPlanetModel PlanetModel
		{
			get { return m_PlanetModel; }
		}

		/// <summary>
		/// Gets the planet instance
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
		}

		#endregion

		#region Private Members

		private readonly ModelTemplateInstanceContext m_InstanceContext = new ModelTemplateInstanceContext( );
		private readonly IPlanetModelTemplate m_PlanetTemplate;
		private IPlanetModel m_PlanetModel;
		private readonly IPlanet m_Planet;
		private readonly WaveAnimationParameters m_WaveAnimationModel = new WaveAnimationParameters( );

		#endregion
	}
}