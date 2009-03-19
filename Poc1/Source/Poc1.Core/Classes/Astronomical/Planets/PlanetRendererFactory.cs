using System;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Rb.Core.Utils;

namespace Poc1.Core.Classes.Astronomical.Planets
{
	/// <summary>
	/// Generic planet renderer factory
	/// </summary>
	/// <typeparam name="TPlanetRenderer">
	/// Planet renderer type. Must have a constructor that takes a single IPlanet-derived argument
	/// </typeparam>
	public class PlanetRendererFactory<TPlanetRenderer> : IPlanetRendererFactory
		where TPlanetRenderer : IPlanetRenderer
	{
		/// <summary>
		/// Gets the singleton instance of this factory
		/// </summary>
		public static IPlanetRendererFactory Instance
		{
			get { return s_Instance; }
		}

		#region IPlanetRendererFactory Members

		/// <summary>
		/// Creates an instance of TPlanetRenderer
		/// </summary>
		/// <param name="planet">Planet to attach the renderer to</param>
		/// <returns>Returns the new planet renderer</returns>
		public IPlanetRenderer Create( IPlanet planet )
		{
			Arguments.CheckNotNull( planet, "planet" );
			TPlanetRenderer renderer = ( TPlanetRenderer )Activator.CreateInstance( typeof( TPlanetRenderer ), planet );
			return renderer;
		}

		#endregion

		#region Private Members

		private static readonly IPlanetRendererFactory s_Instance = new PlanetRendererFactory<TPlanetRenderer>( );

		#endregion
	}
}
