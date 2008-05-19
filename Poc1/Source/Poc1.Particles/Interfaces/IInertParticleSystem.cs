namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Particle system populated by particles that have no physics applied to them
	/// </summary>
	/// <remarks>
	/// A lightweight particle system - particles only store positions; no velocity or physical characteristics
	/// required.
	/// Particles are only generated when the particle system is moved 
	/// </remarks>
	public interface IInertParticleSystem : IParticleSystem
	{
	}
}
