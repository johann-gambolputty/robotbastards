using System;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Rb.Core.Components;
using Rb.Core.Components.Generic;
using Rb.Core.Utils;

namespace Poc1.Core.Classes.Astronomical.Planets
{
	/// <summary>
	/// Abstract planet model
	/// </summary>
	public class AbstractPlanetModel : Composite<IPlanetEnvironmentModel>, IPlanetModel
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="planet">Planet that this model is attached to</param>
		public AbstractPlanetModel( IPlanet planet )
		{
			Arguments.CheckNotNull( planet, "planet" );
			m_Planet = planet;
		}

		#region IPlanetModel Members

		/// <summary>
		/// Event raised when a planet model changes
		/// </summary>
		public event EventHandler<ModelChangedEventArgs> ModelChanged;

		/// <summary>
		/// Gets a typed model from this composite
		/// </summary>
		/// <typeparam name="TModel">Model type to retrieve</typeparam>
		/// <returns>Returns the first instance of type TModel in this composite, or null if none exist.</returns>
		public TModel GetModel<TModel>( )
		{
			return CompositeUtils.GetComponent<TModel>( this );
		}

		/// <summary>
		/// Gets the planet that this model is attached to
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Raises <see cref="ModelChanged"/> with empty arguments
		/// </summary>
		protected void OnModelChanged( )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, ModelChangedEventArgs.Empty );
			}
		}

		/// <summary>
		/// Raises <see cref="ModelChanged"/> with the specifed arguments
		/// </summary>
		/// <param name="args">Event arguments</param>
		protected void OnModelChanged( ModelChangedEventArgs args )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, args );
			}
		}

		#endregion

		#region Private Members

		private readonly IPlanet m_Planet;

		#endregion
	}

}
