using System;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Rb.Core.Components.Generic;

namespace Poc1.Core.Interfaces.Astronomical.Planets
{
	/// <summary>
	/// Planet model
	/// </summary>
	public interface IPlanetModel : IComposite<IPlanetEnvironmentModel>
	{
		/// <summary>
		/// Event raised when a planet model changes
		/// </summary>
		event EventHandler<ModelChangedEventArgs> ModelChanged;

		/// <summary>
		/// Gets a typed model from this composite
		/// </summary>
		/// <typeparam name="TModel">Model type to retrieve</typeparam>
		/// <returns>Returns the first instance of type TModel in this composite, or null if none exist.</returns>
		TModel GetModel<TModel>( ) where TModel : IPlanetEnvironmentModel;

		/// <summary>
		/// Gets the planet that this model is attached to
		/// </summary>
		IPlanet Planet
		{
			get;
		}
	}
}
