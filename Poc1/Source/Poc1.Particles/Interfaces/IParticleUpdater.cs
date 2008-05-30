using System.Collections.Generic;

namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Interface for classes that update particle properties over their lifespan
	/// </summary>
	public interface IParticleUpdater
	{
		/// <summary>
		/// Updates all particles
		/// </summary>
		void Update( IParticleSystem ps, IEnumerable<IParticle> particles );
	}
}
