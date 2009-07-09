using System;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Rb.Core.Components.Generic;
using Rb.Core.Utils;

namespace Poc1.Core.Classes.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Base class implementation of <see cref="IPlanetModelTemplate"/>
	/// </summary>
	public abstract class AbstractPlanetModelTemplate : Composite<IPlanetEnvironmentModelTemplate>, IPlanetModelTemplate
	{
		#region IPlanetModelTemplate Members

		/// <summary>
		/// Gets a model template of the specified type
		/// </summary>
		/// <typeparam name="T">Model template type to retrieve</typeparam>
		/// <returns>Model template object, or null if no model template of type T exists in this planet template</returns>
		public T GetModelTemplate<T>( ) where T : class, IPlanetEnvironmentModelTemplate
		{
			foreach ( IPlanetEnvironmentModelTemplate template in Components )
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
		/// <param name="modelFactory">Factory used to create models from templates</param>
		/// <param name="context">Instanciation context</param>
		public void CreateModelInstance( IPlanetModel planetModel, IPlanetEnvironmentModelFactory modelFactory, ModelTemplateInstanceContext context )
		{
			BuildPlanetModel( planetModel, modelFactory, context );
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
		protected virtual void BuildPlanetModel( IPlanetModel planetModel, IPlanetEnvironmentModelFactory modelFactory, ModelTemplateInstanceContext context )
		{
			Arguments.CheckNotNull( planetModel, "planetModel" );
			foreach ( IPlanetEnvironmentModelTemplate modelTemplate in Components )
			{
				IPlanetEnvironmentModel model = modelFactory.CreateModel( modelTemplate );
				model.PlanetModel = planetModel;
				modelTemplate.SetupInstance( model, context );
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
	}
}
