using System;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Rb.Core.Utils;

namespace Poc1.Core.Classes.Astronomical.Planets
{
	/// <summary>
	/// Generic planet model factory
	/// </summary>
	/// <typeparam name="TPlanetModel">
	/// Planet model type. Must have a constructor that takes a single IPlanet-derived argument
	/// </typeparam>
	public class PlanetModelFactory<TPlanetModel> : IPlanetModelFactory
		where TPlanetModel : IPlanetModel
	{
		/// <summary>
		/// Gets the singleton instance of this factory
		/// </summary>
		public static IPlanetModelFactory Instance
		{
			get { return s_Instance; }
		}

		#region IPlanetModelFactory Members

		/// <summary>
		/// Creates an instance of TPlanetModel
		/// </summary>
		/// <param name="planet">Planet to attach the model to</param>
		/// <returns>Returns the new planet model</returns>
		public IPlanetModel Create( IPlanet planet )
		{
			Arguments.CheckNotNull( planet, "planet" );
			TPlanetModel model = ( TPlanetModel )Activator.CreateInstance( typeof( TPlanetModel ), planet );
			return model;
		}

		#endregion

		#region Private Members

		private static readonly IPlanetModelFactory s_Instance = new PlanetModelFactory<TPlanetModel>( );

		#endregion
	}
	
}
