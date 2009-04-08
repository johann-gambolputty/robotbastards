
using System.Collections.Generic;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
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

			m_PlanetTemplate.ComponentAdded += OnTemplateAdded;
			m_PlanetTemplate.ComponentRemoved += OnTemplateRemoved;

			m_Planet.Model.ComponentAdded += OnModelAdded;
			m_Planet.Model.ComponentRemoved += OnModelRemoved;
		}

		#region Private Members

		private readonly IPlanetModelTemplate m_PlanetTemplate;
		private readonly IPlanet m_Planet;
		private readonly ModelTemplateInstanceContext m_Context = new ModelTemplateInstanceContext( );
		private readonly IPlanetEnvironmentModelFactory m_ModelFactory;
		private readonly IPlanetEnvironmentRendererFactory m_RendererFactory;
		private readonly Dictionary<IPlanetEnvironmentModelTemplate, IPlanetEnvironmentModel> m_ModelMap = new Dictionary<IPlanetEnvironmentModelTemplate, IPlanetEnvironmentModel>( );
		private readonly Dictionary<IPlanetEnvironmentModel, IPlanetEnvironmentRenderer[]> m_RendererMap = new Dictionary<IPlanetEnvironmentModel, IPlanetEnvironmentRenderer[]>( );

		/// <summary>
		/// Called when a model is added to a model composite
		/// </summary>
		private void OnModelAdded( IComposite<IPlanetEnvironmentModel> composite, IPlanetEnvironmentModel component )
		{
			IPlanetEnvironmentRenderer[] renderers = m_RendererFactory.CreateModelRenderer( component );
			if ( renderers == null )
			{
				return;
			}
			foreach ( IPlanetEnvironmentRenderer renderer in renderers )
			{
				renderer.PlanetRenderer = m_Planet.Renderer;
			}
			m_RendererMap.Add( component, renderers );
		}

		/// <summary>
		/// Called when a model is removed from a model composite
		/// </summary>
		private void OnModelRemoved( IComposite<IPlanetEnvironmentModel> composite, IPlanetEnvironmentModel component )
		{
			IPlanetEnvironmentRenderer[] renderers;
			if ( !m_RendererMap.TryGetValue( component, out renderers ) )
			{
				return;
			}
			foreach ( IPlanetEnvironmentRenderer renderer in renderers )
			{
				renderer.PlanetRenderer = null;
			}
			m_RendererMap.Remove( component );
		}

		/// <summary>
		/// Called when an environment model template is added to a planet template
		/// </summary>
		private void OnTemplateAdded( IComposite<IPlanetEnvironmentModelTemplate> composite, IPlanetEnvironmentModelTemplate component )
		{
			IPlanetEnvironmentModel model = m_ModelFactory.CreateModel( component );
			if ( model == null )
			{
				return;
			}
			model.PlanetModel = m_Planet.Model;
			component.SetupInstance( model, m_Context );
			m_ModelMap.Add( component, model );

		}

		/// <summary>
		/// Called when an environment model template is removed from a planet template
		/// </summary>
		private void OnTemplateRemoved( IComposite<IPlanetEnvironmentModelTemplate> composite, IPlanetEnvironmentModelTemplate component )
		{
			IPlanetEnvironmentModel model;
			if ( !m_ModelMap.TryGetValue( component, out model ) )
			{
				return;
			}
			model.PlanetModel = null;
		}
		
		#endregion
	}
}
