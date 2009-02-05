
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Planets.Models.Templates;
using Rb.Core.Threading;

namespace Poc1.Universe.Planets.Spherical.Models.Templates
{
	/// <summary>
	/// Spherical planet cloud model template
	/// </summary>
	public class SpherePlanetCloudModelTemplate : PlanetCloudModelTemplate
	{
		/// <summary>
		/// Creates the cloud model
		/// </summary>
		protected override IPlanetCloudModel CreateCloudModel( )
		{
			//	TODO: AP: Need a way to specify which work queue to use
			return new SpherePlanetCloudModel( ExtendedThreadPool.Instance );
		}
	}
}
