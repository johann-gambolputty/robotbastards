
namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Interface that determines the life of a particle
	/// </summary>
	public interface IParticleKiller : IParticleSystemComponent
	{
		/// <summary>
		/// Kills particles
		/// </summary>
		void KillParticles( IParticleSystem particles );
	}
}
