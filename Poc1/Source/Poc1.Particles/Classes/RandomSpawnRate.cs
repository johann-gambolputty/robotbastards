using System;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Chooses a random number of particles to spawn with each particle system update
	/// </summary>
	public class RandomSpawnRate : SpawnRate
	{
		public RandomSpawnRate( )
		{
		}

		public RandomSpawnRate( int rate ) :
			base( rate )
		{
		}

		public RandomSpawnRate( int minimumRate, int maximumRate ) :
			base( minimumRate, maximumRate )
		{
		}

		#region ISpawnRate Members

		/// <summary>
		/// Returns the number of particles to spawn on this update step
		/// </summary>
		public override int GetNumberOfParticlesToSpawn( float updateTime )
		{
			return m_Random.Next( MinimumRate, MaximumRate );
		}

		#endregion

		#region Private Members

		private readonly Random m_Random = new Random( 0 );

		#endregion

	}
}
