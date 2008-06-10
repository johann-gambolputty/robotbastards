using System.ComponentModel;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Composite killer
	/// </summary>
	[Browsable(false)]
	public class ParticleCompositeKiller : ParticleSystemCompositeComponent<IParticleKiller>, IParticleKiller
	{
		#region IParticleKiller Members

		/// <summary>
		/// Kills particles
		/// </summary>
		public void KillParticles( IParticleSystem particles )
		{
			foreach ( IParticleKiller killer in Components )
			{
				killer.KillParticles( particles );
			}
		}

		#endregion
	}
}
