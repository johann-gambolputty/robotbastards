using System.Collections.Generic;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Implements <see cref="IParticleUpdater"/>. Does nothing.
	/// </summary>
	public class NullParticleUpdater : IParticleUpdater
	{
		#region IParticleUpdater Members

		/// <summary>
		/// Updates all particles
		/// </summary>
		public void Update( IParticleSystem ps, IEnumerable<IParticle> particles )
		{
		}

		#endregion
	}
}
