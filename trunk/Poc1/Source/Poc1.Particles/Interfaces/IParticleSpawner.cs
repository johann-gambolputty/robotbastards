
namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Sets up spawn states for new particles
	/// </summary>
	/// <remarks>
	/// Provides some standard spawn states for new particles. 
	/// </remarks>
	public interface IParticleSpawner : IParticleSystemComponent
	{
		/// <summary>
		/// Spawns a given number of particles
		/// </summary>
		void SpawnParticles( IParticleSystem particles, int count );
	}
}
