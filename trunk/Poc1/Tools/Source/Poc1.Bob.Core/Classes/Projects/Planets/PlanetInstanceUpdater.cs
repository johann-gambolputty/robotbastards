
using System;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Planets.Models.Templates;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Projects.Planets
{
	/// <summary>
	/// Updates the current planet instance when the underlying template in the current planet
	/// project changes
	/// </summary>
	public class PlanetInstanceUpdater
	{
		/// <summary>
		/// Setup constructo
		/// </summary>
		/// <param name="planetTemplate">Planet template</param>
		/// <param name="planet">Planet to update when the template changes</param>
		/// <exception cref="System.ArgumentNullException">Thrown if planetTemplate or planet are null</exception>
		public PlanetInstanceUpdater( PlanetModelTemplate planetTemplate, IPlanet planet )
		{
			Arguments.CheckNotNull( planetTemplate, "planetTemplate" );
			Arguments.CheckNotNull( planet, "planet" );
			planetTemplate.TemplateChanged += OnTemplateChanged;
		}

		#region Private Members

		/// <summary>
		/// Called when the planet template changes
		/// </summary>
		private void OnTemplateChanged( object sender, EventArgs args )
		{
		}

		#endregion
	}
}
