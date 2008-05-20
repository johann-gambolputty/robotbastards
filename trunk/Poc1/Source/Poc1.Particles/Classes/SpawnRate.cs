using System;
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
		/// Gets/sets the minimum number of particles that can be generated each update
		/// </summary>
		public int MinimumRate
		{
			get { return m_MinimumRate; }
			set { m_MinimumRate = value; }
		}

		/// <summary>
		/// Gets/sets the maximum number of particles that can be generated each update
		/// </summary>
		public int MaximumRate
		{
			get { return m_MaximumRate; }
			set { m_MaximumRate = value; }
		}

		/// <summary>
		/// Sets the minimum and maximum rates to a single value (i.e. a fixed number of particles
		/// will be generated each update)
		/// </summary>
		public int Rate
		{
			set { m_MinimumRate = m_MaximumRate = value; }
		}

		#region ISpawnRate Members

		/// <summary>
		/// Returns the number of particles to spawn on this update step
		/// </summary>
		public abstract int GetNumberOfParticlesToSpawn( );

		#endregion

		#region Private Members

		private int m_MinimumRate;
		private int m_MaximumRate;

		#endregion
	}
}
