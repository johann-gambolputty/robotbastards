
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Particle system interface
	/// </summary>
	/// <remarks>
	/// Very simple particle systems at the moment - could do with an upgrade.
	/// </remarks>
	public interface IParticleSystem : IRenderable
	{
		/// <summary>
		/// Particle system frame
		/// </summary>
		Matrix44 Frame
		{
			get;
		}

		/// <summary>
		/// Gets/sets the maximum number of particles. The default for this is 256
		/// </summary>
		int MaximumNumberOfParticles
		{
			get; set;
		}

		/// <summary>
		/// Enables/disables particle spawning.
		/// </summary>
		bool EnableSpawning
		{
			get; set;
		}

		/// <summary>
		/// Gets the object used to create new particles, and destroy old particles
		/// </summary>
		IParticleFactory ParticleFactory
		{
			get;
		}

		/// <summary>
		/// Gets/sets the object used to determine the spawn rate for particles
		/// </summary>
		/// <remarks>
		/// By default, each particle system should have a default spawn rate
		/// </remarks>
		ISpawnRate SpawnRate
		{
			get; set;
		}

		/// <summary>
		/// The object that sets up the initial state of newly spawned particles
		/// </summary>
		/// <remarks>
		/// If null, no particles will be spawned.
		/// By default, each particle system should have a default spawner
		/// </remarks>
		IParticleSpawner Spawner
		{
			get; set;
		}

		/// <summary>
		/// The object that updates particle properties.
		/// </summary>
		/// <remarks>
		/// If null, particles remain inert (i.e. their initial spawn state is not changed over their lifetime).
		/// By default, this is null
		/// </remarks>
		IParticleUpdater Updater
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the object that determines the lifespan of particles. 
		/// </summary>
		/// <remarks>
		/// If null, particles will never be destroyed, until <see cref="MaximumNumberOfParticles"/> is reached, 
		/// at which point any newly spawned particles will replace the eldest particles in the system.
		/// By default, each particle system should have a default killer.
		/// </remarks>
		IParticleKiller Killer
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the object that renders the particle system
		/// </summary>
		/// <remarks>
		/// If null, particles will never be rendered.
		/// </remarks>
		IParticleRenderer Renderer
		{
			get; set;
		}
	}
}
