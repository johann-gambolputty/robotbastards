
using System.Collections.Generic;

namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Sets up spawn states for new particles
	/// </summary>
	/// <remarks>
	/// Provides some standard spawn states for new particles. 
	/// </remarks>
	public interface IParticleSpawner
	{

		/// <summary>
		/// Spawns a given number of particles
		/// </summary>
		void SpawnParticles( IParticleFactory factory, int count, IList<IParticle> particles );
	}
}
