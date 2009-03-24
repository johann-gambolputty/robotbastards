using System;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models
{
	/// <summary>
	/// Planet environment model interface
	/// </summary>
	public interface IPlanetEnvironmentModel
	{
		/// <summary>
		/// Event raised when this model changes
		/// </summary>
		event EventHandler<ModelChangedEventArgs> ModelChanged;

		/// <summary>
		/// Gets the planet that this model is attached to (via the planet model)
		/// </summary>
		IPlanet Planet
		{
			get;
		}

		/// <summary>
		/// Gets/sets the planet model that this model is a part of
		/// </summary>
		IPlanetModel PlanetModel
		{
			get; set;
		}

		/// <summary>
		/// Invokes the correct Visit() call on the specified visitor object
		/// </summary>
		/// <typeparam name="TReturn">Visitor return type</typeparam>
		/// <param name="visitor">Visitor interface to invoke</param>
		/// <returns>Returns the result of the visit call</returns>
		TReturn InvokeVisit<TReturn>( IPlanetEnvironmentModelVisitor<TReturn> visitor );
	}
}
