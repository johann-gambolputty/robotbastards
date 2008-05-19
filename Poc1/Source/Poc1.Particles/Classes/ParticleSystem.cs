using Poc1.Particles.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Handy abstract base class for particle systems
	/// </summary>
	public abstract class ParticleSystem : IParticleSystem
	{
		#region IParticleSystem Members

		/// <summary>
		/// Particle system centre
		/// </summary>
		public Point3 Centre
		{
			get { return m_Centre; }
			set { m_Centre = value; }
		}

		/// <summary>
		/// The object that sets up the initial state of newly spawned particles
		/// </summary>
		public ISpawnStateGenerator SpawnStates
		{
			get { return m_SpawnStates; }
			set { m_SpawnStates = value; }
		}

		/// <summary>
		/// Gets/sets the maximum number of particles. The default for this is int.MaxValue (i.e. uncapped)
		/// </summary>
		public int MaximumNumberOfParticles
		{
			get { return m_MaximumNumberOfParticles; }
			set { m_MaximumNumberOfParticles = value; }
		}

		/// <summary>
		/// Enables/disables particle spawning.
		/// </summary>
		public bool EnableSpawning
		{
			get { return m_EnableSpawning; }
			set { m_EnableSpawning = value; }
		}

		/// <summary>
		/// Gets/sets the object used to determine the spawn rate for particles
		/// </summary>
		public ISpawnRate SpawnRate
		{
			get { return m_SpawnRate; }
			set { m_SpawnRate = value; }
		}

		/// <summary>
		/// Gets/sets the minimum age at which particles are destroyed (age measured in updates)
		/// </summary>
		public int MinimumDeathAge
		{
			get { return m_MinimumDeathAge; }
			set { m_MinimumDeathAge = value; }
		}

		/// <summary>
		/// Gets/sets the minimum age at which particles are destroyed (age measured in updates)
		/// </summary>
		public int MaximumDeathAge
		{
			get { return m_MaximumDeathAge; }
			set { m_MaximumDeathAge = value; }
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this particle system
		/// </summary>
		public abstract void Render( IRenderContext context );

		#endregion

		#region Private Members

		private Point3 m_Centre						= Point3.Origin;
		private bool m_EnableSpawning				= true;
		private ISpawnRate m_SpawnRate				= new RandomSpawnRate( 1 );
		private int m_MinimumDeathAge				= 10;
		private int m_MaximumDeathAge				= 10;
		private int m_MaximumNumberOfParticles		= 256;
		private ISpawnStateGenerator m_SpawnStates;

		#endregion
	}
}
