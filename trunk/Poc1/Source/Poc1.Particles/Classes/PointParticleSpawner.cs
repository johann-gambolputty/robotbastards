using System;
using System.ComponentModel;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Spawns particles at a single point
	/// </summary>
	public class PointParticleSpawner : IParticleSpawner
	{
		/// <summary>
		/// Gets/sets the particle system owner
		/// </summary>
		[Browsable( false )]
		public IParticleSystem Owner
		{
			get { return m_Owner; }
			set { m_Owner = value; }
		}

		#region IParticleSpawner Members

		/// <summary>
		/// Spawns a given number of particles
		/// </summary>
		public void SpawnParticles( IParticleFactory factory, int count, System.Collections.IList particles )
		{
			if ( m_Owner == null )
			{
				throw new InvalidOperationException( "Tried to spawn particles before setting the owner the spawner" );
			}

			for ( int i = 0; i < count; ++i )
			{
				object particle = factory.CreateParticle( );
				particles.Add( particle );
			}
		}

		#endregion

		#region Private Members

		private IParticleSystem m_Owner;

		#endregion
	}
}
