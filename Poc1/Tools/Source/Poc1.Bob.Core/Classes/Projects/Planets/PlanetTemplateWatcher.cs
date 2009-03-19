
using System.Collections.Generic;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Core.Components.Generic;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Projects.Planets
{
	/// <summary>
	/// Watches changes in a planet template. Adds/removes models and 
	/// </summary>
	public class PlanetTemplateWatcher
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public PlanetTemplateWatcher( IPlanetModelTemplate planetTemplate, IPlanet planet, IPlanetEnvironmentModelFactory modelFactory, IPlanetEnvironmentRendererFactory rendererFactory )
		{
			Arguments.CheckNotNull( planetTemplate, "planetTemplate" );
			Arguments.CheckNotNull( planet, "planet" );
			m_ModelFactory = modelFactory;
			m_RendererFactory = rendererFactory;
			m_PlanetTemplate = planetTemplate;
			m_Planet = planet;

			m_PlanetTemplate.ComponentAdded += OnComponentAdded;
			m_PlanetTemplate.ComponentRemoved += OnComponentRemoved;
		}

		#region Private Members

		private readonly IPlanetModelTemplate m_PlanetTemplate;
		private readonly IPlanet m_Planet;
		private readonly ModelTemplateInstanceContext m_Context = new ModelTemplateInstanceContext( );
		private readonly IPlanetEnvironmentModelFactory m_ModelFactory;
		private readonly IPlanetEnvironmentRendererFactory m_RendererFactory;
		private readonly Dictionary<IPlanetEnvironmentModelTemplate, IPlanetEnvironmentModel> m_ModelMap = new Dictionary<IPlanetEnvironmentModelTemplate, IPlanetEnvironmentModel>( );
		private readonly Dictionary<IPlanetEnvironmentModel, IPlanetEnvironmentRenderer> m_RendererMap = new Dictionary<IPlanetEnvironmentModel, IPlanetEnvironmentRenderer>( );

		/// <summary>
		/// Called when an environment model template is added to a planet template
		/// </summary>
		private void OnComponentAdded( IComposite<IPlanetEnvironmentModelTemplate> composite, IPlanetEnvironmentModelTemplate component )
		{
			IPlanetEnvironmentModel model = m_ModelFactory.CreateModel( component );
			if ( model == null )
			{
				return;
			}
			component.SetupInstance( model, m_Context );
			model.PlanetModel = m_Planet.PlanetModel;
			m_ModelMap.Add( component, model );

			IPlanetEnvironmentRenderer renderer = m_RendererFactory.CreateModelRenderer( model );
			if ( renderer == null )
			{
				return;
			}
			
			renderer.PlanetRenderer = m_Planet.PlanetRenderer;
			m_RendererMap.Add( model, renderer );
		}

		/// <summary>
		/// Called when an environment model template is removed from a planet template
		/// </summary>
		private void OnComponentRemoved( IComposite<IPlanetEnvironmentModelTemplate> composite, IPlanetEnvironmentModelTemplate component )
		{
			IPlanetEnvironmentModel model;
			if ( !m_ModelMap.TryGetValue( component, out model ) )
			{
				return;
			}
			model.PlanetModel = null;
			IPlanetEnvironmentRenderer renderer;
			if ( !m_RendererMap.TryGetValue( model, out renderer ) )
			{
				return;
			}
			renderer.PlanetRenderer = null;
		}
		
		#endregion
	}
}
