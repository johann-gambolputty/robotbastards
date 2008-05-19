
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
		/// Particle system centre
		/// </summary>
		Point3 Centre
		{
			get; set;
		}

		/// <summary>
		/// The object that sets up the initial state of newly spawned particles
		/// </summary>
		ISpawnStateGenerator SpawnStates
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the maximum number of particles. The default for this is int.MaxValue (i.e. uncapped)
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
		/// Gets/sets the object used to determine the spawn rate for particles
		/// </summary>
		ISpawnRate SpawnRate
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the minimum age at which particles are destroyed (age measured in updates)
		/// </summary>
		int MinimumDeathAge
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the minimum age at which particles are destroyed (age measured in updates)
		/// </summary>
		int MaximumDeathAge
		{
			get; set;
		}
	}
}
