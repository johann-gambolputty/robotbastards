using Poc1.Particles.Interfaces;
using System.Collections.Generic;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Kills particles when they reach a certain age
	/// </summary>
	public class ParticleAgeKiller : IParticleKiller
	{
		#region Public Members

		/// <summary>
		/// Gets/set the age at which particles die
		/// </summary>
		public int DeathAge
		{
			get { return m_DeathAge; }
			set { m_DeathAge = value; }
		}

		#endregion

		#region IParticleKiller Members

		/// <summary>
		/// Kills particles
		/// </summary>
		public void KillParticles( IParticleSystem particles )
		{
			ParticleFieldIterator iter = new ParticleFieldIterator( particles.Buffer, ParticleBase.Age );
			for ( int i = 0; i < particles.Buffer.NumActiveParticles; ++i )
			{
				iter.GetPosition( );
			}
		}

		#endregion

		#region Private Members

		private int m_DeathAge = 10;

		#endregion

	}
}
