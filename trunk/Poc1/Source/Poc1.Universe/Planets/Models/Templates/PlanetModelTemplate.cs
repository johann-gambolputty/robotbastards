
using System;
using System.Collections.Generic;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Rb.Core.Components.Generic;
using Rb.Core.Utils;

namespace Poc1.Universe.Planets.Models.Templates
{
	/// <summary>
	/// Base class implementation of <see cref="IPlanetModelTemplateBase"/>
	/// </summary>
	public abstract class PlanetModelTemplate : Composite<IPlanetEnvironmentModelTemplate>, IPlanetModelTemplate
	{
		#region IPlanetModelTemplate Members

		/// <summary>
		/// Gets/sets the list of environment model templates
		/// </summary>
		public List<IPlanetEnvironmentModelTemplate> EnvironmentModelTemplates
		{
			get { return m_EnvironmentModelTemplates; }
			set
			{
				Arguments.CheckNotNull( value, "value" );
				if ( value != m_EnvironmentModelTemplates )
				{

					m_EnvironmentModelTemplates = value;
					OnTemplateChanged( );
				}
			}
		}

		/// <summary>
		/// Gets a model template of the specified type
		/// </summary>
		/// <typeparam name="T">Model template type to retrieve</typeparam>
		/// <returns>Model template object, or null if no model template of type T exists in this planet template</returns>
		public T GetModelTemplate<T>( ) where T : class, IPlanetEnvironmentModelTemplate
		{
			foreach ( IPlanetEnvironmentModelTemplate template in m_EnvironmentModelTemplates )
			{
				if ( template is T )
				{
					return ( T )template;
				}
			}
			return null;
		}

		/// <summary>
		/// Creates an instance of a planet model from this template
		/// </summary>
		/// <param name="planetModel">Planet model to setup</param>
		/// <param name="context">Instanciation context</param>
		public void CreateModelInstance( IPlanetModel planetModel, ModelTemplateInstanceContext context )
		{
			BuildPlanetModel( planetModel, context );
		}

		#endregion

		#region IPlanetModelTemplateBase Members

		/// <summary>
		/// Event raised when this template is changed
		/// </summary>
		public event EventHandler TemplateChanged;

		#endregion

		#region Protected Members

		/// <summary>
		/// Builds a planet model
		/// </summary>
		protected virtual void BuildPlanetModel( IPlanetModel planetModel, ModelTemplateInstanceContext context )
		{
			Arguments.CheckNotNull( planetModel, "planetModel" );
			foreach ( IPlanetEnvironmentModelTemplate modelTemplate in EnvironmentModelTemplates )
			{
				modelTemplate.CreateInstance( planetModel, context );
			}
		}

		/// <summary>
		/// Raises the <see cref="TemplateChanged"/> event
		/// </summary>
		protected void OnTemplateChanged( )
		{
			OnTemplateChanged( EventArgs.Empty );
		}

		/// <summary>
		/// Raises the <see cref="TemplateChanged"/> event
		/// </summary>
		protected void OnTemplateChanged( EventArgs args )
		{
			if ( TemplateChanged != null )
			{
				TemplateChanged( this, args );
			}
		}

		#endregion

		#region Private Members

		private List<IPlanetEnvironmentModelTemplate> m_EnvironmentModelTemplates = new List<IPlanetEnvironmentModelTemplate>( );

		#endregion
	}
}
