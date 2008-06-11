using System.ComponentModel;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Handy abstract base class for implementing <see cref="ISpawnRate"/>
	/// </summary>
	public abstract class SpawnRate : ISpawnRate
	{
		public SpawnRate( )
		{
		}

		public SpawnRate( int rate )
		{
			m_MinimumRate = rate;
			m_MaximumRate = rate;
		}

		public SpawnRate( int minimumRate, int maximumRate )
		{
			m_MinimumRate = minimumRate;
			m_MaximumRate = maximumRate;
		}

		/// <summary>
		/// Gets/sets the minimum spawn rate
		/// </summary>
		[Description( "The minimum number of particles that are spawned each update" )]
		public int MinimumRate
		{
			get { return m_MinimumRate; }
			set { m_MinimumRate = value; }
		}

		/// <summary>
		/// Gets/sets the maximum spawn rate
		/// </summary>
		[Description( "The maximum number of particles that are spawned each update" )]
		public int MaximumRate
		{
			get { return m_MaximumRate; }
			set { m_MaximumRate = value; }
		}

		/// <summary>
		/// Sets the minimum and maximum rates to a single value (i.e. a fixed number of particles
		/// will be generated each update)
		/// </summary>
		[Browsable( false )]
		public int Rate
		{
			set { m_MinimumRate = m_MaximumRate = value; }
		}

		#region ISpawnRate Members

		/// <summary>
		/// Returns the number of particles to spawn on this update step
		/// </summary>
		public abstract int GetNumberOfParticlesToSpawn( float udpateTime );

		#endregion

		#region Private Members

		private int m_MinimumRate;
		private int m_MaximumRate;

		#endregion
	}
}
