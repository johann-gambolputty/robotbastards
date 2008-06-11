
namespace Poc1.Particles.Interfaces
{
	public interface ISpawnRate
	{
		/// <summary>
		/// Returns the number of particles to spawn on this update step
		/// </summary>
		/// <param name="updateTime">Time, in seconds, since the last update</param>
		int GetNumberOfParticlesToSpawn( float updateTime );
	}
}
