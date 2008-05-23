
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles
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
			get;
		}

		/// <summary>
		/// Gets/sets the minimum birth rate (minimum number of particles that can be generated per update)
		/// </summary>
		int MinimumBirthRate
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the maximum birth rate (maximum number of particles that can be generated per update)
		/// </summary>
		int MaximumBirthRate
		{
			get; set;
		}

		/// <summary>
		/// Sets the minimum and maximum birth rates (so each update generates a fixed number of particles)
		/// </summary>
		int BirthRate
		{
			set;
		}
	}
}
