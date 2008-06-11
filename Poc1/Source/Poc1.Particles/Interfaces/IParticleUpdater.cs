
namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Interface for classes that update particle properties over their lifespan
	/// </summary>
	public interface IParticleUpdater : IParticleSystemComponent
	{
		/// <summary>
		/// Updates all particles
		/// </summary>
		/// <param name="ps">Particle system to update</param>
		/// <param name="updateTime">Time since the last update, in seconds</param>
		void Update( IParticleSystem ps, float updateTime );
	}
}
