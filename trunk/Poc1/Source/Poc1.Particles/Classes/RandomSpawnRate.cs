using System;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	public class RandomSpawnRate : ISpawnRate
	{
		public RandomSpawnRate( )
		{
		}

		public RandomSpawnRate( int rate )
		{
			m_MinimumRate = rate;
			m_MaximumRate = rate;
		}

		public RandomSpawnRate( int minimumRate, int maximumRate )
		{
			m_MinimumRate = minimumRate;
			m_MaximumRate = maximumRate;
		}

		public int MinimumRate
		{
			get { return m_MinimumRate; }
			set { m_MinimumRate = value; }
		}

		public int MaximumRate
		{
			get { return m_MaximumRate; }
			set { m_MaximumRate = value; }
		}

		public int Rate
		{
			set { m_MinimumRate = m_MaximumRate = value; }
		}

		#region ISpawnRate Members

		public int GetNumberOfParticlesToSpawn( )
		{
			return m_Random.Next( MinimumRate, MaximumRate );
		}

		#endregion

		#region Private Members

		private int m_MinimumRate;
		private int m_MaximumRate; 
		private readonly Random m_Random = new Random( 0 );

		#endregion

	}
}
