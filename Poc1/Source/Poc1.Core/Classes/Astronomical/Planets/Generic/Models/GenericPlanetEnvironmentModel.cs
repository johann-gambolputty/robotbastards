using System;
using Poc1.Core.Classes.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets;


namespace Poc1.Core.Classes.Astronomical.Planets.Generic.Models
{
	/// <summary>
	/// Generic environment model that is attached to specifically typed planet and planet models
	/// </summary>
	/// <typeparam name="TPlanet">Planet type</typeparam>
	/// <typeparam name="TPlanetModel">Planet model type</typeparam>
	public class GenericPlanetEnvironmentModel<TPlanet, TPlanetModel> : AbstractPlanetEnvironmentModel
		where TPlanet : IPlanet
		where TPlanetModel : IPlanetModel
	{
		/// <summary>
		/// Gets the planet that this model is attached to (via the planet model)
		/// </summary>
		public new TPlanet Planet
		{
			get { return ( TPlanet )base.Planet; }
		}

		/// <summary>
		/// Gets the planet model that this model is a part of
		/// </summary>
		public new TPlanetModel PlanetModel
		{
			get { return ( TPlanetModel )base.PlanetModel; }
			set { base.PlanetModel = value; }
		}

		#region Protected Members

		/// <summary>
		/// Called after this environment model has been added to the specified planet model
		/// </summary>
		/// <param name="model">Planet model that this environment model was added to</param>
		protected override void OnAddedToPlanetModel( IPlanetModel model )
		{
			if ( !( model is TPlanetModel ) )
			{
				throw new ArgumentException( "Expected planet model to be of type " + typeof( TPlanetModel ) );
			}
			if ( !( model.Planet is TPlanet ) )
			{
				throw new ArgumentException( "Expected planet to be of type " + typeof( TPlanet ) );
			}
			base.OnAddedToPlanetModel( model );
		}

		#endregion
	}

}
