
namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Interface that determines the life of a particle
	/// </summary>
	public interface IParticleKiller
	{
		/// <summary>
		/// Kills particles
		/// </summary>
		void KillParticles( IParticleFactory factory, System.Collections.IEnumerable particles );
	}
}
