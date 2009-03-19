using System;
using Poc1.Bob.Core.Interfaces.Projects;
using Poc1.Tools.Waves;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Poc1.Universe.Interfaces.Planets.Renderers;
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
		/// <param name="modelFactory">Factory used for creating environment models from templates</param>
		/// <param name="renderFactory">Factory used for creating environment renderers from models</param>
		/// <param name="template">Template to use</param>
		/// <param name="planet">Planet to use</param>
		protected PlanetProject( ProjectType projectType, string name, IPlanetEnvironmentModelFactory modelFactory, IPlanetEnvironmentRendererFactory renderFactory, IPlanetModelTemplate template, IPlanet planet ) :
			base( projectType, name, null, null )
		{
			Arguments.CheckNotNull( template, "template" );
			Arguments.CheckNotNull( modelFactory, "modelFactory" );
			Arguments.CheckNotNull( renderFactory, "renderFactory" );
			Arguments.CheckNotNull( planet, "planet" );

			if ( planet.PlanetModel == null )
			{
				throw new ArgumentNullException( "planet", "Planet needs valid model" );
			}
			m_Planet = planet;
			m_PlanetTemplate = template;
			template.CreateModelInstance( planet.PlanetModel, modelFactory, InstanceContext );

			new PlanetTemplateWatcher( template, planet, modelFactory, renderFactory );
		}

		/// <summary>
		/// Gets the planet model instance
		/// </summary>
		public IPlanetModel PlanetModel
		{
			get { return m_Planet.PlanetModel; }
		}

		/// <summary>
		/// Gets the planet instance
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
		}

		/// <summary>
		/// Gets the instance context used for creating models from templates
		/// </summary>
		public ModelTemplateInstanceContext InstanceContext
		{
			get { return m_InstanceContext; }
		}

		#endregion

		#region Private Members

		private readonly ModelTemplateInstanceContext m_InstanceContext = new ModelTemplateInstanceContext( );
		private readonly IPlanetModelTemplate m_PlanetTemplate;
		private readonly IPlanet m_Planet;
		private readonly WaveAnimationParameters m_WaveAnimationModel = new WaveAnimationParameters( );

		#endregion
	}
}