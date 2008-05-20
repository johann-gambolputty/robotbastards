
namespace Poc1.Particles.Interfaces
{
	public interface ISpawnRate
	{
		/// <summary>
		/// Returns the number of particles to spawn on this update step
		/// </summary>
		int GetNumberOfParticlesToSpawn( );
	}
}
