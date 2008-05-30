using System.Collections.Generic;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Spawns particles at a single point
	/// </summary>
	public class PointParticleSpawner : IParticleSpawner
	{
		#region IParticleSpawner Members

		/// <summary>
		/// Spawns a given number of particles
		/// </summary>
		public void SpawnParticles( IParticleFactory factory, int count, IList<IParticle> particles )
		{
			for ( int i = 0; i < count; ++i )
			{
				IParticle particle = factory.CreateParticle( );
				particles.Add( particle );
			}
		}

		#endregion
	}
}
