using Poc1.Particles.Interfaces;

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
			SerialParticleFieldIterator iter = new SerialParticleFieldIterator( ( ISerialParticleBuffer )particles.Buffer, ParticleBase.Age );
			for ( int i = 0; i < particles.Buffer.NumActiveParticles; ++i )
			{
				iter.MoveNext( );
				++iter.IntValue;
				if ( iter.IntValue > m_DeathAge )
				{
					particles.Buffer.MarkParticleForRemoval( i );
				}
			}
			particles.Buffer.RemoveMarkedParticles( );
		}

		#endregion

		#region IParticleSystemComponent Members

		/// <summary>
		/// Called when this component is attached to a particle system
		/// </summary>
		public void Attach( IParticleSystem particles )
		{
			particles.Buffer.AddField( ParticleBase.Age, ParticleFieldType.Int32, 1, 0 );
		}

		#endregion

		#region Private Members

		private int m_DeathAge = 10;

		#endregion

	}
}
